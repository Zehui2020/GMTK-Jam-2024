using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctopusCloneEntity : BaseEntity
{
    public float counter;
    /*public override void Init(Transform _targetPoint)
    {
        base.Init(_targetPoint);
        //not targetable
        isTargetable = false;
        counter = 0;
        ApplyStatusEffect(EntityStatusEffect.Sleep);
    }*/


    public override void HandleUpdate()
    {
        switch (entityState)
        {
            case EntityState.Death:
                if (!animator)
                    isDead = true;
                break;
            default:
                if (counter > 0)
                {
                    counter -= Time.deltaTime;
                    if (counter <= 0)
                    {
                        entityState = EntityState.Death;
                        if (animator)
                            animator.SetBool("isDead", true);
                    }
                }
                
                break;
        }
    }
}