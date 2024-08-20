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
    protected EntityState entityState;

    public enum EntityStatusEffect
    {
        None,
        Fear,
        Sleep,
        Stun,
        TotalStatus
    }
    protected EntityStatusEffect entityStatusEffect;
    private float statusCounter;

    public EntityStatsScriptableObject _inputStats;

    //Animation controller
    protected Animator animator;
    //sprite renderer
    private SpriteRenderer spriteRenderer;

    protected EntityStats entityStats;

    public bool isEnemy;
    public bool isDead;
    public bool isTargetable;

    //check if entity has been initialize
    public bool hasInit = false;

    //attack counter
    protected float attackCounter;
    //private float attackAnimCounter;

    protected Transform _targetPoint;

    //Active attributes/multiplier
    protected float activeMovementValue;
    protected float activeAttackMult;
    protected float activeDamageTakenMult;

    protected float currWeight;

    // Start is called before the first frame update
    public virtual void Init(Transform targetPoint)
    {
        //GetComponent
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.sortingOrder = 100 + Random.Range(0, 6);
        
        //Stats Initialization
        hasInit = true;
        entityStats = new EntityStats();
        SetStats(_inputStats);
        entityState = EntityState.Walk;
        attackCounter = 0;
        isDead = false;
        _targetPoint = targetPoint;
        isTargetable = true;
        currWeight = entityStats.weight;

        entityStatusEffect = EntityStatusEffect.None;
        statusCounter = 0;

        //active val
        activeMovementValue = 1;
        activeAttackMult = 1;
        activeDamageTakenMult = 1;


        //Rotate Image
        spriteRenderer.flipX = !isEnemy;
    }

    // Update is called once per frame
    public virtual void HandleUpdate()
    {
        //DEBUGGING ONLY
        //Draw Attack Range
        //Vector3 dir = _targetPoint.position - transform.position;
        //dir.Normalize();
        //Debug.DrawRay(transform.position + dir * entityStats.minAttackRange, dir * entityStats.maxAttackRange, Color.green, 0.01f);

        HandleStatusEffect();
        HandlePassiveTrait();

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
        if (attackCounter <= 0 && entityState == EntityState.Idle && entityStatusEffect == EntityStatusEffect.None)
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
                transform.position = Vector2.MoveTowards(transform.position, _targetPoint.position, entityStats.movementSpeed * 1 * Time.deltaTime);
                break;
            case EntityState.Idle:
                break;
            case EntityState.Attack:
                //DEBUGGING ONLY
                /*if (!animator)
                    break;
                if (entityStats.attackCooldown - attackCounter >= animator.GetCurrentAnimatorClipInfo(0).Length)
                {
                    //change back after animation finish
                    animator.SetBool("IsAttacking", false);
                    entityState = EntityState.Idle;
                }*/
                break;
            case EntityState.Death:
                //DEBUGGING ONLY
                if (!animator)
                {
                    isDead = true;
                }
                break;
            default:
                break;
        }
    }

    //assign stats
    protected void SetStats(EntityStatsScriptableObject _newStats)
    {
        entityStats.entityName = _newStats.entityName;
        entityStats.level = _newStats.level;
        entityStats.cost = _newStats.cost;
        entityStats.upgradeCost = _newStats.upgradeCost;
        entityStats.attackDamage = _newStats.attackDamage;
        entityStats.health = _newStats.health;
        entityStats.maxHealth = _newStats.health;
        entityStats.movementSpeed = _newStats.movementSpeed;
        entityStats.detectRange = _newStats.detectRange;
        entityStats.minAttackRange = _newStats.minAttackRange;
        entityStats.maxAttackRange = _newStats.maxAttackRange;
        entityStats.weight = _newStats.weight;
        entityStats.isAreaOfEffect = _newStats.isAreaOfEffect;
        entityStats.attackCooldown = _newStats.attackCooldown;
        entityStats.attackTraitCooldown = _newStats.attackTraitCooldown;
        entityStats.passiveTraitTriggerDuration = _newStats.passiveTraitTriggerDuration;
        entityStats.passiveTraitDuration = _newStats.passiveTraitDuration;
        entityStats.attackTraitPercentage = _newStats.attackTraitPercentage;
        entityStats.moneyEarnedOnDeath = _newStats.moneyEarnedOnDeath;
}

    public EntityStats GetStats()
    { 
        //DebugUtility.AssertNotNull(entityStats, "Entity stats was null");
        return entityStats; 
    }

    public EntityState GetState()
    {
        return entityState;
    }

    public void SetState(EntityState _newState)
    {
        entityState = _newState;
    }

    public EntityStatusEffect GetStatusEffect()
    {
        return entityStatusEffect;
    }

    public float GetWeight()
    {
        return currWeight;
    }

    public void SetWeight(float newWeight)
    {
        currWeight = newWeight;
    }

    public void Damage(int _amt)
    {
        if (entityState == EntityState.Death)
            return;

        entityStats.health -= (int)(_amt * activeDamageTakenMult);
        if (entityStats.health < 0)
        {
            entityState = EntityState.Death;
            //animation change to death
            if (animator)
            {
                animator.SetBool("IsDead", true);
            }
        }
    }

    public bool DetectedEnemy(bool hasDetectedEnemy)
    {

        if (entityStatusEffect != EntityStatusEffect.None && entityState != EntityState.Attack)
        {
            return true;
        }
        

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

    public virtual void HandlePassiveTrait()
    {

    }

    public virtual void HandleActiveTrait(float _scaleAngle)
    {

    }

    public void ApplyStatusEffect(EntityStatusEffect _effect, float _duration = 0)
    {
        if (entityStatusEffect != EntityStatusEffect.None)
            return;

        switch(_effect)
        {
            case EntityStatusEffect.Fear:
                statusCounter = 1f;
                //flip character
                spriteRenderer.flipX = isEnemy;
                entityStatusEffect = EntityStatusEffect.Fear;
                break;

            case EntityStatusEffect.Sleep:
                statusCounter = (_duration == 0) ? 4f : _duration;

                entityStatusEffect = EntityStatusEffect.Sleep;
                break;

            case EntityStatusEffect.Stun:
                statusCounter = (_duration == 0) ? 2f : _duration;
                entityStatusEffect = EntityStatusEffect.Stun;
                break;

            default:
                break;
        }
    }

    protected void DealStatusEffect(EntityStatusEffect _effect, int totalEntitiesAffected)
    {
        EntityController.Instance.ApplyStatusEffect(this, _effect, totalEntitiesAffected);
    }

    protected virtual void HandleAttackTrait()
    {

    }

    protected void HandleStatusEffect()
    {
        //if no status, then don't enter
        if (entityStatusEffect == EntityStatusEffect.None)
        {
            return;
        }
            

        if (statusCounter > 0)
        {
            statusCounter -= Time.deltaTime;

            //status time end
            if (statusCounter <= 0)
            {
                //reset
                entityStatusEffect = EntityStatusEffect.None;
                spriteRenderer.flipX = !isEnemy;
            }
        }

        if (entityStatusEffect == EntityStatusEffect.Fear)
        {
            //walk backwards
            activeMovementValue = -0.8f;
            //set to walk
            entityState = EntityState.Walk;
        }

        else if (entityStatusEffect == EntityStatusEffect.Sleep || entityStatusEffect == EntityStatusEffect.Stun)
        {
            //cannot move
            entityState = EntityState.Idle;
        }
    }

    //Animation Events
    public void Attack()
    {
        EntityController.Instance.HandleEntityAttack(this);
        HandleAttackTrait();
    }

    public void AttackAnimEnd()
    {
        animator.SetBool("IsAttacking", false);
        entityState = EntityState.Idle;
    }

    public void DeathAnimEnd()
    {
        isDead = true;
    }

    //Atributes
    public int GetAttackDamage()
    {
        return (int)(entityStats.attackDamage * activeAttackMult);
    }

    public void SetHealth(int _health)
    {
        entityStats.health = _health;
        entityStats.maxHealth = _health;
    }

    public virtual Vector3 GetPos()
    {
        return transform.position;
    }
}
