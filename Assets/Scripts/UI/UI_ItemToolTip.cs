using TMPro;
using UnityEngine;

public class UI_ItemToolTip : MonoBehaviour
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

        if (itemName.text.Length > 12)
            itemName.fontSize = itemName.fontSize * .7f;
        else
            itemName.fontSize = defaultFontSize;

        gameObject.SetActive(true);
    }

    public void HideToolTip()
    {
        itemName.fontSize = defaultFontSize;
        gameObject.SetActive(false);
    }
}
