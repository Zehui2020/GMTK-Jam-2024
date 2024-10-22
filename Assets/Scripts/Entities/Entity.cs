using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Entity", fileName = "Entity")]
public class Entity : ScriptableObject
{
    public EntityStatsScriptableObject _stats;
    public GameObject _entityPrefab;
    public Sprite placeholderSprite;
    public Sprite selectionIcon;

    public string entityName;
    [TextArea(2, 10)]
    public string abilities;
}