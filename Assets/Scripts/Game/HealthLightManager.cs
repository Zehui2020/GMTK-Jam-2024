using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthLightManager : MonoBehaviour
{
    [SerializeField] private List<HealthLight> _healthLights;
    [SerializeField] private BaseEntity _base;

    public void InitHealthLights()
    {
        int intervals = _base.GetStats().health / _healthLights.Count;
        int health = _base.GetStats().health - 1;

        for (int i = 0; i < _healthLights.Count; i++)
        {
            _healthLights[i].SetThreshold(health);
            _healthLights[i].OnBlowEvent += OnLightBlow;
            health -= intervals;
        }
    }

    private void Update()
    {
        foreach (HealthLight light in _healthLights)
        {
            light.UpdateLight(_base.GetStats().health);
        }
    }

    private void OnLightBlow(HealthLight healthLight)
    {
        int index = _healthLights.IndexOf(healthLight);
        if (index == -1 || index + 1 > _healthLights.Count - 1)
            return;

        _healthLights[index + 1].StartFlicker();
    }
}