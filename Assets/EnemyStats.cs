public class EnemyStats : CharacterStats
{
    private Enemy enemy;

    protected override void Start()
    {
        base.Start();

        enemy = GetComponent<Enemy>();
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        enemy.DamageEffect();
    }

    protected override void Die()
    {
        base.Die();

        enemy.Die();
    }
}
