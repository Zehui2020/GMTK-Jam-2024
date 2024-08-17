using UnityEngine;

// This scriptable object will store runtime data
// The UI script will maintain this object's data
[CreateAssetMenu(menuName = "Scriptable Object/Shared Data/Level Selection",
    fileName = "LevelSelectionSharedData")]
public sealed class LevelSelectionSharedData : ScriptableObject
{
    public Level Level { get; set; } = null;

    private void OnEnable()
    {
        hideFlags = HideFlags.DontUnloadUnusedAsset;
    }
}