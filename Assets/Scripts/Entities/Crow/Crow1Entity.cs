using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BaseEntity;

public class Crow1Entity : BaseEntity
{
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

    public override Vector3 GetPos()
    {
        if (entityState == EntityState.Attack)
            return originalPos;
        return transform.position;
    }
}
