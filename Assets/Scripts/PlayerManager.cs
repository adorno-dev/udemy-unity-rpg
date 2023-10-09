using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public Player player;

    public int currency;

    private void Awake()
    {
        if (instance is not null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    public bool HaveEnoughMoney(int price)
    {
        if (price > currency)
        {
            Debug.Log("Not enough money");
            return false;
        }

        currency = currency - price;

        return true;
    }

    public int GetCurrentCurrency() => currency;
}
