using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimProjectileController : MonoBehaviour
{
    public void DeleteOnAnimEnd()
    {
        Destroy(gameObject);
    }
}
