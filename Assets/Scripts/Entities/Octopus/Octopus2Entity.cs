using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Octopus2Entity : BaseEntity
{
    private float passiveTriggerCounter;
    [SerializeField] private GameObject _clonePrefab;

    /*public override void Init(Transform targetPoint)
    {
        base.Init(targetPoint);
        passiveTriggerCounter = 0;
    }*/

    public override void HandlePassiveTrait()
    {

    }

    public override void HandleActiveTrait(float _scaleAngle)
    {
        //Movement based on scale's angle and direction character is moving in
        //Tilt towards ally base
        if (_scaleAngle >= 5)
        {
            activeMovementValue = isEnemy ? 1.4f : 0.5f;
        }
        //tilt towards enemy
        else if (_scaleAngle <= -5)
        {
            activeMovementValue = isEnemy ? 0.5f : 1.4f;
        }
        //neutral
        else
            activeMovementValue = 1;


        //Ability: spawn clone on the opposite side if scale lean on ally side for too long
        if (GetStatusEffect() != EntityStatusEffect.Sleep &&
            ((_scaleAngle >= 4 && !isEnemy) ||
            (_scaleAngle <= -4 && isEnemy)))
        {

            passiveTriggerCounter += Time.deltaTime;

            //if hit limit
            if (passiveTriggerCounter >= GetStats().passiveTraitTriggerDuration)
            {
                //activate trigger
                ApplyStatusEffect(EntityStatusEffect.Sleep, entityStats.passiveTraitDuration);

                //summon clone
                GameObject newEntity = Instantiate(_clonePrefab);
                //set position
                newEntity.transform.position = _targetPoint.position;
                //set Scale
                newEntity.transform.localScale = transform.localScale;
                //Init
                newEntity.GetComponent<BaseEntity>().Init(_targetPoint);
                //Add Weight
                newEntity.GetComponent<BaseEntity>().SetWeight(entityStats.weight);
                newEntity.GetComponent<OctopusCloneEntity>().counter = entityStats.passiveTraitDuration;
                //add to entitycontroller
                EntityController.Instance.AddEntity(newEntity, isEnemy);

                currWeight = 0;

                //Increase defense
                activeDamageTakenMult = 0.6f;
            }
        }
        else
        {
            //reset
            passiveTriggerCounter = 0;

        }

        if (GetStatusEffect() != EntityStatusEffect.Sleep)
        {
            //reset
            currWeight = entityStats.weight;
            activeDamageTakenMult = 1.0f;
        }
    }
}
