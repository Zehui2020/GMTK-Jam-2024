using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEntity : MonoBehaviour
{
    public enum EntityState
    {
        Walk,
        Idle,
        Attack,
        Death,
        TotalStates
    }
    private EntityState entityState;

    public EntityStats _inputStats;

    //Animation controller
    private Animator animator;

    private EntityStats entityStats;

    public bool isEnemy;
    public bool isDead;

    //check if entity has been initialize
    public bool hasInit = false;

    //attack counter
    private float attackCounter;

    private Transform _targetPoint;

    // Start is called before the first frame update
    public void Init(Transform targetPoint)
    {
        hasInit = true;
        entityStats = new EntityStats();
        SetStats(_inputStats);
        entityState = EntityState.Walk;
        attackCounter = 0;
        isDead = false;
        _targetPoint = targetPoint;
    }

    // Update is called once per frame
    public void HandleUpdate()
    {
        //attack counter countdown
        if (attackCounter > 0)
        {
            attackCounter -= Time.deltaTime;
        }
        //if countdown reach zero and entity is Idling
        if (attackCounter <= 0 && entityState == EntityState.Idle)
        {
            //change to attack
            entityState = EntityState.Attack;
            attackCounter = entityStats.attackCooldown;
            Debug.Log(entityStats.attackCooldown);
            //activate animations
        }

        switch (entityState)
        {
            case EntityState.Walk:
                transform.position = Vector3.MoveTowards(transform.position, _targetPoint.position, entityStats.movementSpeed * Time.deltaTime);
                break;
            case EntityState.Idle:
                break;
            case EntityState.Attack:
                Attack();
                SetState(EntityState.Idle);
                break;
            case EntityState.Death:
                break;
            default:
                break;
        }
    }

    //assign stats
    private void SetStats(EntityStats _newStats)
    {
        entityStats.entityName = _newStats.entityName;
        entityStats.level = _newStats.level;
        entityStats.cost = _newStats.cost;
        entityStats.upgradeCost = _newStats.upgradeCost;
        entityStats.attackDamage = _newStats.attackDamage;
        entityStats.health = _newStats.health;
        entityStats.movementSpeed = _newStats.movementSpeed;
        entityStats.detectRange = _newStats.detectRange;
        entityStats.minAttackRange = _newStats.minAttackRange;
        entityStats.maxAttackRange = _newStats.maxAttackRange;
        entityStats.weight = _newStats.weight;
        entityStats.isAreaOfEffect = _newStats.isAreaOfEffect;
        entityStats.attackCooldown = _newStats.attackCooldown;
    }

    public EntityStats GetStats()
    { 
        return entityStats; 
    }

    public void Attack()
    {
        EntityController.Instance.HandleEntityAttack(this);
        Debug.Log((isEnemy? "Enemy" : "Ally") + " Attack");
    }

    public void SetState(EntityState _newState)
    {
        entityState = _newState;
    }

    public void Damage(int _amt)
    {
        if (entityState == EntityState.Death)
            return;

        entityStats.health -= _amt;
        if (entityStats.health < 0)
        {
            entityState = EntityState.Death;
            isDead = true;
            Debug.Log((isEnemy ? "Enemy" : "Ally") + " Die");
        }
    }

    public bool DetectedEnemy(bool hasDetectedEnemy)
    {
        //go into idle if detected enemy
        if (hasDetectedEnemy && entityState == EntityState.Walk)
        {
            entityState = EntityState.Idle;
        }
        //continue moving forward if do not detect enemy
        else if (!hasDetectedEnemy && entityState == EntityState.Idle)
        {
            entityState = EntityState.Walk;
        }

        return hasDetectedEnemy;
    }
}
