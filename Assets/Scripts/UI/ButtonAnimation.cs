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

    private SpriteRenderer _spriteRenderer;
    private bool _isHover;
    private int _currSprite;

    public void OnPointerEnter(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = _spriteList[_defaultSpriteIndex];
        _isHover = false;
        _currSprite = -1;
    }

    private void Update()
    {
        if (_isHover)
        {

        }
    }
}
