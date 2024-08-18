using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
public class CreditsMove : MonoBehaviour,IDragHandler
{
    public enum SEGMENT_TYPE
    {
        TITLE,
        CATEGORY,
        ROLE,
        NAME
    }

    [SerializeField]
    private List<CreditsSegment> _textList;

    //top padding ased on segment type
    [SerializeField]
    private List<int> _textSizeY;

    [SerializeField]
    private GameObject _textContainer;

    [SerializeField]
    private float _sensitivity;

    [SerializeField]
    private int _textBlockBuffer;

    private Vector3 _direction;
    private int _totalTextSize;

    public void OnDrag(PointerEventData eventData)
    {
        _textContainer.transform.localPosition += new Vector3(0,
            eventData.delta.y * _sensitivity,0);
    }

    private void Start()
    {
        _direction = new(0, 0, 0);
        int _currentShared = 0;
        int _currentSharedSizeY = 0;
        foreach(CreditsSegment segment in _textList)
        {
            if (!segment.IsShared)
            {
                _totalTextSize += _textSizeY[(int)segment.GetSegment()];
                segment.transform.localPosition = new Vector3(0, -_totalTextSize, 0);
            }
            else
            {
                _currentShared++;
                _currentSharedSizeY += _textSizeY[(int)segment.GetSegment()];
                segment.transform.localPosition = new Vector3(
                    _currentShared < 3 ? -150 : 150, 
                    -_totalTextSize - _currentSharedSizeY, 0);
                switch (_currentShared)
                {
                    case 2:
                        _currentSharedSizeY = 0;
                        break;
                    case 4:
                        _currentShared = 0;
                        _totalTextSize += _currentSharedSizeY;
                        _currentSharedSizeY = 0;
                        
                        break;
                }
            }
        }
        _totalTextSize += _textBlockBuffer;
    }

    private void Update()
    {
        _textContainer.transform.localPosition += _direction * Time.deltaTime;
        //text block acceleration
        if (_direction.y < 25)
        {
            _direction += new Vector3(0, 10 * Time.deltaTime, 0);
            if (_direction.y >= 25)
            {
                _direction = new Vector3(0, 25, 0);
            }
        }
        CheckTexts();
    }
    private void CheckTexts()
    {
        for (int _textNum = 0; _textNum < _textList.Count; _textNum++)
        {
            Vector3 textLocalPos = _textList[_textNum].transform.localPosition;
            //check need wrap from top to bottom
            if (textLocalPos.y + _textContainer.transform.localPosition.y
                > _totalTextSize * 0.5f)
            {
                _textList[_textNum].transform.localPosition = textLocalPos
                    + new Vector3(0, -_totalTextSize, 0);
            }
            //check if need wrap from bottom to top
            else if (textLocalPos.y + _textContainer.transform.localPosition.y
                < -_totalTextSize * 0.5f)
            {
                _textList[_textNum].transform.localPosition = textLocalPos
                    + new Vector3(0, _totalTextSize, 0);
            }
        }
    }
}
