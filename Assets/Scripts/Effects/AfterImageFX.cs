using UnityEngine;

public class AfterImageFX : MonoBehaviour
{
    private SpriteRenderer sr;
    private float colorLooseRate;

    public void SetupAfterImage(float loosingSpeed, Sprite spriteImage)
    {
        sr = GetComponent<SpriteRenderer>();

        sr.sprite = spriteImage;
        colorLooseRate = loosingSpeed;
    }
    
    private void Update()
    {
        var alpha = sr.color.a - colorLooseRate * Time.deltaTime;
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);

        if (sr.color.a <= 0)
            Destroy(gameObject);
    }
}
