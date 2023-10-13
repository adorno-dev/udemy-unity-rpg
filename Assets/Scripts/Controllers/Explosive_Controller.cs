using UnityEngine;

public class Explosive_Controller : MonoBehaviour
{
    private Animator anim;
    private CharacterStats myStats;
    private float growSpeed = 15;
    private float maxSize = 6;
    private float explosionRadius;

    private bool canGrow = true;

    private void Update()
    {
        if (canGrow)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        
        if (maxSize - transform.localScale.x < .5f)
        {
            canGrow = false;
            anim.SetTrigger("Explode");
        }
    }

    public void SetupExplosive(CharacterStats myStats, float growSpeed, float maxSize, float explosionRadius)
    {
        anim = GetComponent<Animator>();

        this.myStats = myStats;
        this.growSpeed = growSpeed;
        this.maxSize = maxSize;
        this.explosionRadius = explosionRadius;
    }

    private void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<CharacterStats>() != null)
            {
                hit.GetComponent<Entity>().SetupKnockbackDir(transform);
                myStats.DoDamage(hit.GetComponent<CharacterStats>());
            }
        }
    }

    private void SelfDestroy() => Destroy(gameObject);
}