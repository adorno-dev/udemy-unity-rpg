using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private float sfxMinimumDistance;
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;

    public bool playBgm;
    private int bgmIndex;

    private bool canPlaySFX;

    private void Awake()
    {
        if (instance != null) Destroy(instance.gameObject);
        else instance = this;

        Invoke("AllowSFX", 1f);
    }


    private void Update()
    {
        if (!playBgm)
            StopAllBGM();
        else
        {
            if (!bgm[bgmIndex].isPlaying)
                PlayBGM(bgmIndex);
        }
    }

    public void PlaySFX(int sfxIndex, Transform source = null)
    {
        // if (sfx[sfxIndex].isPlaying)
        //     return;

        if (canPlaySFX == false)
            return;

        if (source != null && Vector2.Distance(PlayerManager.instance.player.transform.position, source.position) > sfxMinimumDistance)
            return;

        if (sfxIndex < sfx.Length)
        {
            sfx[sfxIndex].pitch = Random.Range(.85f, 1.1f);
            sfx[sfxIndex].Play();
        }
    }

    public void StopSFXWithTime(int index) => StartCoroutine(DecreaseVolume(sfx[index]));

    private IEnumerator DecreaseVolume(AudioSource audio)
    {
        float defaultVolume = audio.volume;

        while (audio.volume > .1f)
        {
            audio.volume -= audio.volume * .2f;

            yield return new WaitForSeconds(.7f);

            if (audio.volume <= .1f)
            {

                audio.Stop();
                audio.volume = defaultVolume;
                break;
            }
        }
    }

    public void StopSFX(int index) => sfx[index].Stop();

    public void PlayRandomBGM()
    {
        bgmIndex = Random.Range(0, bgm.Length);

        PlayBGM(bgmIndex);
    }

    public void PlayBGM(int bgmIndex)
    {
        this.bgmIndex = bgmIndex;

        this.StopAllBGM();

        bgm[this.bgmIndex].Play();
    }

    public void StopAllBGM()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }

    private void AllowSFX() => canPlaySFX = true;

}
