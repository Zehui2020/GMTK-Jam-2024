using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mantis3Entity : BaseEntity
{
    private float attackAbilityTraitCounter = 0;
    [SerializeField] private GameObject _projectileEffectPrefab;

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

        //increase attack damage if tilted to oppotsite base
        activeDamageTakenMult = isEnemy ?
            (_scaleAngle >= 4 ? 1.4f : 1.0f) : // check if tilted to ally side if is enemy
            (_scaleAngle <= -4 ? 1.4f : 1.0f); //check if tilted to enemy side if is ally
    }

    protected override void HandleAttackTrait()
    {
        if (attackAbilityTraitCounter <= 0)
        {
            //Apply fear
            attackAbilityTraitCounter = entityStats.attackTraitCooldown;//15 sec
            DealStatusEffect(EntityStatusEffect.Fear, 3);
        }

        //projectile
        GameObject newObj = Instantiate(_projectileEffectPrefab);
        newObj.transform.rotation = transform.rotation;
        newObj.GetComponent<AnimProjectileController>().Init(transform.position + (_targetPoint.position - transform.position).normalized * 0.5f, transform.position + (_targetPoint.position - transform.position).normalized * entityStats.detectRange);
    }
}
