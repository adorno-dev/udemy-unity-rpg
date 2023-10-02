using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public Player player;

    private void Awake()
    {
        if (instance is not null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }
}
