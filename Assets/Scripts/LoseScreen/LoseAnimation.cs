using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Rendering.Universal;
public class LoseAnimation : MonoBehaviour
{
    [SerializeField]
    private List<Animator> _m_AnimatorList;

    [SerializeField]
    private Light2D _light;

    [SerializeField]
    private LoadScene _loadScene;

    private float _currTime;


    // Start is called before the first frame update
    private void Start()
    {
        _currTime = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        _currTime += Time.deltaTime;
        if (_currTime <= 1.5f)
        {
            _light.intensity = 3;
        }
        else if (_currTime <= 1.525f)
        {
            _light.intensity = 0;
        }
        else if (_currTime <= 1.550f)
        {
            _light.intensity = 3;
        }
        else if (_currTime <= 1.575)
        {
            _light.intensity = 0;
        }
        else if (_currTime <= 2.0)
        {
            _light.intensity = 3;
        }
        else if (_currTime <= 2.1)
        {
            _light.intensity = 0;
            foreach (Animator anim in _m_AnimatorList)
            {
                anim.Play("Death");
                
            }
        }
        for (int animno = 0; animno < _m_AnimatorList.Count; animno++)
        {
            BaseEntity en = _m_AnimatorList[animno].GetComponent<BaseEntity>();
            if (en && en.isDead)
            {
                _m_AnimatorList.Remove(_m_AnimatorList[animno]);
                Destroy(en.gameObject);
                animno--;
            }
        }
        if (Input.GetAxisRaw("Fire1") != 0)
        {
            _loadScene.Load();
        }
    }
}
