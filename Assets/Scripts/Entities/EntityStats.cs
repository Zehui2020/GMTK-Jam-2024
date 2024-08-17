using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EntityStatsScriptableObject")]
public class EntityStats : ScriptableObject
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
}