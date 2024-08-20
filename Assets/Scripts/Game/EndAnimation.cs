using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class EndAnimation : MonoBehaviour
{
    [SerializeField]
    private Image _backgroundDim;

    [SerializeField]
    private GameObject _winBanner;

    [SerializeField]
    private GameObject _loseBanner;

    [SerializeField]
    private LevelController _levelController;

    [SerializeField]
    private TMP_Text _specialText;

    [SerializeField]
    private GameObject _clickAnywhereToContinue;

    [SerializeField]
    private List<GameObject> _winEffects;
    
    [SerializeField]
    private LoadScene _loadScene;

    private float _currTime;
    private bool _isFinished;
    private bool _result;
    // Start is called before the first frame update
    void Start()
    {
        _currTime = 0;
        _backgroundDim.color = new(0, 0, 0, 0);
        _specialText.text = "";
        foreach(GameObject go in _winEffects)
        {
            go.SetActive(false);
            
        }
        _clickAnywhereToContinue.SetActive(false);
        _winBanner.SetActive(false);
        _loseBanner.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_isFinished)
        {
            _currTime += Time.deltaTime;
            if (_currTime <= 0.5f)
            {
                _backgroundDim.color = Color.Lerp(new(0, 0, 0, 0), new(0, 0, 0, 0.75f), _currTime * 2);
            }
            else if (_currTime <= 1.5f)
            {
                if (_result)
                {
                    _winBanner.SetActive(true);
                    _loseBanner.SetActive(false);
                }
                else
                {
                    _loseBanner.SetActive(true);
                    _winBanner.SetActive(false);
                }
            }
            else if (_currTime <= 2f)
            {
                if (_result)
                {
                    switch (_levelController.GetLevel().levelID)
                    {
                        case 0:
                            _specialText.text = "You unlocked <color=#00FFFF>Tent Cat</color> & <color=#00FFFF>Mantis</color>!<br>You may now use it in further stages!";
                            break;
                        case 1:
                            _specialText.text = "You unlocked <color=#00FFFF>Octopus</color>!<br>You may now use it in further stages!";
                            break;
                        case 2:
                            _specialText.text = "You unlocked <color=#00FFFF>Crow</color>!<br>You may now use it in further stages!";
                            break;
                        default:
                            _specialText.text = "";
                            break;
                    }
                }
            }
            else if (_currTime <= 2.5f)
            {
                _clickAnywhereToContinue.SetActive(true);
                if (_result)
                {
                    if (!_winEffects[0].activeSelf)
                    {
                        foreach (GameObject go in _winEffects)
                        {
                            go.SetActive(true);
                        }
                    }
                }
            }
            else
            {
                if (Input.GetAxis("Fire1") != 0)
                {
                    _loadScene.Load();
                }
            }
        }
    }

    public void EndGame(bool isWin)
    {
        _isFinished = true;
        _result = isWin;
    }
}
