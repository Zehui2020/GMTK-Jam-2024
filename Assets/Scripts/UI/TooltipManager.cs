using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance;

    [SerializeField] private Tooltip _detailTooltip;
    [SerializeField] private Tooltip _upgradeTooltip;
    [SerializeField] private GameObject _arrow;
    [SerializeField] private RectTransform _rectTransform;

    [SerializeField] private Animator _animator;

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    public void ShowTooltip(Entity currentEntity, Entity upgradedEntity, RectTransform tooltipPosition)
    {
        gameObject.SetActive(true);
        SetupToolip(currentEntity, upgradedEntity, tooltipPosition);
    }

    public void SetupToolip(Entity currentEntity, Entity upgradedEntity, RectTransform tooltipPosition)
    {
        _detailTooltip.SetTooltip(currentEntity.entityName, SetupStatString(currentEntity._stats, null), currentEntity.abilities);

        if (upgradedEntity == null)
        {
            _upgradeTooltip.gameObject.SetActive(false);
            _arrow.gameObject.SetActive(false);
            _detailTooltip.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        }
        else
        {
            _upgradeTooltip.gameObject.SetActive(true);
            _arrow.gameObject.SetActive(true);
            _detailTooltip.GetComponent<RectTransform>().anchoredPosition = new Vector3(-260, 0, 0);

            _upgradeTooltip.SetTooltip(upgradedEntity.entityName, SetupStatString(currentEntity._stats, upgradedEntity._stats), upgradedEntity.abilities);
        }

        _rectTransform.SetParent(tooltipPosition);
        _rectTransform.anchoredPosition = Vector3.zero;
    }

    public void HideTooltip()
    {
        _animator.SetTrigger("hide");
    }

    public void SetInactive()
    {
        gameObject.SetActive(false);
    }

    private string SetupStatString(EntityStatsScriptableObject currentStats, EntityStatsScriptableObject upgradedStats)
    {
        string finalString = string.Empty;

        if (upgradedStats == null)
        {
            finalString += "Health: " + currentStats.health + "\n";
            finalString += "Damage: " + currentStats.attackDamage + "\n";
            finalString += "Weight: " + currentStats.weight + "\n";
            finalString += "Speed: " + currentStats.movementSpeed + "\n";
            finalString += "Range: " + currentStats.detectRange + "\n";
        }
        else
        {
            // Health
            if (upgradedStats.health < currentStats.health)
                finalString += "Health: <color=red>" + upgradedStats.health + "</color>\n";
            else if (upgradedStats.health == currentStats.health)
                finalString += "Health: " + upgradedStats.health + "\n";
            else
                finalString += "Health: <color=green>" + upgradedStats.health + "</color>\n";

            // Damage
            if (upgradedStats.attackDamage < currentStats.attackDamage)
                finalString += "Damage: <color=red>" + upgradedStats.attackDamage + "</color>\n";
            else if (upgradedStats.attackDamage == currentStats.attackDamage)
                finalString += "Damage: " + upgradedStats.attackDamage + "\n";
            else
                finalString += "Damage: <color=green>" + upgradedStats.attackDamage + "</color>\n";

            // Weight
            finalString += "Weight: " + upgradedStats.weight + "\n";

            // Speed
            if (upgradedStats.movementSpeed < currentStats.movementSpeed)
                finalString += "Speed: <color=red>" + upgradedStats.movementSpeed + "</color>\n";
            else if (upgradedStats.movementSpeed == currentStats.movementSpeed)
                finalString += "Speed: " + upgradedStats.movementSpeed + "\n";
            else
                finalString += "Speed: <color=green>" + upgradedStats.movementSpeed + "</color>\n";

            // Range
            if (upgradedStats.detectRange < currentStats.detectRange)
                finalString += "Range: <color=red>" + upgradedStats.detectRange + "</color>\n";
            else if (upgradedStats.detectRange == currentStats.detectRange)
                finalString += "Range: " + upgradedStats.detectRange + "\n";
            else
                finalString += "Range: <color=green>" + upgradedStats.detectRange + "</color>\n";
        }

        return finalString;
    }
}