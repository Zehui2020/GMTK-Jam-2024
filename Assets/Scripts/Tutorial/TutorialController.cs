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

    [SerializeField]
    private List<GameObject> _troopButtonGO;

    [SerializeField]
    private Level _endlessLevelSwap;

    [SerializeField]
    private TMP_Text _controls;

    private int _currentTutorialNumber;
    private int _currentTutorialSet;
    private TutorialData _currentTutorialData;
    private float _currentTime;
    private bool _hasClicked;
    private int _currentWave;
    private int _mantisPlaced;
    private List<BaseEntity> _enemyEntities;
    public void CheckNewWave(int newWave)
    {
        _currentWave = newWave;
    }

    public void CheckTroopClcked(int troop)
    {
        switch (troop)
        {
            case 0:
                if (_currentTutorialSet == 1 && _troopSelectionController.GetSelectedEntity() != null)
                {
                    NextTutorial();
                }
                break;
        }
    }

    public void CheckTroopPlaced(Entity troop)
    {
        if (_currentTutorialSet == 3)
        {
            _mantisPlaced++;
            if (_mantisPlaced >= 2)
            {
                _troopsButtons[1].enabled = false;
                _troopButtonGO[1].gameObject.SetActive(false);
            }
        }
    }

    public void CheckEnemyPlaced(BaseEntity newEnemy)
    {
        if (_currentTutorialSet == 0)
        {
            NextTutorial();
        }
        _enemyEntities.Add(newEnemy);
    }
    private void Start()
    {
        _currentTutorialNumber = 0;
        _currentTutorialSet = -1;
        _currentTime = 0;
        _textboxContainer.gameObject.SetActive(false);
        _hasClicked = false;
        _mantisPlaced = 0;
        for (int buttonno = 0; buttonno < _troopsButtons.Count; buttonno++)
        {
            _troopsButtons[buttonno].enabled = false;
            _troopButtonGO[buttonno].SetActive(false);
        }
        _enemyEntities = new();
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
                if (_currentTime >= 1.5)
                {
                    NextTutorial();    
                }
                break;
            case 2:
                if (_currentWave == 1)
                {
                    _currentTime += Time.deltaTime;
                    if (_currentTime >= 1f)
                    {
                        NextTutorial();
                    }
                }
                else
                {
                    if (_enemyEntities.Count > 0 && _enemyEntities[0] == null)
                    {
                        _enemyEntities.Clear();
                        _levelController.Play();
                    }
                }
                break;
            case 3:
                if (_enemyEntities.Count > 1 && _enemyEntities[0] == null && _enemyEntities[1] == null)
                {
                    _enemyEntities.Clear();
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
                _troopButtonGO[0].gameObject.SetActive(true);
                break;
            case 2:
                _troopSelectionController.enabled = false;
                break;
            case 3:
                _levelController.Stop();
                _troopButtonGO[1].gameObject.SetActive(true);
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
                _troopButtonGO[0].gameObject.SetActive(false);
                break;
            case 3:
                _troopsButtons[1].enabled = true;
                break;
            case 4:
                for (int buttonno = 0; buttonno < 2; buttonno++)
                {
                    _troopsButtons[buttonno].enabled = true;
                    _troopButtonGO[buttonno].SetActive(true);
                }
                _levelController.SetLevel(_endlessLevelSwap);
                break;
        }
    }
}
