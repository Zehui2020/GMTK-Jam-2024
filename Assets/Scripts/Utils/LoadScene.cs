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

    private void Start()
    {
        _isLoading = false;
        _blackScreen.color = new Color(0, 0, 0, 1);
    }
    private void Update()
    {
        if (_isLoading)
        {
            _blackScreen.color = Color.Lerp(_blackScreen.color,
                new(0, 0, 0, 1), Time.deltaTime * 7.5f);
        }
        else
        {
            _blackScreen.color = Color.Lerp(_blackScreen.color,
                            new(0, 0, 0, 0), Time.deltaTime);
        }
    }
    public void Load()
    {
        _isLoading = true;
        Invoke(nameof(LoadNewScene), 1.25f);
    }

    private void LoadNewScene()
    {
        SceneManager.LoadSceneAsync(_sceneName);
    }
}
