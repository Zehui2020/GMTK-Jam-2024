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


}
