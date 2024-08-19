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

    public enum EntityStatusEffect
    {
        None,
        Fear,
        TotalStatus
    }
    private EntityStatusEffect entityStatusEffect;
    private float statusCounter;

    public EntityStats _inputStats;

    //Animation controller
    private Animator animator;
    //sprite renderer
    private SpriteRenderer spriteRenderer;

    protected EntityStats entityStats;

    public bool isEnemy;
    public bool isDead;

    //check if entity has been initialize
    public bool hasInit = false;

    //attack counter
    private float attackCounter;
    //private float attackAnimCounter;
    private float deathCounter;

    private Transform _targetPoint;

    protected float activeMovementValue;

    // Start is called before the first frame update
    public void Init(Transform targetPoint)
    {
        //GetComponent
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        //Stats Initialization
        hasInit = true;
        entityStats = ScriptableObject.CreateInstance<EntityStats>(); 
        SetStats(_inputStats);
        entityState = EntityState.Walk;
        attackCounter = 0;
        deathCounter = 0;
        isDead = false;
        _targetPoint = targetPoint;
        activeMovementValue = 1;
        entityStatusEffect = EntityStatusEffect.None;
        statusCounter = 0;

        //Rotate Image
        spriteRenderer.flipX = !isEnemy;
    }

    // Update is called once per frame
    public void HandleUpdate()
    {
        //DEBUGGING ONLY
        //Draw Attack Range
        Vector3 dir = _targetPoint.position - transform.position;
        dir.Normalize();
        Debug.DrawRay(transform.position + dir * entityStats.minAttackRange, dir * entityStats.maxAttackRange, Color.green, 0.01f);

        HandleStatusEffect();

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
        DebugUtility.AssertNotNull(entityStats, "Entity stats was null");
        return entityStats; 
    }

    public EntityState GetState()
    {
        return entityState;
    }

    public void Attack()
    {
        EntityController.Instance.HandleEntityAttack(this);
        HandleAttackTrait();
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

    public virtual void HandlePassiveTrait()
    {

    }

    public virtual void HandleActiveTrait(float _scaleAngle)
    {

    }

    public void ApplyStatusEffect(EntityStatusEffect _effect)
    {
        if (entityStatusEffect != EntityStatusEffect.None)
            return;

        switch(_effect)
        {
            case EntityStatusEffect.Fear:
                statusCounter = 1f;
                //flip character
                spriteRenderer.flipX = isEnemy;
                break;
            default:
                break;
        }
    }

    protected void DealStatusEffect(EntityStatusEffect _effect, int totalEntitiesAffected)
    {

    }

    protected virtual void HandleAttackTrait()
    {

    }

    private void HandleStatusEffect()
    {
        statusCounter -= Time.deltaTime;
        //status time end
        if (statusCounter <= 0)
        {
            //reset
            entityStatusEffect = EntityStatusEffect.None;
            spriteRenderer.flipX = !isEnemy;
        }


        if (entityStatusEffect == EntityStatusEffect.Fear)
        {
            activeMovementValue = -0.8f;
            //set to walk
            entityState = EntityState.Walk;
        }
    }
}
