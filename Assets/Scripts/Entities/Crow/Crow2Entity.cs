using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crow2Entity : BaseEntity
{
    private bool hasAbilityActivated;

    public override void Init(Transform targetPoint)
    {
        base.Init(targetPoint);
        hasAbilityActivated = false;
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
}
