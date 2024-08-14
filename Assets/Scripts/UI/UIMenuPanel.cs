using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(LoadScene))]
public class UIMenuPanel : MonoBehaviour
{
    private LoadScene _loadScene;

    public void OnStartButtonPress()
    {
        // TODO - Assert
        if (_loadScene)
        {
            _loadScene.Load();
        }
    }

    public void OnQuitButtonPress()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void Awake()
    {
        _loadScene = GetComponent<LoadScene>();
    }
}