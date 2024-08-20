using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonAnimation : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private List<Sprite> _spriteList = new List<Sprite>();

    [SerializeField]
    private int _defaultSpriteIndex;

    [SerializeField]
    private float _spriteInterval;

    [SerializeField]
    private GameObject _hoverGO;

    [SerializeField]
    private float _hoverSpeed;

    private SpriteRenderer _spriteRenderer;
    private bool _isHover;
    private int _currSprite;
    private float _currTime;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isHover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isHover = false;
    }

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = _spriteList[_defaultSpriteIndex];
        _isHover = false;
        _currSprite = _defaultSpriteIndex;
        _currTime = _spriteInterval;
    }

    private void Update()
    {
        if (_currTime <= 0)
        {
            _currSprite++;
            if (_currSprite >= _spriteList.Count)
            {
                _currSprite = 0;
            }
            _spriteRenderer.sprite = _spriteList[_currSprite];
            _currTime = _spriteInterval;
        }
        else
        {
            _currTime -= Time.deltaTime;
        }

        if (_isHover)
        {
            _hoverGO.transform.localScale = Vector3.Lerp(_hoverGO.transform.localScale, new(-1.7f, 0.55f, 1), Time.deltaTime * _hoverSpeed);
        }
        else
        {
            _hoverGO.transform.localScale = Vector3.Lerp(_hoverGO.transform.localScale, new(0f, 0.55f, 1), Time.deltaTime * _hoverSpeed);
        }
    }
}
