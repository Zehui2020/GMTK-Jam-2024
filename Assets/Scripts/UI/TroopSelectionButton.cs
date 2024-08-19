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

    [SerializeField] private Color _availableColor;
    [SerializeField] private Color _unavailableColor;

    [SerializeField] private TextMeshProUGUI _entityCost;
    [SerializeField] private TextMeshProUGUI _entityLevel;
    [SerializeField] private RectTransform _tooltipPosition;
    [SerializeField] private Animator _animator;

    [SerializeField] private float _upgradeHoldDelay;
    [SerializeField] private float _upgradeHoldDuration;

    [SerializeField] private List<Entity> _entityList = new();
    private int currentEntityLevel = 0;

    private Coroutine _checkUpgradeRoutine;
    private Coroutine _holdUpgradeRoutine;
    private Coroutine _hoverRoutine;

    private bool canSelect = false;
    private bool canUpgrade = false;

    private void Start()
    {
        _entityIcon.sprite = _entityList[currentEntityLevel].selectionIcon;
        _entityCost.text = "" + _entityList[currentEntityLevel]._stats.cost;
        _entityLevel.text = "Lvl " + (currentEntityLevel + 1);
    }

    public void OnSelectTroop()
    {
        if (!canSelect)
        {
            return;
        }

        _selectionController.SelectTroop(_entityList[currentEntityLevel]);
    }

    public void UpgradeSelection()
    {
        MoneyController.Instance.SpendMoney(_entityList[currentEntityLevel]._stats.upgradeCost);

        currentEntityLevel++;
        if (currentEntityLevel >= _entityList.Count - 1)
            currentEntityLevel = _entityList.Count - 1;

        _entityIcon.sprite = _entityList[currentEntityLevel].selectionIcon;
        _entityCost.text = "" + _entityList[currentEntityLevel]._stats.cost;
        _entityLevel.text = "Lvl " + (currentEntityLevel + 1);

        if (currentEntityLevel < _entityList.Count - 1)
            TooltipManager.Instance.SetupToolip(_entityList[currentEntityLevel], _entityList[currentEntityLevel + 1], _tooltipPosition);
        else
            TooltipManager.Instance.SetupToolip(_entityList[currentEntityLevel], null, _tooltipPosition);
    }

    public void StartUpgradeProcess()
    {
        if (currentEntityLevel >= _entityList.Count - 1 || !canUpgrade)
            return;

        _checkUpgradeRoutine = StartCoroutine(CheckUpgradeRoutine());
    }

    public void StopUpgradeProcess()
    {
        if (_checkUpgradeRoutine != null)
            StopCoroutine(_checkUpgradeRoutine);

        if (_holdUpgradeRoutine != null)
        {
            StopCoroutine(_holdUpgradeRoutine);
        }

        _animator.SetBool("upgrading", false);
        TooltipManager.Instance.SetUpgradeAnimation(false);

        _upgradeSlider.value = 0;
    }

    private IEnumerator CheckUpgradeRoutine()
    {
        yield return new WaitForSeconds(_upgradeHoldDelay);

        if (_hoverRoutine != null)
            StopCoroutine(_hoverRoutine);
        _hoverRoutine = null;

        if (currentEntityLevel < _entityList.Count - 1)
            TooltipManager.Instance.ShowTooltip(_entityList[currentEntityLevel], _entityList[currentEntityLevel + 1], _tooltipPosition);
        else
            TooltipManager.Instance.ShowTooltip(_entityList[currentEntityLevel], null, _tooltipPosition);

        _holdUpgradeRoutine = StartCoroutine(HoldUpgradeRoutine());
        _checkUpgradeRoutine = null;
    }

    private IEnumerator HoldUpgradeRoutine()
    {
        _upgradeSlider.value = 0;
        _upgradeSlider.maxValue = _upgradeHoldDuration;
        _animator.SetBool("upgrading", true);
        TooltipManager.Instance.SetUpgradeAnimation(true);

        float timer = 0;
        while (timer < _upgradeHoldDuration)
        {
            timer += Time.deltaTime;
            _upgradeSlider.value = timer;
            yield return null;
        }

        _animator.SetBool("upgrading", false);
        TooltipManager.Instance.FinishUpgrading();
        TooltipManager.Instance.SetUpgradeAnimation(false);
        _upgradeSlider.value = 0;

        _holdUpgradeRoutine = null;
        UpgradeSelection();
    }

    public void StartHoverRoutine()
    {
        _hoverRoutine = StartCoroutine(HoverRoutine());
    }

    public void StopHoverRoutine()
    {
        if (_hoverRoutine != null)
            StopCoroutine(_hoverRoutine);

        TooltipManager.Instance.HideTooltip();
        _hoverRoutine = null;
    }

    private IEnumerator HoverRoutine()
    {
        yield return new WaitForSeconds(1f);

        if (currentEntityLevel < _entityList.Count - 1)
            TooltipManager.Instance.ShowTooltip(_entityList[currentEntityLevel], _entityList[currentEntityLevel + 1], _tooltipPosition);
        else
            TooltipManager.Instance.ShowTooltip(_entityList[currentEntityLevel], null, _tooltipPosition);

        _hoverRoutine = null;
    }

    private void Update()
    {
        if (MoneyController.Instance.money < _entityList[currentEntityLevel]._stats.cost)
        {
            canSelect = false;
            _entityIcon.color = _unavailableColor;
        }
        else
        {
            canSelect = true;
            _entityIcon.color = _availableColor;
        }

        if (MoneyController.Instance.money < _entityList[currentEntityLevel]._stats.upgradeCost)
        {
            canUpgrade = false;
        }
        else
        {
            canUpgrade = true;
        }
    }
}