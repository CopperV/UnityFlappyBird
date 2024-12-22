using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance { get { return instance; } }

    [SerializeField]
    private Transform spawnPoint;
    [SerializeField]
    private GameObject playerPrefab;

    private GameObject localPlayer;
    public GameObject LocalPlayer {  get { return localPlayer; } }

    public static UnityAction StartGameEvent;
    public static UnityAction StopGameEvent;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        if(localPlayer != null && !localPlayer.IsDestroyed())
        {
            Destroy(localPlayer);
            localPlayer = null;
        }

        localPlayer = Instantiate(playerPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation, null);

        StartGameEvent?.Invoke();
    }

    public void StopGame()
    {
        Debug.Log("Stopping the Game");

        StopGameEvent?.Invoke();

        Time.timeScale = 0f;
    }
}
