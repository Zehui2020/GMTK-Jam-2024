using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForceLayoutRebuild : MonoBehaviour
{
    [SerializeField] private RectTransform targetRectTransform;

    private void Start()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(targetRectTransform);
    }

    private void OnEnable()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(targetRectTransform);
    }
}