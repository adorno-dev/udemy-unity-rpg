using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player player;

    protected override void Start()
    {
        base.Start();

        // player = GetComponent<Player>();
        player = PlayerManager.instance.player;
    }

    protected override void Die()
    {
        base.Die();

        player.Die();

        GameManager.instance.lostCurrencyAmount = PlayerManager.instance.currency;
        PlayerManager.instance.currency = 0;

        GetComponent<PlayerItemDrop>()?.GenerateDrop();
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }

    protected override void DecreaseHealthBy(int damage)
    {
        base.DecreaseHealthBy(damage);

        ItemData_Equipment currentArmor = Inventory.instance.GetEquipment(EquipmentType.Armor);

        if (currentArmor != null)
            currentArmor.Effect(player.transform);
    }

    public override void OnEvasion()
    {
        player.skill.dodge.CreateMirageOnDodge();
    }

    public void CloneDoDamage(CharacterStats targetStats, float multiplier)
    {
        if (TargetCanAvoidAttack(targetStats))
            return;

        int totalDamage = damage.GetValue() + strength.GetValue();

        if (multiplier > 0)
            totalDamage = Mathf.RoundToInt(totalDamage * multiplier);

        if (CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
        }

        totalDamage = CheckTargetArmor(targetStats, totalDamage);

        targetStats.TakeDamage(totalDamage);

        DoMagicalDamage(targetStats); // remove if you don't want to apply magic hit on primary attack
    }
}
