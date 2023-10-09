[System.Serializable]
public class GameData
{
    public int currency;

    public SerializableDictionary<string, int> inventory;

    public GameData()
    {
        currency = 0;

        inventory = new SerializableDictionary<string, int>();
    }
}