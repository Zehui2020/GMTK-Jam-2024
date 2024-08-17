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
}