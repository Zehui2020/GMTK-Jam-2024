using UnityEngine;

using static DebugUtility;

public class LevelController : MonoBehaviour
{
    // TODO (Chris): To be set from the selection screen, or a manager
    public Level CurrentLevel { get; set; }

    private int _waveIndex; 
    private float _timer;
    private bool _isRunning = false;

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

    private void Update()
    {
        if (!_isRunning)
        {
            return;
        }

        _timer += Time.deltaTime;
    } 
}