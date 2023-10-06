using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public event Action ApplicationPausedEvent;

    private static GameManager _instance;
    public static GameManager Instance
    {
        get { return _instance; }
        private set { _instance = value; }
    }

    private GameClient _gameClient;
    private LoadingController _loadingController;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        GameClient.ParentForPrefabedManagers = transform;
        _gameClient = GameClient.Instance;

        _gameClient.InitServices();

        _loadingController = new LoadingController();
        _loadingController.StartGame();
    }

    private void Update()
    {
        if (Instance == this)
            _gameClient?.Update();
    }

    private void OnDestroy()
    {
        if (Instance == this)
            _gameClient?.Dispose();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            ApplicationPausedEvent?.Invoke();
        }
    }
}