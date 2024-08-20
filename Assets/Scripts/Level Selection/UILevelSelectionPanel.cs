using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(LoadScene))]
public class UILevelSelectedPanel : MonoBehaviour
{
    private LoadScene _loadScene;

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