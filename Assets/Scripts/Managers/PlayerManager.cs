using UnityEngine;

public class PlayerManager : MonoBehaviour, ISaveManager
{
    public static PlayerManager instance;

    public Player player;

    public int currency;

    private void Awake()
    {
        if (instance != null)
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

    public int GetCurrency() => currency;

    public void LoadData(GameData data)
    {
        this.currency = data.currency;
    }

    public void SaveData(ref GameData data)
    {
        data.currency = this.currency;
    }
}
