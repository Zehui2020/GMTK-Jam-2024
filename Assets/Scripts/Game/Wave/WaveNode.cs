using System;

using UnityEngine;

[Serializable]
public struct WaveNode
{
    [field: Header("WaveNode Dependencies")]

    [field: SerializeField]
    public GameObject EntityPrefab { get; private set; }

    [field: Header("WaveNode Settings")]

    [field: SerializeField]
    public float SpawnAfterStartSeconds { get; private set; }

    [field: SerializeField]
    public float PositionSpawn { get; private set; }

    [field: Header("WaveNode Sequence Settings")]

    [field: SerializeField]
    [field: Range(1, 200)]
    public int SequenceAmount { get; private set; }

    [field: SerializeField]
    [field: Range(0.1f, 100.0f)]
    public float SequenceTimeGap { get; private set; }
}