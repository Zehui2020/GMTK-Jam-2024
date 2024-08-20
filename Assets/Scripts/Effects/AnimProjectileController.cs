using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimProjectileController : MonoBehaviour
{
    private float lerpCounter;
    private float animationClipLength;
    private Vector3 startPos, endPos;

    public void Init(Vector3 _startPosition, Vector3 _endPosition)
    {
        lerpCounter = 0;
        animationClipLength = GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length;
        startPos = _startPosition;
        endPos = _endPosition;
    }

    private void Update()
    {
        lerpCounter += Time.deltaTime * animationClipLength * 2;
        
        transform.position = Vector3.Lerp(startPos, endPos, lerpCounter);
    }

    public void DeleteOnAnimEnd()
    {
        Destroy(gameObject);
    }
}
