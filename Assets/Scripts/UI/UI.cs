using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour, ISaveManager
{
    [Header("End screen")]
    [SerializeField] private UI_FadeScreen fadeScreen;
    [SerializeField] private GameObject endText;
    [SerializeField] private GameObject restartButton;
    [Space]

    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject inGameUI;

    public UI_SkillToolTip skillToolTip;
    public UI_ItemToolTip itemToolTip;
    public UI_StatToolTip statToolTip;
    public UI_CraftWindow craftWindow;

    [SerializeField] private UI_VolumeSlider[] volumeSettings;

    private void Awake()
    {
        SwitchTo(skillTreeUI); // we need this to assin events on skill tree stats before we assign events on skill scripts
        fadeScreen.gameObject.SetActive(true);
    }

    void Start()
    {
        SwitchTo(inGameUI);

        itemToolTip.gameObject.SetActive(false);
        statToolTip.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            SwitchWithKeyTo(characterUI);

        if (Input.GetKeyDown(KeyCode.B))
            SwitchWithKeyTo(craftUI);

        if (Input.GetKeyDown(KeyCode.K))
            SwitchWithKeyTo(skillTreeUI);

        if (Input.GetKeyDown(KeyCode.O))
            SwitchWithKeyTo(optionsUI);
    }

    public void SwitchTo(GameObject menu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            bool isFadeScreen = transform.GetChild(i).GetComponent<UI_FadeScreen>() != null; // we need this to keep fade screen game object active

            if (!isFadeScreen)
                transform.GetChild(i).gameObject.SetActive(false);
        }

        if (menu != null)
        {
            // AudioManager.instance?.PlaySFX(7, null);

            if (AudioManager.instance != null)
                AudioManager.instance.PlaySFX(7);

            menu.SetActive(true);
        }
        
        if (GameManager.instance != null)
        {
            if (menu == inGameUI)
                GameManager.instance.PauseGame(false);
            else
                GameManager.instance.PauseGame(true);
        }
    }

    public void SwitchWithKeyTo(GameObject menu)
    {
        if (menu != null && menu.activeSelf)
        {
            menu.SetActive(false);
            CheckForGameUI();
            return;
        }

        SwitchTo(menu);
    }

    private void CheckForGameUI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf && transform.GetChild(i).GetComponent<UI_FadeScreen>() == null)
                return;
        }

        SwitchTo(inGameUI);
    }

    public void SwitchOnEndScreen()
    {
        fadeScreen.gameObject.SetActive(true);
        fadeScreen.FadeOut();
        StartCoroutine(EndScreenCoroutine());
    }

    IEnumerator EndScreenCoroutine()
    {
        yield return new WaitForSeconds(1);

        endText.SetActive(true);

        yield return new WaitForSeconds(1);

        restartButton.SetActive(true);
    }

    public void RestartGameButton() => GameManager.instance.RestartScene();

    public void LoadData(GameData data)
    {
        foreach (KeyValuePair<string, float> item in data.volumeSettings)
        {
            foreach (UI_VolumeSlider volume in volumeSettings)
            {
                if (volume.parameter == item.Key)
                    volume.LoadSlider(item.Value);
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        data.volumeSettings.Clear();

        foreach (UI_VolumeSlider volume in volumeSettings)
        {
            data.volumeSettings.Add(volume.parameter, volume.slider.value);
        }
    }
}
