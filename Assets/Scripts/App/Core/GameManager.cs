using UnityEngine;

public class GameManager : MonoBehaviour
{
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
        _gameClient?.Update();
    }

    private void OnDestroy()
    {
        _gameClient?.Dispose();
    }
}