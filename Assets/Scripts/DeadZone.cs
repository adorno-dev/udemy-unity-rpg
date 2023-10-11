using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<CharacterStats>() != null)
            other.GetComponent<CharacterStats>().KillEntity();
        else
            Destroy(other.gameObject);
        
        if (other.GetComponentInParent<ItemObject>() != null)
            Destroy(other.GetComponentInParent<ItemObject>().gameObject);
    }
}
