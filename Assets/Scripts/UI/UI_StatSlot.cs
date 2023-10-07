using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_StatSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string statName;
    [SerializeField] private StatType statType;
    [SerializeField] private TextMeshProUGUI statValueText;
    [SerializeField] private TextMeshProUGUI statNameText;

    [TextArea]
    [SerializeField] private string statDescription;

    private UI ui;

    private void OnValidate()
    {
        gameObject.name = $"Stat - {statName}";

        if (statNameText != null)
            statNameText.text = statName;
    }

    void Start()
    {
        ui = GetComponentInParent<UI>();

        UpdateStatValueUI();
    }

    public void UpdateStatValueUI()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        if (playerStats != null)
        {
            statValueText.text = $"{playerStats.GetStat(statType).GetValue()}";

            if (statType == StatType.health)
                statValueText.text = $"{playerStats.GetMaxHealthValue()}";

            if (statType == StatType.damage)
                statValueText.text = $"{playerStats.damage.GetValue() + playerStats.strength.GetValue()}";

            if (statType == StatType.critPower)
                statValueText.text = $"{playerStats.critPower.GetValue() + playerStats.strength.GetValue()}";

            if (statType == StatType.critChance)
                statValueText.text = $"{playerStats.critChance.GetValue() + playerStats.agility.GetValue()}";

            if (statType == StatType.evasion)
                statValueText.text = $"{playerStats.evasion.GetValue() + playerStats.agility.GetValue()}";

            if (statType == StatType.magicResistance)
                statValueText.text = $"{playerStats.magicResistance.GetValue() + playerStats.intelligence.GetValue() * 3}";
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.statToolTip.ShowStatToolTip(statDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.statToolTip.HideStatToolTip();
    }
}