using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsSegment : MonoBehaviour
{
    public bool IsShared;

    [SerializeField]
    private CreditsMove.SEGMENT_TYPE segmentType;

    public CreditsMove.SEGMENT_TYPE GetSegment()
    {
        return segmentType;
    }
}
