using System.Collections;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    #region Components

    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public SpriteRenderer sr { get; private set; }
    public CharacterStats stats { get; private set; }
    public CapsuleCollider2D cd { get; private set; }

    #endregion

    [Header("Knockback info")]
    [SerializeField] protected Vector2 knockbackPower;
    [SerializeField] protected Vector2 knockbackOffset;
    [SerializeField] protected float knockbackDuration;
    protected bool isKnocked;

    [Header("Collision info")]
    public Transform attackCheck;
    public float attackCheckRadius;
    [Space]
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [Space]
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;

    public int knockbackDir { get; private set; }

    public int facingDir { get; private set; } = 1;
    protected bool facingRight = true;


    public System.Action onFlipped;


    protected virtual void Awake() { }

    protected virtual void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<CharacterStats>();
        cd = GetComponent<CapsuleCollider2D>();
    }

    protected virtual void Update() { }

    public virtual void SetupKnockbackDir(Transform damageDirection)
    {
        if (damageDirection.position.x > transform.position.x)
            knockbackDir = -1;
        else if (damageDirection.position.x < transform.position.x)
            knockbackDir = 1;
    }

    public void SetupKnockbackPower(Vector2 knockbackPower) => this.knockbackPower = knockbackPower;

    protected virtual void SetupZeroKnockbackPower() { }

    protected virtual IEnumerator HitKnockback()
    {
        isKnocked = true;

        float xOffset = Random.Range(knockbackOffset.x, knockbackOffset.y);

        // if (knockbackPower.x > 0 || knockbackPower.y > 0)   // this line makes player immune to freeze effect when he takes hit.
        rb.velocity = new Vector2((knockbackPower.x + xOffset) * knockbackDir, knockbackPower.y);

        yield return new WaitForSeconds(knockbackDuration);

        isKnocked = false;

        SetupZeroKnockbackPower();
    }

    protected virtual void ReturnDefaultSpeed()
    {
        anim.speed = 1;
    }

    public virtual void SlowEntityBy(float slowPercentage, float slowDuration) { }

    public virtual void DamageImpact() => StartCoroutine("HitKnockback");

    #region Velocity

    public void SetZeroVelocity()
    {
        if (isKnocked)
            return;

        rb.velocity = new Vector2(0, 0);
    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        if (isKnocked)
            return;

        rb.velocity = new Vector2(xVelocity, yVelocity);

        FlipController(xVelocity);
    }

    #endregion

    #region Collision

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * facingDir, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }

    public virtual bool IsGroundDetected()
        => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

    public virtual bool IsWallDetected()
        => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);

    #endregion

    #region Flip

    public virtual void Flip()
    {
        facingRight = !facingRight;
        facingDir = facingDir * -1;
        transform.Rotate(0, 180, 0);

        if (onFlipped != null)
            onFlipped();
    }

    public virtual void FlipController(float x)
    {
        if (x > 0 && !facingRight)
            Flip();
        else if (x < 0 && facingRight)
            Flip();
    }

    public virtual void SetupDefaultFacingDir(int direction)
    {
        facingDir = direction;

        if (facingDir == -1)
            facingRight = false;
    }

    #endregion

    public virtual void Die() { }
}
