using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _stats;
    [SerializeField] private TextMeshProUGUI _abilities;

    public void SetTooltip(string title, string stats, string abilities)
    {
        _title.text = title;
        _stats.text = stats;
        _abilities.text = abilities;
    }
}