using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Player Settings", fileName = "PlayerSettings")]
public class PlayerSettings : ScriptableObject
{
    public float masterVolume;
    public float bgmVolume;
    public float sfxVolume;

    public void ResetVolume()
    {
        masterVolume = 1.0f;
        bgmVolume = 1.0f;
        sfxVolume = 1.0f;
    }
}