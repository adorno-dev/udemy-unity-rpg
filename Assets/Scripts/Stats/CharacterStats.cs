using System.Collections;
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

public class CharacterStats : MonoBehaviour
{
    private EntityFX fx;

    [Header("Major stats")]
    public Stat strength;       // 1 point increase damage by 1 and crit. power by 1%
    public Stat agility;        // 1 point increase evasion by %1 and crit. change by %1
    public Stat intelligence;   // 1 point increase magic damage by 1 and magic resistance by 3
    public Stat vitality;       // 1 point increase health by 3 or 5 points

    [Header("Offensive stats")]
    public Stat damage;
    public Stat critChance;
    public Stat critPower;      // default value 150%

    [Header("Defensive stats")]
    public Stat maxHealth;
    public Stat armor;
    public Stat evasion;
    public Stat magicResistance;

    [Header("Magic stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightingDamage;


    public bool isIgnited;      // does damage over time
    public bool isChilled;      // reduce armor by 20%
    public bool isShocked;      // reduce accuracy by 20%


    [SerializeField] private float ailmentsDuration = 4;
    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;


    private float igniteDamageCooldown = .3f;
    private float igniteDamageTimer;
    private int igniteDamage;
    [SerializeField] private GameObject shockStrikePrefab;
    private int shockDamage;


    public int currentHealth;

    public System.Action onHealthChanged;
    public bool isDead { get; private set; }
    public bool isInvincible { get; private set; }
    private bool isVulnerable;

    protected virtual void Start()
    {
        critPower.SetDefaultValue(150);

        currentHealth = GetMaxHealthValue();

        fx = GetComponent<EntityFX>();
    }

    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;

        igniteDamageTimer -= Time.deltaTime;

        if (ignitedTimer < 0)
            isIgnited = false;

        if (chilledTimer < 0)
            isChilled = false;

        if (shockedTimer < 0)
            isShocked = false;

        if (isIgnited)
            ApplyIgniteDamage();
    }

    public void MakeVulnerableFor(float duration) => StartCoroutine(VulnerableCoroutine(duration));

    private IEnumerator VulnerableCoroutine(float duration)
    {
        isVulnerable = true;

        yield return new WaitForSeconds(duration);

        isVulnerable = false;
    }

    public virtual void IncreaseStatBy(int modifier, float duration, Stat statToModify)
    {
        if (statToModify == null)
            return;

        StartCoroutine(StatModCoroutine(modifier, duration, statToModify));
    }

    private IEnumerator StatModCoroutine(int modifier, float duration, Stat statToModify)
    {
        statToModify.AddModifier(modifier);

        yield return new WaitForSeconds(duration);

        statToModify.RemoveModifier(modifier);
    }

    private void ApplyIgniteDamage()
    {
        if (igniteDamageTimer < 0)
        {
            DecreaseHealthBy(igniteDamage);

            if (currentHealth < 0 && !isDead)
                Die();

            igniteDamageTimer = igniteDamageCooldown;
        }
    }

    public virtual void DoDamage(CharacterStats targetStats)
    {
        bool criticalStrike = false;

        if (TargetCanAvoidAttack(targetStats))
            return;
        
        targetStats.GetComponent<Entity>().SetupKnockbackDir(transform);

        int totalDamage = damage.GetValue() + strength.GetValue();

        if (CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
            criticalStrike = true;
        }

        fx.CreateHitFx(targetStats.transform, criticalStrike);

        totalDamage = CheckTargetArmor(targetStats, totalDamage);

        targetStats.TakeDamage(totalDamage);

        DoMagicalDamage(targetStats); // remove if you don't want to apply magic hit on primary attack
    }

    #region Magical damage and ailments

    public virtual void DoMagicalDamage(CharacterStats targetStats)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightingDamage = lightingDamage.GetValue();

        int totalMagicDamage = _fireDamage + _iceDamage + _lightingDamage + intelligence.GetValue();
        totalMagicDamage = CheckTargetResistence(targetStats, totalMagicDamage);
        targetStats.TakeDamage(totalMagicDamage);

        if (Mathf.Max(_fireDamage, _iceDamage, _lightingDamage) <= 0)
            return;

        AttemptToApplyAilments(targetStats, _fireDamage, _iceDamage, _lightingDamage);
    }

    private void AttemptToApplyAilments(CharacterStats targetStats, int _fireDamage, int _iceDamage, int _lightingDamage)
    {
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightingDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightingDamage;
        bool canApplyShock = _lightingDamage > _fireDamage && _lightingDamage > _iceDamage;

        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            if (Random.value < .3f && _fireDamage > 0)
            {
                canApplyIgnite = true;
                targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }

            if (Random.value < .5f && _iceDamage > 0)
            {
                canApplyChill = true;
                targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }

            if (Random.value < .5f && _lightingDamage > 0)
            {
                canApplyShock = true;
                targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
        }

        if (canApplyIgnite)
            targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f));

        if (canApplyShock)
            targetStats.SetupShockStrikeDamage(Mathf.RoundToInt(_lightingDamage * .1f));

        targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
    }

    private int CheckTargetResistence(CharacterStats targetStats, int totalMagicDamage)
    {
        totalMagicDamage -= targetStats.magicResistance.GetValue() + (targetStats.intelligence.GetValue() * 3);
        totalMagicDamage = Mathf.Clamp(totalMagicDamage, 0, int.MaxValue);
        return totalMagicDamage;
    }

    public void ApplyAilments(bool ignite, bool chill, bool shock)
    {
        bool canApplyIgnite = !isIgnited && !isChilled && !isShocked;
        bool canApplyChill = !isIgnited && !isChilled && !isShocked;
        bool canApplyShock = !isIgnited && !isChilled;

        if (ignite && canApplyIgnite)
        {
            ignitedTimer = ailmentsDuration;
            isIgnited = ignite;

            fx.IgniteFxFor(ailmentsDuration);
        }

        if (chill && canApplyChill)
        {
            chilledTimer = ailmentsDuration;
            isChilled = chill;

            float slowPercentage = .2f;

            GetComponent<Entity>()?.SlowEntityBy(slowPercentage, ailmentsDuration);

            fx.ChillFxFor(ailmentsDuration);
        }

        if (shock && canApplyShock)
        {
            if (!isShocked)
            {
                ApplyShock(shock);
            }
            else
            {
                if (GetComponent<Player>() != null)
                    return;

                HitNearestTargetWithShockStrike();
            }
        }
    }

    public void ApplyShock(bool shock)
    {
        if (isShocked)
            return;

        shockedTimer = ailmentsDuration;
        isShocked = shock;

        fx.ShockFxFor(ailmentsDuration);
    }

    private void HitNearestTargetWithShockStrike()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) > 1)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }

            if (closestEnemy == null)       // delete if you don't want shocked target to be hit by shock strike
                closestEnemy = transform;
        }

        if (closestEnemy != null)
        {
            GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);
            newShockStrike.GetComponent<ShockStrike_Controller>().Setup(shockDamage, closestEnemy.GetComponent<CharacterStats>());
        }
    }

    public virtual void TakeDamage(int damage)
    {
        if (isInvincible)
            return;

        DecreaseHealthBy(damage);

        GetComponent<Entity>().DamageImpact();

        fx.StartCoroutine("FlashFX");

        if (currentHealth < 0 && !isDead)
            Die();
    }

    public virtual void IncreaseHealthBy(int amount)
    {
        currentHealth += amount;

        if (currentHealth > GetMaxHealthValue())
            currentHealth = GetMaxHealthValue();
        
        if (onHealthChanged != null)
            onHealthChanged();
    }

    protected virtual void DecreaseHealthBy(int damage)
    {
        if (isVulnerable)
            damage = Mathf.RoundToInt(damage * 1.1f);

        currentHealth -= damage;

        if (onHealthChanged != null)
            onHealthChanged();
    }

    public void SetupIgniteDamage(int damage) => igniteDamage = damage;

    public void SetupShockStrikeDamage(int damage) => shockDamage = damage;

    #endregion

    public int GetMaxHealthValue() => maxHealth.GetValue() + vitality.GetValue() * 5;

    protected virtual void Die() 
    {
        isDead = true;
    }

    public void KillEntity()
    {
        if (!isDead)
            Die();
    }

    public void MakeInvincible(bool invincible) => this.isInvincible = isInvincible;

    #region Stat calculations

    protected int CheckTargetArmor(CharacterStats targetStats, int totalDamage)
    {
        if (targetStats.isChilled)
            totalDamage -= Mathf.RoundToInt(targetStats.armor.GetValue() * .8f);
        else
            totalDamage -= targetStats.armor.GetValue();

        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    public virtual void OnEvasion()
    {

    }

    protected bool TargetCanAvoidAttack(CharacterStats targetStats)
    {
        int totalEvasion = targetStats.evasion.GetValue() + targetStats.agility.GetValue();

        if (isShocked)
            totalEvasion += 20;

        if (Random.Range(0, 100) < totalEvasion)
        {
            targetStats.OnEvasion();
            return true;
        }

        return false;
    }

    protected bool CanCrit()
    {
        int totalCriticalChance = critChance.GetValue() + agility.GetValue();
        
        if (Random.Range(0, 100) <= totalCriticalChance)
        {
            return true;
        }

        return false;
    }

    protected int CalculateCriticalDamage(int damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * 0.1f;
        float critDamage = damage * totalCritPower;

        return Mathf.RoundToInt(critDamage);
    }

    #endregion

    public Stat GetStat(StatType buffType)
    {
        switch (buffType)
        {
            case StatType.strength: return strength;
            case StatType.agility: return agility;
            case StatType.intelligence: return intelligence;
            case StatType.vitality: return vitality;
            case StatType.damage: return damage;
            case StatType.critChance: return critChance;
            case StatType.critPower: return critPower;
            case StatType.health: return maxHealth;
            case StatType.armor: return armor;
            case StatType.evasion: return evasion;
            case StatType.magicResistance: return magicResistance;
            case StatType.fireDamage: return fireDamage;
            case StatType.iceDamage: return iceDamage;
            case StatType.lightingDamage: return lightingDamage;        
            default: return null;
        };
    }

}
