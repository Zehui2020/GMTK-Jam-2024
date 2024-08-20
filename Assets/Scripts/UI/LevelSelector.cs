using UnityEngine;

using static DebugUtility; 

// To be added on as another component on a UI object
public class LevelSelector : MonoBehaviour
{
    [Header("LevelSelector Dependencies")]

    [SerializeField]
    private LevelSelectionSharedData _sharedData;

    [SerializeField]
    private Level _levelToSelect;

    private Animator _animator;

    public void Select() 
    {
        AssertNotNull(_levelToSelect, "No level to select");
        _sharedData.Level = _levelToSelect;
        _animator.SetTrigger("select");
        AudioManager.Instance.PlayOneShot("ButtonClick");
    }

    public void OnHover()
    {
        _animator.SetBool("hovering", true);
        AudioManager.Instance.PlayOneShot("ButtonHover");
    }

    public void OnHoverExit()
    {
        _animator.SetBool("hovering", false);
    }

    private void Awake()
    {
        if (!_levelToSelect)
        {
            Debug.LogWarning(
                "There is no level inside the `levelToSelect` field");
        }
        _sharedData.Level = null; 
        _animator = GetComponent<Animator>();
    }
}