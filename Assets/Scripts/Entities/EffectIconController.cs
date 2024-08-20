using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectIconController : MonoBehaviour
{

    //effect gameobject icons
    //1. attack down
    //2. attack up
    //3. Defense Down
    //4. Defense Up
    //5. movement down
    //6. movement up
    //7. Weight Down
    //8. Weight up
    //9. fear
    //10. sleep
    //11. stun
    [SerializeField] private GameObject[] effectIcons;

    public void UpdateEffectIcons(List<bool> showIcons)
    {
        int x = 0;
        int y = 0;

        for (int i = 0; i < effectIcons.Length; i++)
        {
            if (showIcons[i])
            {
                //show icon
                effectIcons[i].SetActive(true);

                //placement
                effectIcons[i].transform.position = transform.position + new Vector3(x * 0.125f, -y * 0.125f, 0);
                x++;
                y += (x == 4 ? 1 : 0);
                x = x % 4;
            }
            else
            {
                //don't show icon
                effectIcons[i].SetActive(false);
            }
        }
    }
}
