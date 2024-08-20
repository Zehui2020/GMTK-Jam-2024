using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(LoadScene))]
public class UILevelSelectedPanel : MonoBehaviour
{
    private LoadScene _loadScene;

    [SerializeField]
    private List<Image> _tentImages;

    [SerializeField]
    private List<GameObject> _tentLockGO;

    [SerializeField]
    private List<Button> _tentButtons;

    [SerializeField]
    private PlayerDataSO _playerDataSO;

    private void OnEnable()
    {
        for (int tentno = 0; tentno < _tentImages.Count; tentno++)
        {
            if (_playerDataSO.MaxUnlockedLevel >= tentno)
            {
                _tentImages[tentno].color = new(1, 1, 1,1);
                _tentLockGO[tentno].SetActive(false);
                _tentButtons[tentno].interactable = true;
            }
            else
            {
                _tentImages[tentno].color = new(0.5f, 0.5f, 0.5f,1);
                _tentLockGO[tentno].SetActive(true);
                _tentButtons[tentno].interactable = false;
            }
        }
    }

    public void OnBackButtonPress()
    {
        // TODO (Chris):  Assert
        if (_loadScene)
        {
            _loadScene.Load();
        }
    }

    private void Awake()
    {
        _loadScene = GetComponent<LoadScene>();
    }
}