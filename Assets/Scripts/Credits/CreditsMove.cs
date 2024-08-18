using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
public class CreditsMove : MonoBehaviour,IDragHandler
{
    [SerializeField]
    private List<CreditsSegment> textList;

    [SerializeField]
    private GameObject textContainer;
    public void OnDrag(PointerEventData eventData)
    {
        
    }

    private void Start()
    {
        
    }
    private void Update()
    {
        
    }
}
