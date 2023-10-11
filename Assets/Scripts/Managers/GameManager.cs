using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveManager
{
    public static GameManager instance;

    private Transform player;

    [SerializeField] private Checkpoint[] checkpoints;
    [SerializeField] private string closestCheckpointId;

    [Header("Lost currency")]
    [SerializeField] private GameObject lostCurrencyPrefab;
    public int lostCurrencyAmount;
    [SerializeField] private float lostCurrencyX;
    [SerializeField] private float lostCurrencyY;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        checkpoints = checkpoints = FindObjectsOfType<Checkpoint>(true);

        player = PlayerManager.instance.player.transform;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            RestartScene();
    }

    public void RestartScene()
    {
        SaveManager.instance.SaveGame();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void LoadData(GameData data) => StartCoroutine(LoadWithDelay(data));

    private void LoadCheckpoints(GameData data)
    {
        foreach (KeyValuePair<string, bool> entries in data.checkpoints)
        {
            foreach (Checkpoint checkpoint in checkpoints)
            {
                if (checkpoint.id == entries.Key && entries.Value == true)
                    checkpoint.ActivateCheckpoint();
            }
        }
    }

    private void LoadLostCurrency(GameData data)
    {
        lostCurrencyAmount = data.lostCurrencyAmount;
        lostCurrencyX = data.lostCurrencyX;
        lostCurrencyY = data.lostCurrencyY;

        if (lostCurrencyAmount > 0)
        {
            var newLostCurrency = Instantiate(lostCurrencyPrefab, new Vector3(lostCurrencyX, lostCurrencyY), Quaternion.identity);
            newLostCurrency.GetComponent<LostCurrencyController>().currency = lostCurrencyAmount;
        }

        lostCurrencyAmount = 0;
    }

    private IEnumerator LoadWithDelay(GameData data)
    {
        yield return new WaitForSeconds(.1f);

        LoadCheckpoints(data);
        LoadClosestCheckpoint(data);
        LoadLostCurrency(data);
    }

    public void SaveData(ref GameData data)
    {
        data.lostCurrencyAmount = lostCurrencyAmount;
        data.lostCurrencyX = player.position.x;
        data.lostCurrencyY = player.position.y;

        data.closestCheckpointId = FindClosestCheckpoint()?.id;
        data.checkpoints.Clear();

        foreach (var checkpoint in checkpoints)
        {
            data.checkpoints.Add(checkpoint.id,  checkpoint.activationStatus);
        }
    }

    private void LoadClosestCheckpoint(GameData data)
    {
        if (data.closestCheckpointId == null)
            return;

        closestCheckpointId = data.closestCheckpointId;

        foreach (Checkpoint checkpoint in checkpoints)
        {
            if (closestCheckpointId == checkpoint.id)
                player.position = checkpoint.transform.position;
        }
    }

    private Checkpoint FindClosestCheckpoint()
    {
        Checkpoint closestCheckpoint = null;

        float closestDistance = Mathf.Infinity;
        
        foreach (var checkpoint in checkpoints)
        {
            float distanceToCheckpoint = Vector2.Distance(player.position, checkpoint.transform.position);

            if (distanceToCheckpoint < closestDistance && checkpoint.activationStatus == true)
            {
                closestDistance = distanceToCheckpoint;
                closestCheckpoint = checkpoint;
            }
        }

        return closestCheckpoint;
    }

    public void PauseGame(bool pause)
    {
        if (pause) Time.timeScale = 0;
        else Time.timeScale = 1;
    }

}