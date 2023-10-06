using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ItemData itemData;

    private void SetupVisuals()
    {
        if (itemData == null)
            return;

        GetComponent<SpriteRenderer>()
            .sprite = itemData.icon;

        gameObject.name = $"Item object - {itemData.itemName}";
    }

    private void Start()
    {
    }

    public void SetupItem(ItemData itemData, Vector2 velocity)
    {
        this.itemData = itemData;

        rb.velocity = velocity;

        SetupVisuals();
    }

    public void PickupItem()
    {
        Inventory.instance.AddItem(itemData);
        Destroy(gameObject);
    }
}
