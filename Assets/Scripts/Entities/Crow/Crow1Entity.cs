using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BaseEntity;

public class Crow1Entity : BaseEntity
{
    [SerializeField] private GameObject _posPrefab;
    private GameObject ogPosObj, targetPosObj;

    private enum CrowAttackStates
    {
        Rise,
        Dive,
        Return
    }
    private CrowAttackStates attackState;

    //No Special Attributes
    //Vector3 originalPos;
    //Vector3 targetPos;
    Vector3 startPos;
    private float lerpCounter;

    public override void Init(Transform targetPoint)
    {
        base.Init(targetPoint);
        lerpCounter = 0;
        attackState = CrowAttackStates.Rise;

        ogPosObj = Instantiate(_posPrefab);
        ogPosObj.transform.parent = transform.parent;
        ogPosObj.GetComponent<PositionEntity>().creater = gameObject;
        targetPosObj = Instantiate(_posPrefab);
        targetPosObj.transform.parent = transform.parent;
        targetPosObj.GetComponent<PositionEntity>().creater = gameObject;
    }

    Vector3 SetVector3Pos(Vector3 _pos)
    {
        return new Vector3(_pos.x, _pos.y, _pos.z);
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
            ogPosObj.transform.position = SetVector3Pos(gameObject.transform.position);
            startPos = SetVector3Pos(gameObject.transform.position);
            targetPosObj.transform.position = SetVector3Pos(startPos + new Vector3(0, 2, 0));
            
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
                            transform.position = Vector3.Lerp(startPos, targetPosObj.transform.position, lerpCounter);
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
                            transform.position = Vector3.Lerp(startPos, targetPosObj.transform.position, lerpCounter);
                            //reach end of lerp
                            if (lerpCounter >= 1)
                            {
                                animator.SetBool("IsAttacking", false);
                                //dive
                                lerpCounter = 0;
                                attackState = CrowAttackStates.Return;

                                //Set Positions
                                //new Start
                                startPos = SetVector3Pos(transform.position);

                                //Attack
                                Attack();
                            }

                            break;

                        case CrowAttackStates.Return:
                            lerpCounter += Time.deltaTime / 4;
                            animator.SetBool("IsWalking", true);
                            //dive
                            transform.position = Vector3.Lerp(startPos, ogPosObj.transform.position, lerpCounter);
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
        startPos = SetVector3Pos(transform.position);
        //new target
        targetPosObj.transform.position = ogPosObj.transform.position + (_targetPoint.position - ogPosObj.transform.position).normalized * entityStats.detectRange;
    }

    public override Vector3 GetPos()
    {
        if (entityState == EntityState.Attack)
            return ogPosObj.transform.position;
        return transform.position;
    }
}
