using TMPro;
using UnityEngine;

public class UI_SkillToolTip : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI skillDescription;
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillCost;
    [SerializeField] private float defaultNameFontSize;

    public void ShowToolTip(string skillDescription, string skillName, int price)
    {
        this.skillName.text = skillName;
        this.skillDescription.text = skillDescription;
        this.skillCost.text = $"Cost: {price}";

        base.AdjustPosition();
        base.AdjustFontSize(this.skillName);

        gameObject.SetActive(true);
    }

    public void HideToolTip()
    {
        skillName.fontSize = defaultNameFontSize;
        gameObject.SetActive(false);
    }
}
