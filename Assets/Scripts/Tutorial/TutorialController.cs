using System.Collections.Generic;

using TMPro;
using UnityEngine;
public class TutorialController : MonoBehaviour
{
    [SerializeField] private List<TutorialData> _dataList;

    [SerializeField] private GameObject _textboxContainer;

    [SerializeField] private TMP_Text _instructionsText;

    private int _currentTutorialNumber;
    private int _currentTutorialSet;
    public void CheckNewWave(int newWave)
    {
        Debug.Log("wave " + newWave);
    }

    private void Start()
    {
        _currentTutorialNumber = 0;
        _currentTutorialSet = 0;
    }
    private void Update()
    {
        
    }

    private void ShowNextTutorial()
    {
        
    }
}
