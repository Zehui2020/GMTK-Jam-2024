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
    //sprite renderer
    private SpriteRenderer spriteRenderer;

    private EntityStats entityStats;

    public bool isEnemy;
    public bool isDead;

    //check if entity has been initialize
    public bool hasInit = false;

    //attack counter
    private float attackCounter;
    //private float attackAnimCounter;
    private float deathCounter;

    private Transform _targetPoint;

    // Start is called before the first frame update
    public void Init(Transform targetPoint)
    {
        //GetComponent
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        //Stats Initialization
        hasInit = true;
        entityStats = new EntityStats();
        SetStats(_inputStats);
        entityState = EntityState.Walk;
        attackCounter = 0;
        deathCounter = 0;
        isDead = false;
        _targetPoint = targetPoint;

        //Rotate Image
        spriteRenderer.flipX = !isEnemy;
    }

    // Update is called once per frame
    public void HandleUpdate()
    {
        //attack counter countdown
        if (attackCounter > 0)
        {
            attackCounter -= Time.deltaTime;
            
            //DEBUGGING ONLY
            if (entityState == EntityState.Attack && !animator)
            {
                Attack();
                entityState = EntityState.Idle;
            }
        }
        //if countdown reach zero and entity is Idling
        if (attackCounter <= 0 && entityState == EntityState.Idle)
        {
            //change to attack
            entityState = EntityState.Attack;
            attackCounter = entityStats.attackCooldown;
            //activate animations
            if (animator)
                animator.SetBool("IsAttacking", true);
            
        }

        switch (entityState)
        {
            case EntityState.Walk:
                transform.position = Vector2.MoveTowards(transform.position, _targetPoint.position, entityStats.movementSpeed * Time.deltaTime);
                break;
            case EntityState.Idle:
                break;
            case EntityState.Attack:
                //DEBUGGING ONLY
                if (!animator)
                    break;
                if (entityStats.attackCooldown - attackCounter >= animator.GetCurrentAnimatorClipInfo(0).Length)
                {
                    //change back after animation finish
                    animator.SetBool("IsAttacking", false);
                    entityState = EntityState.Idle;
                }
                break;
            case EntityState.Death:
                deathCounter -= Time.deltaTime;
                if (deathCounter <= 0)
                    isDead = true;
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

    public EntityState GetState()
    {
        return entityState;
    }

    public void Attack()
    {
        EntityController.Instance.HandleEntityAttack(this);
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
            //animation change to death
            if (animator)
            {
                animator.SetBool("IsDead", true);
                //set counter
                deathCounter = animator.GetCurrentAnimatorClipInfo(0).Length;
            }
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

        if (animator)
            animator.SetBool("IsWalking", entityState == EntityState.Walk);


        return hasDetectedEnemy;
    }
}
