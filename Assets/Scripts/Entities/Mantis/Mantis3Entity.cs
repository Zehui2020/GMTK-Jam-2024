using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mantis3Entity : BaseEntity
{
    private float attackAbilityTraitCounter = 0;

    public override void HandlePassiveTrait()
    {
        if (attackAbilityTraitCounter > 0)
        {
            attackAbilityTraitCounter -= Time.deltaTime;
        }
    }

    public override void HandleActiveTrait(float _scaleAngle)
    {
        //Movement based on scale's angle and direction character is moving in
        //Tilt towards ally base
        if (_scaleAngle >= 5)
        {
            activeMovementValue = isEnemy ? 1.5f : 1f;
        }
        //tilt towards enemy
        else if (_scaleAngle <= -5)
        {
            activeMovementValue = isEnemy ? 1f : 1.5f;
        }
        //neutral
        else
            activeMovementValue = 1;

        //If fear: activeMovementValue is negative
    }

    protected override void HandleAttackTrait()
    {
        if (attackAbilityTraitCounter <= 0)
        {
            //Apply fear
            attackAbilityTraitCounter = entityStats.attackTraitCooldown;//15 sec
            DealStatusEffect(EntityStatusEffect.Fear, 3);
        }
    }
}
