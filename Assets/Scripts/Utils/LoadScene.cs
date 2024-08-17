using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField]
    private string _sceneName;

    //Screen buffer
    [SerializeField]
    private Image _blackScreen;

    private bool _isLoading;

    private float _currTime;

    public void Load()
    {
        _isLoading = true;
        _currTime = 0;
        Invoke(nameof(LoadNewScene), 0.625f);
    }
    private void Start()
    {
        _isLoading = false;
        _blackScreen.color = new Color(0, 0, 0, 1);
        _currTime = 0;
    }
    private void Update()
    {
        if (_currTime <= 1.25)
        {
            if (_isLoading)
            {
                _blackScreen.color = Color.Lerp(new(0, 0, 0, 0),
                    new(0, 0, 0, 1), _currTime);
            }
            else
            {
                _blackScreen.color = Color.Lerp(new(0, 0, 0, 1),
                    new(0, 0, 0, 0), _currTime);
            }
            _currTime += Time.deltaTime * 2;
        }
    }
    private void LoadNewScene()
    {
        _blackScreen.color = new Color(0, 0, 0, 1);
        SceneManager.LoadSceneAsync(_sceneName);
    }
}
