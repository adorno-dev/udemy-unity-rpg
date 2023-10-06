using UnityEngine;

public class ThunderStrike_Controller : MonoBehaviour
{
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Enemy>() != null)
        {
            PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

            EnemyStats enemyTarget = other.GetComponent<EnemyStats>();

            playerStats.DoMagicalDamage(enemyTarget);
        }
    }
}
