using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    [SerializeField] private string fileName;

    private List<ISaveManager> saveManagers;
    private FileDataHandler dataHandler;
    private GameData gameData;


    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else 
            instance = this;
    }

    private void Start()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);

        saveManagers = FindAllSaveManagers();

        LoadGame();
    }

    [ContextMenu("Delete save file")]
    private void DeleteSaveData()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        dataHandler.Delete();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        gameData = dataHandler.Load();

        if (gameData == null)
        {
            Debug.Log("No saved data found!");
            NewGame();
        }

        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.SaveData(ref gameData);
        }

        dataHandler.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<ISaveManager> FindAllSaveManagers()
    {
        IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveManager>();

        return new List<ISaveManager>(saveManagers);
    }
}