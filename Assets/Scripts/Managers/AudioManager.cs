using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private float sfxMinimumDistance;
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;

    public bool playBgm;
    private int bgmIndex;

    private void Awake()
    {
        if (instance != null) Destroy(instance.gameObject);
        else instance = this;
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

    public void PlaySFX(int sfxIndex, Transform source)
    {
        if (sfx[sfxIndex].isPlaying)
            return;

        if (source != null && Vector2.Distance(PlayerManager.instance.player.transform.position, source.position) > sfxMinimumDistance)
            return;

        if (sfxIndex < sfx.Length)
        {
            sfx[sfxIndex].pitch = Random.Range(.85f, 1.1f);
            sfx[sfxIndex].Play();
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

}
