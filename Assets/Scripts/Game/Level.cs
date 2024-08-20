using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Level", fileName = "New Level")]
public class Level : ScriptableObject
{
    [field: Header("Level Dependencies")]

    [field: SerializeField]
    public List<Wave> Waves { get; private set; }

    public int levelID;

    public List<bool> buttonUses;
    private void OnEnable()
    {
        hideFlags = HideFlags.DontUnloadUnusedAsset;
    }
}