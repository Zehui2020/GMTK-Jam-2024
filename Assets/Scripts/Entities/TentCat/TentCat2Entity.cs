using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentCat2Entity : BaseEntity
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
            activeMovementValue = isEnemy ? 1.6f : 0.5f;
        }
        //tilt towards enemy
        else if (_scaleAngle <= -5)
        {
            activeMovementValue = isEnemy ? 0.6f : 1.5f;
        }
        //neutral
        else
            activeMovementValue = 1;


        //If fear: activeMovementValue is negative
    }
}