using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private Image itemIcon;
    [SerializeField] private Button craftButton;

    [SerializeField] private Image[] materialImage;

    public void SetupCraftWindow(ItemData_Equipment data)
    {
        craftButton.onClick.RemoveAllListeners();

        for (int i = 0; i < materialImage.Length; i++)
        {
            materialImage[i].color = Color.clear;
            materialImage[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.clear;
        }

        for (int i = 0; i < data.craftingMaterials.Count; i++)
        {
            if (data.craftingMaterials.Count > materialImage.Length)
                Debug.LogWarning("You have more materials amount than you have material slots in craft window.");

            materialImage[i].color = Color.white;
            materialImage[i].sprite = data.craftingMaterials[i].data.itemIcon;

            var materialSlot = materialImage[i].GetComponentInChildren<TextMeshProUGUI>();
            materialSlot.color = Color.white;
            materialSlot.text = data.craftingMaterials[i].stackSize.ToString();
        }

        itemIcon.sprite = data.itemIcon;
        itemName.text = data.itemName;
        itemDescription.text = data.GetDescription();

        craftButton.onClick.AddListener(() => Inventory.instance.CanCraft(data, data.craftingMaterials));
    }
}
