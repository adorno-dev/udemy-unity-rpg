using TMPro;
using UnityEngine;

public class UI_ItemToolTip : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemType;
    [SerializeField] private TextMeshProUGUI itemDescription;

    [SerializeField] private int defaultFontSize = 32;

    public void ShowToolTip(ItemData_Equipment item)
    {
        if (item == null)
            return;

        itemName.text = item.itemName;
        itemType.text = item.equipmentType.ToString();
        itemDescription.text = item.GetDescription();

        base.AdjustFontSize(itemName);
        base.AdjustPosition();

        gameObject.SetActive(true);
    }

    public void HideToolTip()
    {
        itemName.fontSize = defaultFontSize;
        gameObject.SetActive(false);
    }
}
