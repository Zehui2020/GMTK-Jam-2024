using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using static DebugUtility;
using UnityEditor;

public class LevelController : MonoBehaviour
{
    private struct EntitySpawn 
    {
        public GameObject Prefab;
        public float SpawnTime;
        public float Position;
    }

    [field: Header("LevelController Dependencies")]

    [field: Tooltip("Leave this empty if there is no level to debug")]
    [field: SerializeField]
    private Level _debugLevel;

    // TODO (Chris): To be set from the selection screen, or a manager
    public Level CurrentLevel { get; set; }

    private bool _isRunning = false;
    private int _waveIndex; 
    private float _timer;

    private float _lastNodeTime = 0.0f;

    private Wave _currentWave;
    private readonly List<EntitySpawn> _currentEntitySpawns = new();

    public void Init()
    {
        if (_debugLevel)
        {
            CurrentLevel = _debugLevel;
        }

        AssertNotNull(CurrentLevel, "Current level is null");
        Assert(!CurrentLevel.Waves.IsEmpty(), "Current level waves are empty");

        // Init gameplay variables
        _waveIndex = 0;
        _timer = 0.0f;

        _currentWave = CurrentLevel.Waves[0];
        OnNextWave(_currentWave);
    }
    
    public void Play()
    {
        Init();

        _isRunning = true;
    }

    private float GetEntitySpawnTime(WaveNode node)
    {
        return (Mathf.Max(node.SequenceAmount, 1) - 1) * 
            Mathf.Max(node.SequenceTimeGap, 0.1f) +
            node.SpawnAfterStartSeconds;
    }

    private void OnNextWave(Wave wave)
    {
        Assert(_currentEntitySpawns.IsEmpty(), 
            "Current entity spawns not cleared before next wave");

        // Calculate the total time taken for the last node
        _lastNodeTime = wave.Nodes.IsEmpty() ? 0.0f :
            wave.Nodes
                .Select(node => GetEntitySpawnTime(node))
                .Max();

        // Shallow copy the nodes
        foreach (var node in wave.Nodes)
        {
            int sequenceAmount = Mathf.Max(node.SequenceAmount, 1);
            float timeGap = Mathf.Max(node.SequenceTimeGap, 0.1f);

            for (int i = 0; i < sequenceAmount; ++i)
            {
                _currentEntitySpawns.Add(new() 
                {
                    Prefab = node.EntityPrefab,
                    SpawnTime = node.SpawnAfterStartSeconds + i * timeGap,
                    Position = node.PositionSpawn
                });
            }
        }
    }

    private void Awake()
    {
        Play();
    }

    private void Update()
    {
        if (!_isRunning)
        {
            return;
        }

        SpawnLoop();
    }

    private void QueryAndSpawnNodes()
    {
        for (int i = 0; i < _currentEntitySpawns.Count; ++i)
        {
            EntitySpawn entitySpawn = _currentEntitySpawns[i];
            if (entitySpawn.SpawnTime <= _timer)
            {
                // Spawn the node
                EntityController.Instance.SpawnEntity(entitySpawn.Prefab, true);

                _currentEntitySpawns.RemoveAt(i);
                --i;
            }
        }
    }

    private void SpawnLoop()
    {
        _timer += Time.deltaTime;

        QueryAndSpawnNodes();

        // Check if timer is past the last node time, and buffer of the wave
        if (_timer >= _lastNodeTime + _currentWave.LastNodeBuffer)
        {
            // Reset the timer
            _timer = 0.0f;

            if (_waveIndex == CurrentLevel.Waves.Count - 1)
            {
                // Stop
                _isRunning = false;
            }
            else
            {
                // Go to the next wave if exists
                _currentWave = CurrentLevel.Waves[++_waveIndex];
                OnNextWave(_currentWave);
            }
        }
    }
}