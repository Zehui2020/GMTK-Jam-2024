using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using static DebugUtility;

public class LevelController : MonoBehaviour
{
    // TODO (Chris): To be set from the selection screen, or a manager
    public Level CurrentLevel { get; set; }

    private int _waveIndex; 
    private float _timer;

    private float _lastNodeTime = 0.0f;

    private bool _isRunning = false;
    private Wave _currentWave;
    private List<WaveNode> _currentWaveNodes;

    public void Init()
    {
        AssertNotNull(CurrentLevel, "Current level is null");
        Assert(CurrentLevel.Waves.IsEmpty(), "Current level waves are empty");

        // Init gameplay variables
        _waveIndex = 0;
        _timer = 0.0f;
    }
    
    public void Play()
    {
        _isRunning = true;
    }

    private void OnNextWave(Wave wave)
    {
        // NOTE (Chris): This will definitely change once we have sequence nodes
        // Calculate the total time taken for the last node
        _lastNodeTime = wave.Nodes.IsEmpty() ? 0.0f :
            wave.Nodes.Select(node => node.SpawnAfterStartSeconds).Max();

        // Shallow copy the nodes
        _currentWaveNodes = wave.Nodes;
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
        for (int i = 0; i < _currentWaveNodes.Count; ++i)
        {
            WaveNode node = _currentWaveNodes[i];
            if (node.SpawnAfterStartSeconds <= _timer)
            {
                // Spawn the node
                _currentWaveNodes.RemoveAt(i);
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