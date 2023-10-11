using System.Collections;
using Cinemachine;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    [Header("Screen shake FX")]
    [SerializeField] private float shakeMultiplier;
    public Vector3 shakeSwordImpact;
    public Vector3 shakeHighDamage;
    private CinemachineImpulseSource screenShake;

    [Header("After image FX")]
    [SerializeField] private GameObject afterImagePrefab;
    [SerializeField] private float colorLooseRate;
    [SerializeField] private float afterImageCooldown;

    private float afterImageCooldownTimer;

    [Header("Flash FX")]
    [SerializeField] private Material hitMat;
    [SerializeField] private float flashDuration;

    private SpriteRenderer sr;    
    private Material originalMat;
    private Player player;

    [Header("Ailment colors")]
    [SerializeField] private Color[] chillColor;
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] shockedColor;

    [Header("Ailment particles")]
    [SerializeField] private ParticleSystem igniteFx;
    [SerializeField] private ParticleSystem chillFx;
    [SerializeField] private ParticleSystem shockFx;

    [Header("Hit FX")]
    [SerializeField] private GameObject hitFx;
    [SerializeField] private GameObject criticalHitFx;

    [Space]
    [Header("Dust FX")]
    [SerializeField] private ParticleSystem dustFx;


    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMat = sr.material;
        screenShake = GetComponent<CinemachineImpulseSource>();
        player = PlayerManager.instance.player;
    }

    private void Update()
    {
        afterImageCooldownTimer -= Time.deltaTime;
    }

    public void ScreenShakeFX(Vector3 shakePower)
    {
        screenShake.m_DefaultVelocity = new Vector3(shakePower.x * player.facingDir, shakePower.y) * shakeMultiplier;
        screenShake.GenerateImpulse();
    }

    public void CreateAfterImageFX()
    {
        if (afterImageCooldownTimer < 0)
        {
            afterImageCooldownTimer = afterImageCooldown;

            var newAfterImage = Instantiate(afterImagePrefab, transform.position, transform.rotation);

            newAfterImage.GetComponent<AfterImageFX>().SetupAfterImage(colorLooseRate, sr.sprite);   
        }
    }

    public void MakeTransparent(bool transparent)
    {
        if (transparent)
            sr.color = Color.clear;
        else
            sr.color = Color.white;
    }

    private IEnumerator FlashFX()
    {
        sr.material = hitMat;

        Color currentColor = sr.color;

        sr.color = Color.white;

        yield return new WaitForSeconds(flashDuration);

        sr.color = currentColor;

        sr.material = originalMat;
    }

    private void RedColorBlink()
    {
        if (sr.color != Color.white)
            sr.color = Color.white;
        else
            sr.color = Color.red;
    }

    private void CancelColorChange()
    {
        CancelInvoke();

        sr.color = Color.white;

        igniteFx.Stop();
        chillFx.Stop();
        shockFx.Stop();
    }

    private void IgniteColorFx()
    {
        if (sr.color != igniteColor[0])
            sr.color = igniteColor[0];
        else
            sr.color = igniteColor[1];
    }

    private void ChillColorFx()
    {
        if (sr.color != chillColor[0])
            sr.color = chillColor[0];
        else
            sr.color = chillColor[1];
    }

    private void ShockColorFx()
    {
        if (sr.color != shockedColor[0])
            sr.color = shockedColor[0];
        else
            sr.color = shockedColor[1];
    }

    public void IgniteFxFor(float seconds)
    {
        igniteFx.Play();
        InvokeRepeating("IgniteColorFx", 0, .3f);
        Invoke("CancelColorChange", seconds);
    }

    public void ChillFxFor(float seconds)
    {
        chillFx.Play();
        InvokeRepeating("ChillColorFx", 0, .3f);
        Invoke("CancelColorChange", seconds);
    }

    public void ShockFxFor(float seconds)
    {
        shockFx.Play();
        InvokeRepeating("ShockColorFx", 0, .3f);
        Invoke("CancelColorChange", seconds);
    }

    public void CreateHitFx(Transform target, bool critical)
    {
        float zRotation = Random.Range(-90, 90);
        float xPosition = Random.Range(-.5f, .5f);
        float yPosition = Random.Range(-.5f, .5f);

        var hitFxRotation = new Vector3(0, 0, zRotation);
        var hitPrefab = hitFx;

        if (critical)
        {
            float yRotation = 0;

            hitPrefab = criticalHitFx;
            zRotation = Random.Range(-45, 45);

            if (GetComponent<Entity>().facingDir == -1)
                yRotation = 180;
            
            hitFxRotation = new Vector3(0, yRotation, zRotation);
        }

        var newHitFx = Instantiate(hitPrefab, target.position + new Vector3(xPosition, yPosition), Quaternion.identity); //, target); // if you want particle to follow target
        newHitFx.transform.Rotate(hitFxRotation);

        Destroy(newHitFx, .5f);
    }

    public void PlayDustFX()
    {
        if (dustFx != null)
            dustFx.Play();
    }
}
