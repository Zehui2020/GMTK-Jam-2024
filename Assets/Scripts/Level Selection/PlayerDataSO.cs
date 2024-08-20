using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/PlayerDataSO")]
public class PlayerDataSO : ScriptableObject
{
    public int MaxUnlockedLevel;

    private void Reset()
    {
        MaxUnlockedLevel = 0;
    }
}
