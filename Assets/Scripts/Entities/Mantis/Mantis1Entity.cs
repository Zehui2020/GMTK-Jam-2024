using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mantis1Entity : BaseEntity
{
    public override void HandlePassiveTrait()
    {

    }

    public override void HandleActiveTrait(float _scaleAngle)
    {
        //Movement based on scale's angle and direction character is moving in
        //Tilt towards ally base
        if (_scaleAngle >= 5)
        {
            activeMovementValue = isEnemy ? 1.2f : 0.8f;
        }
        //tilt towards enemy
        else if (_scaleAngle <= -5)
        {
            activeMovementValue = isEnemy ? 0.8f : 1.2f;
        }
        //neutral
        else
            activeMovementValue = 1;

        //increase attack damage if tilted to oppotsite base
        activeDamageTakenMult = isEnemy ?
            (_scaleAngle >= 4 ? 1.4f : 1.0f) : // check if tilted to ally side if is enemy
            (_scaleAngle <= -4 ? 1.4f : 1.0f); //check if tilted to enemy side if is ally
    }
}
