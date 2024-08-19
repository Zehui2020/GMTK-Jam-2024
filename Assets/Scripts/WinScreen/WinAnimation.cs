using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinAnimation : MonoBehaviour
{
    [SerializeField]
    private LoadScene _loadScene;
    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetAxisRaw("Fire1") != 0)
        {
            _loadScene.Load();
        }
    }
}
