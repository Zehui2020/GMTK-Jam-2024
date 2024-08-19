using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Entity", fileName = "Entity")]
public class Entity : ScriptableObject
{
    public EntityStatsScriptableObject _stats;
    public GameObject _entityPrefab;
    public Sprite placeholderSprite;
}