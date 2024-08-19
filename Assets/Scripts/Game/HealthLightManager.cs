using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthLightManager : MonoBehaviour
{
    [SerializeField] private List<HealthLight> _healthLights;
    [SerializeField] private BaseEntity _base;

    private void Update()
    {
        foreach (HealthLight light in _healthLights)
        {
            light.UpdateLight(_base.GetStats().health);
        }
    }
}