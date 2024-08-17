using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public enum FollowType
    {
        Position,
        Rotation
    }
    [SerializeField] private FollowType followType;
    [SerializeField] private Transform _target;

    private void Update()
    {
        switch (followType)
        {
            case FollowType.Position:
                transform.position = _target.position;
                break;
            case FollowType.Rotation:
                transform.rotation = _target.rotation;
                break;
        }
    }
}
