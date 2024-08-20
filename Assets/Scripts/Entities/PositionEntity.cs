using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionEntity : MonoBehaviour
{

    public GameObject creater;

    // Update is called once per frame
    void Update()
    {
        if (creater == null)
            Destroy(gameObject);
    }
}
