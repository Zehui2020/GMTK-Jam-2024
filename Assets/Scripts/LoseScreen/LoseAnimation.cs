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
    private GameObject _tumbleweed;

    [SerializeField]
    private LoadScene _loadScene;

    private float _currTime;
    private Vector3 _tumbleweedDir;
    private bool _isTumbleweedRolling;

    // Start is called before the first frame update
    private void Start()
    {
        _currTime = 0;
        _tumbleweedDir = new Vector3(3, 0, 0);
        _isTumbleweedRolling = false;
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
        else if (_currTime >= 3)
        {
            if (!_isTumbleweedRolling)
            {
                _isTumbleweedRolling = true;
                if (Random.Range(0, 2) == 0) {
                    _tumbleweed.transform.localPosition = new Vector3(
                    -10.25f,
                    _tumbleweed.transform.localPosition.y,
                    _tumbleweed.transform.localPosition.z);
                    _tumbleweedDir = new(Random.Range(1, 3), 0, 0);
                }
                else
                {
                    _tumbleweed.transform.localPosition = new Vector3(
                    10.25f,
                    _tumbleweed.transform.localPosition.y,
                    _tumbleweed.transform.localPosition.z);
                    _tumbleweedDir = new(Random.Range(-1, -3), 0, 0);
                }
                _isTumbleweedRolling = true;
            }
            else
            {
                _tumbleweed.transform.localPosition += _tumbleweedDir * Time.deltaTime;
                if (_tumbleweed.transform.localPosition.x > 14f || _tumbleweed.transform.localPosition.x < -14f)
                {
                    _isTumbleweedRolling = false;
                }
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
