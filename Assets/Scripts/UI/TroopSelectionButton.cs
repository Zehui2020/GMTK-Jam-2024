using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class TroopSelectionButton : MonoBehaviour
{
    [SerializeField] private TroopSelectionController _selectionController;

    [SerializeField] private Image _entityIcon;
    [SerializeField] private Slider _upgradeSlider;

    [SerializeField] private TextMeshProUGUI _entityCost;
    [SerializeField] private TextMeshProUGUI _entityLevel;

    [SerializeField] private float _upgradeHoldDelay;
    [SerializeField] private float _upgradeHoldDuration;

    [SerializeField] private List<Entity> _entityList = new();
    private int currentEntityLevel = 0;

    private Coroutine _checkUpgradeRoutine;
    private Coroutine _holdUpgradeRoutine;

    private void Start()
    {
        _entityIcon.sprite = _entityList[currentEntityLevel].selectionIcon;
        _entityCost.text = "" + _entityList[currentEntityLevel]._stats.cost;
        _entityLevel.text = "Lvl " + (currentEntityLevel + 1);
    }

    public void OnSelectTroop()
    {
        _selectionController.SelectTroop(_entityList[currentEntityLevel]);
    }

    public void UpgradeSelection()
    {
        currentEntityLevel++;
        if (currentEntityLevel >= _entityList.Count - 1)
            currentEntityLevel = _entityList.Count - 1;

        _entityIcon.sprite = _entityList[currentEntityLevel].selectionIcon;
        _entityCost.text = "" + _entityList[currentEntityLevel]._stats.cost;
        _entityLevel.text = "Lvl " + (currentEntityLevel + 1);
    }

    public void StartUpgradeProcess()
    {
        if (currentEntityLevel >= _entityList.Count - 1)
            return;

        _checkUpgradeRoutine = StartCoroutine(CheckUpgradeRoutine());
    }

    public void StopUpgradeProcess()
    {
        if (_checkUpgradeRoutine != null)
            StopCoroutine(_checkUpgradeRoutine);

        if (_holdUpgradeRoutine != null)
            StopCoroutine(_holdUpgradeRoutine);

        _upgradeSlider.value = 0;
    }

    private IEnumerator CheckUpgradeRoutine()
    {
        yield return new WaitForSeconds(_upgradeHoldDelay);

        _holdUpgradeRoutine = StartCoroutine(HoldUpgradeRoutine());
        _checkUpgradeRoutine = null;
    }

    private IEnumerator HoldUpgradeRoutine()
    {
        _upgradeSlider.value = 0;
        _upgradeSlider.maxValue = _upgradeHoldDuration;

        float timer = 0;
        while (timer < _upgradeHoldDuration)
        {
            timer += Time.deltaTime;
            _upgradeSlider.value = timer;
            yield return null;
        }

        _upgradeSlider.value = 0;
        UpgradeSelection();
        _holdUpgradeRoutine = null;
    }
}