using UnityEngine;

public enum StatType
{
    strength,
    agility,
    intelligence,
    vitality,
    damage,
    critChance,
    critPower,
    health,
    armor,
    evasion,
    magicResistance,
    fireDamage,
    iceDamage,
    lightingDamage
}

[CreateAssetMenu(fileName = "Buff effect", menuName = "Data/Item effect/Buff effect")]
public class Buff_Effect : ItemEffect
{
    private PlayerStats stats;

    [SerializeField] private StatType buffType;
    [SerializeField] private int buffAmount;
    [SerializeField] private float buffDuration;

    public override void ExecuteEffect(Transform enemyPosition)
    {
        stats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        stats.IncreaseStatBy(buffAmount, buffDuration, StatToModify());
    }

    private Stat StatToModify()
    {
        switch (buffType)
        {
            case StatType.strength: return stats.strength;
            case StatType.agility: return stats.agility;
            case StatType.intelligence: return stats.intelligence;
            case StatType.vitality: return stats.vitality;
            case StatType.damage: return stats.damage;
            case StatType.critChance: return stats.critChance;
            case StatType.critPower: return stats.critPower;
            case StatType.health: return stats.maxHealth;
            case StatType.armor: return stats.armor;
            case StatType.evasion: return stats.evasion;
            case StatType.magicResistance: return stats.magicResistance;
            case StatType.fireDamage: return stats.fireDamage;
            case StatType.iceDamage: return stats.iceDamage;
            case StatType.lightingDamage: return stats.lightingDamage;        
            default: return null;
        };
    }
}
