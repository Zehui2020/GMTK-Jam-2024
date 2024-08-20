using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyElephantBossEntity : BaseEntity
{
    [SerializeField] private GameObject _posPrefab;
    private GameObject ogPosObj;

    private bool hasAbilityActivated;
    private float passiveAbilityCounter;

    private float ogX, currX;
    private float lerpAttackCounter;

    private enum AttackState
    {
        Charging,
        Attacking,
        Returning
    }
    private AttackState attackState;

    Vector3 SetVector3Pos(Vector3 _pos)
    {
        return new Vector3(_pos.x, _pos.y, _pos.z);
    }

    public override void Init(Transform targetPoint)
    {
        base.Init(targetPoint);
        hasAbilityActivated = false;

        passiveAbilityCounter = 0;
        spriteRenderer.flipX = true;
        isEnemy = true;

        attackState = AttackState.Charging;
        lerpAttackCounter = 0;


        ogPosObj = Instantiate(_posPrefab);
        ogPosObj.transform.parent = transform.parent;
    }

    public override void HandleUpdate()
    {
        //DEBUGGING ONLY
        //Draw Attack Range
        /*Vector3 dir = _targetPoint.position - transform.position;
        dir.Normalize();
        Debug.DrawRay(transform.position + dir * entityStats.minAttackRange, dir * entityStats.maxAttackRange, Color.green, 0.01f);
        */
        hurtFlashLerpCounter += Time.deltaTime;
        spriteRenderer.color = Color.Lerp(spriteRenderer.color, Color.white, hurtFlashLerpCounter);

        HandleStatusEffect();
        HandlePassiveTrait();
        UpdateEffectIcon();

        //attack counter countdown
        if (attackCounter > 0)
        {
            attackCounter -= Time.deltaTime;
        }
        //if countdown reach zero and entity is Idling
        if (attackCounter <= 0 && entityState == EntityState.Idle && entityStatusEffect == EntityStatusEffect.None)
        {
            //change to attack
            entityState = EntityState.Attack;
            //attackCounter = entityStats.attackCooldown;
            attackState = AttackState.Charging;

            ogX = transform.localPosition.x;
            currX = transform.localPosition.x;

            ogPosObj.transform.position = SetVector3Pos(gameObject.transform.position);
        }

        switch (entityState)
        {
            case EntityState.Walk:
                transform.position = Vector2.MoveTowards(transform.position, _targetPoint.position, entityStats.movementSpeed * activeMovementValue * 1 * Time.deltaTime);
                break;
            case EntityState.Idle:
                break;
            case EntityState.Attack:
                {
                    
                    switch (attackState)
                    {
                        case AttackState.Charging:
                            lerpAttackCounter += Time.deltaTime / 2f;
                            transform.localPosition = Vector3.Lerp(new Vector3(currX, 0, 0), new Vector3(ogX + (isEnemy ? 1.5f : -1.5f), 0, 0), lerpAttackCounter);
                            if (lerpAttackCounter >= 1)
                            {
                                lerpAttackCounter = 0;
                                attackState = AttackState.Attacking;
                                animator.SetBool("IsAttacking", true);
                                currX = transform.localPosition.x;
                            }
                            break;

                        case AttackState.Attacking:
                            lerpAttackCounter += Time.deltaTime/0.8f;
                            transform.localPosition = Vector3.Lerp(new Vector3(currX, 0, 0), new Vector3(ogX + (isEnemy ? -1.5f : 1.5f), 0, 0), lerpAttackCounter);
                            break;

                        case AttackState.Returning:
                            lerpAttackCounter += Time.deltaTime / 2f;
                            transform.localPosition = Vector3.Lerp(new Vector3(currX, 0, 0), new Vector3(ogX, 0, 0), lerpAttackCounter);
                            if (lerpAttackCounter >= 1)
                            {
                                //reset
                                attackState = AttackState.Charging;
                                entityState = EntityState.Idle;
                                //start cooldown
                                attackCounter = entityStats.attackCooldown;
                            }
                            break;
                    }
                }
                break;
            case EntityState.Death:
                if (ogPosObj != null)
                {
                    Destroy(ogPosObj);
                    ogPosObj = null;
                }

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

    public override void HandlePassiveTrait()
    {
        passiveAbilityCounter += Time.deltaTime;

        //check if health below 30 % to activate ability
        if (!hasAbilityActivated && 0 < entityStats.health && entityStats.health <= entityStats.maxHealth * 0.5f)
        {
            //activate ability
            hasAbilityActivated = true;


            //increase defense
            activeDamageTakenMult = 0.5f;
        }

        //every 25 secs put 5 units to sleep for 5 secs
        if (passiveAbilityCounter > 25)
        {
            //reset
            passiveAbilityCounter = 0;

            DealStatusEffect(EntityStatusEffect.Sleep, 5);
        }
    }

    public override void HandleActiveTrait(float _scaleAngle)
    {
        //move slower up the scale, move faster when health is 25%
        activeMovementValue = (entityStats.health <= entityStats.maxHealth) ? 1.4f  : (_scaleAngle <= -2 ? 0.8f : 1.0f);

        //increase attack if tilted towards own home
        activeAttackMult = (_scaleAngle <= -2 ? 1.75f : 1f);

    }

    protected override void HandleAttackTrait()
    {
        DealStatusEffect(EntityStatusEffect.Stun, 100, true);
    }

    public void ElephantAttackEnd()
    {
        animator.SetBool("IsAttacking", false);
        //reset
        lerpAttackCounter = 0;
        attackState = AttackState.Returning;
        currX = transform.localPosition.x;
    }

    public override Vector3 GetPos()
    {
        if (entityState == EntityState.Attack)
            return ogPosObj.transform.position;
        return transform.position;
    }
}
