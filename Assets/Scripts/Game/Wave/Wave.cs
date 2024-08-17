using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Wave", fileName = "New Wave")]
public class Wave : ScriptableObject 
{
    [field: Header("Wave Settings")]

    [field: SerializeField]
    public List<WaveNode> Nodes { get; private set; } 

    [field: SerializeField]
    public float LastNodeBuffer { get; private set; } = 0.0f;
}