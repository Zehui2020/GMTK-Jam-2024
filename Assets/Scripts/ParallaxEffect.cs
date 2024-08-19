using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    private float length;
    private float startPos;
    [SerializeField] private GameObject cam;
    [SerializeField] private float parallaxEffect;
    [SerializeField] private bool followCamPosY;

    private void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void Update()
    {
        float temp = cam.transform.position.x * (1 - parallaxEffect);
        if (temp > startPos + length)
            startPos += length;
        else if (temp < startPos - length)
            startPos -= length;

        float dist = cam.transform.position.x * parallaxEffect;
        if (followCamPosY)
            transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);
        else
            transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);
    }
}