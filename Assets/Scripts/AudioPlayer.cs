using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Instance.PlayNormalBGM();
    }
}