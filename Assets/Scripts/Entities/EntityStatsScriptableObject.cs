using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/EntityStatsScriptableObject")]
public class EntityStatsScriptableObject : ScriptableObject
{
    public string entityName;
    public int level;
    public int cost;
    public int upgradeCost;
    public int attackDamage;
    public int health;
    public float movementSpeed;
    public float detectRange;
    public float minAttackRange;
    public float maxAttackRange;
    public float weight;
    public bool isAreaOfEffect;
    public float attackCooldown;
    public float attackTraitCooldown;
    public float passiveTraitTriggerDuration;
    public float passiveTraitDuration;
    public float attackTraitPercentage;
}