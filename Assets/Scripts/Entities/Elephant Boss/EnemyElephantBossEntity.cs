using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyElephantBossEntity : BaseEntity
{
    private bool hasAbilityActivated;
    private float passiveAbilityCounter;

    public override void Init(Transform targetPoint)
    {
        base.Init(targetPoint);
        hasAbilityActivated = false;

        passiveAbilityCounter = 0;
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
}
