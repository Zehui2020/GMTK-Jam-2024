using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class TutorialController : MonoBehaviour
{
    [SerializeField] 
    private List<TutorialData> _dataList;

    [SerializeField] 
    private RectTransform _textboxContainer;

    [SerializeField] 
    private TMP_Text _instructionsText;

    [SerializeField]
    private TroopSelectionController _troopSelectionController;

    [SerializeField]
    private LevelController _levelController;

    //Numbered from left to right, 0 - x
    [SerializeField]
    private List<Button> _troopsButtons;

    private int _currentTutorialNumber;
    private int _currentTutorialSet;
    private TutorialData _currentTutorialData;
    private float _currentTime;
    private bool _hasClicked;
    private int _currentWave;
    private int _mantisPlaced;
    public void CheckNewWave(int newWave)
    {
        _currentWave = newWave;
    }

    public void CheckTroopClcked(int troop)
    {
        switch (troop)
        {
            case 0:
                if (_currentTutorialSet == 1)
                {
                    NextTutorial();
                }
                break;
        }
    }

    public void CheckTroopPlaced(Entity troop)
    {
        if (troop._stats.entityName.Equals("AllyRangeEntity1"))
        {
            _mantisPlaced++;
            if (_mantisPlaced >= 2 && _currentTutorialSet == 3)
            {
                _troopsButtons[1].enabled = false;
            }
        }
    }
    private void Start()
    {
        _currentTutorialNumber = 0;
        _currentTutorialSet = -1;
        _currentTime = 0;
        _textboxContainer.gameObject.SetActive(false);
        _hasClicked = false;
        _mantisPlaced = 0;
        foreach(Button button in _troopsButtons)
        {
            button.enabled = false;
        }
    }
    private void Update()
    {
        if (!_textboxContainer.gameObject.activeSelf)
        {
            CheckTutorial();
        }
        else
        {
            if (Input.GetAxisRaw("Fire1") != 0)
            {
                if (!_hasClicked)
                {
                    _currentTutorialNumber++;
                    if (_currentTutorialNumber >=
                        _dataList[_currentTutorialSet].InstructionsText.Count)
                    {
                        _currentTutorialNumber = 0;
                        _textboxContainer.gameObject.SetActive(false);
                        Time.timeScale = 1;
                        TutorialExit();
                    }
                    else
                    {
                        ShowTutorial();
                    }
                    _hasClicked = true;
                }
            }
            else
            {
                _hasClicked = false;
            }
        }
    }
    private void NextTutorial()
    {
        _currentTime = 0;
        _currentTutorialSet++;
        if (_currentTutorialSet < _dataList.Count)
        {
            _currentTutorialData = _dataList[_currentTutorialSet];
            _textboxContainer.gameObject.SetActive(true);
            ShowTutorial();
            Time.timeScale = 0;
            TutorialEnter();
        }
    }
    private void ShowTutorial()
    {
        _textboxContainer.transform.localPosition = 
            _currentTutorialData.ContainerPosition[_currentTutorialNumber];
        _instructionsText.text =
            _currentTutorialData.InstructionsText[_currentTutorialNumber];
        LayoutRebuilder.ForceRebuildLayoutImmediate(_textboxContainer);
    }

    private void CheckTutorial()
    {
        switch (_currentTutorialSet) {
            case -1:
                _currentTime += Time.deltaTime;
                if (_currentTime >= 2)
                {
                    NextTutorial();
                    
                }
                break;
            case 0:
                _currentTime += Time.deltaTime;
                if (_currentTime >= 3)
                {
                    NextTutorial();
                }
                break;
            case 1:
                //TODO (Vincent): Check for enemy tent cat death
                _levelController.Play();
                break;
            case 2:
                if (_currentWave == 1)
                {
                    _currentTime += Time.deltaTime;
                    if (_currentTime >= 4f)
                    {
                        NextTutorial();
                    }
                }
                break;
            case 3:
                //TODO (Vincent): Check for both enemy mantis deaths
                if (_mantisPlaced == 2)
                {
                    NextTutorial();
                }
                break;
        }
    }
    private void TutorialEnter()
    {
        switch (_currentTutorialSet)
        {
            case 1:
                _levelController.Stop();
                break;
            case 2:
                _troopSelectionController.enabled = false;
                break;
            case 3:
                _levelController.Stop();
                _troopsButtons[1].enabled = true;
                _mantisPlaced = 0;
                break;
        }
    }
    private void TutorialExit()
    {
        switch (_currentTutorialSet)
        {
            case 1:
                _troopsButtons[0].enabled = true;
                break;
            case 2:
                _troopSelectionController.enabled = true;
                _troopsButtons[0].enabled = false;
                break;
            case 4:
                foreach(Button button in _troopsButtons)
                {
                    button.enabled = true;
                }
                _levelController.Play();
                break;
        }
    }
}
