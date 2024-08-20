using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using static DebugUtility;
using UnityEditor;
using UnityEngine.Events;

public class LevelController : MonoBehaviour
{
    private struct EntitySpawn 
    {
        public GameObject Prefab;
        public float SpawnTime;
        public float Position;
    }

    [Header("LevelController Dependencies")]

    [SerializeField]
    private LevelSelectionSharedData _selectionSharedData;

    [field: Tooltip("Leave this empty if there is no level to debug")]
    [field: SerializeField]
    private Level _debugLevel;

    [Header("LevelController Settings")]

    [SerializeField]
    private bool _loop = false;

    [Header("LevelController Events")]

    [SerializeField]
    private UnityEvent<int> _onNextWave;

    [SerializeField]
    private UnityEvent<BaseEntity> _onEntitySpawn;

    [Header("LevelController Affect")]

    [SerializeField]
    List<GameObject> _troopButtons;

    private bool _isRunning = false;
    private int _waveIndex; 
    private float _timer;

    private float _lastNodeTime = 0.0f;

    private Level _currentLevel; 
    private Wave _currentWave;
    private readonly List<EntitySpawn> _currentEntitySpawns = new();

    public void SetLevel(Level level, bool play = true)
    {
        Init(level);

        if (play)
        {
            Play();
        }
    }

    public void Init(Level level)
    {
        _currentEntitySpawns.Clear();
        _currentLevel = level; 

        AssertNotNull(_currentLevel, "Current level is null");
        Assert(!_currentLevel.Waves.IsEmpty(), "Current level waves are empty");

        InitValues();

        for (int buttonno = 0; buttonno < _troopButtons.Count; buttonno++)
        {
            if (!level.buttonUses[buttonno])
            {
                _troopButtons[buttonno].SetActive(false);
            }
        }
    }
    
    public void Play()
    {
        _isRunning = true;
    }

    public void Stop()
    {
        _isRunning = false;
    }

    public Level GetLevel()
    {
        return _currentLevel;
    }
    private void InitValues()
    {
        // Init gameplay variables
        _waveIndex = 0;
        _timer = 0.0f;

        _currentWave = _currentLevel.Waves[0];
        OnNextWave(_currentWave);
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

        _onNextWave.Invoke(_waveIndex);
    }

    private void Awake()
    {
        // NOTE (Chris): To be removed, should be 
        // started from the `GameController`
        if (_debugLevel)
        {
            // For debugging purposes, prioritise the debug level
            Init(_debugLevel);
        }
        else if (_selectionSharedData && _selectionSharedData.Level)
        {
            Init(_selectionSharedData.Level);
        }
        else
        {
            Fatal("No level data to initialize with");
            return;
        }

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
                _onEntitySpawn.Invoke(EntityController.Instance.SpawnEntity(
                    entitySpawn.Prefab, true));

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

            if (_waveIndex == _currentLevel.Waves.Count - 1)
            {
                // Stop
                if (!_loop)
                {
                    _isRunning = false;
                }
                else
                {
                    InitValues();
                }
            }
            else
            {
                // Go to the next wave if exists
                _currentWave = _currentLevel.Waves[++_waveIndex];
                OnNextWave(_currentWave);
            }
        }
    }
}