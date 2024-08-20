using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crow2Entity : BaseEntity
{
    private bool hasAbilityActivated;
    private enum CrowAttackStates
    {
        Rise,
        Dive,
        Return
    }
    private CrowAttackStates attackState;

    //No Special Attributes
    Vector3 originalPos;
    Vector3 targetPos;
    Vector3 startPos;
    private float lerpCounter;

    public override void Init(Transform targetPoint)
    {
        base.Init(targetPoint);
        lerpCounter = 0;
        attackState = CrowAttackStates.Rise;
        hasAbilityActivated = false;
    }

    void SetVector3Pos(out Vector3 posToAlt, Vector3 _pos)
    {
        posToAlt = new Vector3(_pos.x, _pos.y, _pos.z);
    }

    //Rework Update for diving
    public override void HandleUpdate()
    {
        //DEBUGGING ONLY
        //Draw Attack Range
        //Vector3 dir = _targetPoint.position - transform.position;
        //dir.Normalize();
        //Debug.DrawRay(transform.position + dir * entityStats.minAttackRange, dir * entityStats.maxAttackRange, Color.green, 0.01f);

        HandleStatusEffect();
        HandlePassiveTrait();
        UpdateEffectIcon();

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
            SetVector3Pos(out originalPos, gameObject.transform.position);
            SetVector3Pos(out startPos, originalPos);
            SetVector3Pos(out targetPos, startPos + new Vector3(0, 2, 0));

            lerpCounter = 0;
            /*            attackCounter = entityStats.attackCooldown;
                        //activate animations
                        if (animator)
                            animator.SetBool("IsAttacking", true);*/

        }

        switch (entityState)
        {
            case EntityState.Walk:
                transform.position = Vector2.MoveTowards(transform.position, _targetPoint.position, entityStats.movementSpeed * 1 * Time.deltaTime);
                break;
            case EntityState.Idle:
                break;
            case EntityState.Attack:
                {
                    //attack states
                    switch (attackState)
                    {
                        case CrowAttackStates.Rise:
                            lerpCounter += Time.deltaTime / 3;
                            //move up
                            transform.position = Vector3.Lerp(startPos, targetPos, lerpCounter);
                            //reach end of lerp
                            if (lerpCounter >= 1)
                            {
                                animator.SetBool("IsWalking", false);
                                animator.SetBool("IsAttacking", true);
                            }
                            else
                            {
                                animator.SetBool("IsWalking", true);
                            }
                            break;

                        case CrowAttackStates.Dive:
                            lerpCounter += Time.deltaTime;
                            //dive
                            transform.position = Vector3.Lerp(startPos, targetPos, lerpCounter);
                            //reach end of lerp
                            if (lerpCounter >= 1)
                            {
                                animator.SetBool("IsAttacking", false);
                                //dive
                                lerpCounter = 0;
                                attackState = CrowAttackStates.Return;

                                //Set Positions
                                //new Start
                                SetVector3Pos(out startPos, transform.position);
                                //new target
                                targetPos = originalPos;

                                //Attack
                                Attack();
                            }

                            break;

                        case CrowAttackStates.Return:
                            lerpCounter += Time.deltaTime / 4;
                            animator.SetBool("IsWalking", true);
                            //dive
                            transform.position = Vector3.Lerp(startPos, targetPos, lerpCounter);
                            //reach end of lerp
                            if (lerpCounter >= 1)
                            {
                                //dive
                                lerpCounter = 0;
                                attackState = CrowAttackStates.Rise;

                                //reset
                                animator.SetBool("IsWalking", false);
                                entityState = EntityState.Idle;
                                //start attack cooldown
                                attackCounter = entityStats.attackCooldown;
                            }
                            break;
                    }
                }
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

    public void StartDive()
    {
        //dive
        lerpCounter = 0;
        attackState = CrowAttackStates.Dive;

        //Set Positions
        //new Start
        SetVector3Pos(out startPos, transform.position);
        //new target
        targetPos = originalPos + (_targetPoint.position - originalPos).normalized * entityStats.detectRange;
    }

    public override void HandlePassiveTrait()
    {
        //check if health below 30 % to activate ability
        if ( !hasAbilityActivated && 0 < entityStats.health && entityStats.health <= entityStats.maxHealth * 0.3f)
        {
            //activate ability
            hasAbilityActivated = true;

            //heal 50%
            entityStats.health += (int)(entityStats.maxHealth * 0.5f);

            //weight increase
            currWeight = entityStats.weight * 2;
        }
    }

    public override void HandleActiveTrait(float _scaleAngle)
    {
        //Movement based on scale's angle and direction character is moving in
        if (hasAbilityActivated)
        {
            //movement speed based on if moving up scale
            activeMovementValue = isEnemy ?
                (_scaleAngle <= -5 ? 0.4f : 1.0f) : // check if tilted to enemy side if is enemy
                (_scaleAngle >= 5 ? 0.4f : 1.0f); //check if tilted to ally side if is ally

            //increase attack damage if tilted to oppotsite base
            activeDamageTakenMult = isEnemy ?
                (_scaleAngle >= 5 ? 1.4f : 1.0f) : // check if tilted to ally side if is enemy
                (_scaleAngle <= -5 ? 1.4f : 1.0f); //check if tilted to enemy side if is ally
        }
        
    }

    protected override void HandleAttackTrait()
    {
        DealStatusEffect(EntityStatusEffect.Stun, 100);
    }

    public override Vector3 GetPos()
    {
        if (entityState == EntityState.Attack)
            return originalPos;
        return transform.position;
    }
}
