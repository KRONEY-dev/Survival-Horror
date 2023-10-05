using Models.Level;
using System;

public class LevelController : IController
{
    public event Action OnLevelLoadedEvent;

    private IGameplayManager _gameplayManager;

    public bool Initialized { get; private set; }

    public Level CurrentLevel { get; private set; }

    public void Init()
    {
        _gameplayManager = GameClient.Get<IGameplayManager>();

        _gameplayManager.GameplayStartedEvent += GameplayStartedEventHandler;
    }

    public void ResetAll()
    {
        CurrentLevel?.Dispose();
        CurrentLevel = null;

        Initialized = false;
    }

    public void Update()
    {
        if (!_gameplayManager.IsGameplayStarted)
            return;

        if (!Initialized)
            return;

        CurrentLevel?.Update();
    }

    public void Dispose()
    {
        CurrentLevel?.Dispose();
    }

    private void LoadLevel(GameplayData.LevelInfo levelInfo)
    {
        CurrentLevel = new Level(levelInfo, _gameplayManager.GameplayData, _gameplayManager.GameplayObject.transform.Find("[Level]"));

        Initialized = true;

        OnLevelLoadedEvent?.Invoke();
    }

    private void GameplayStartedEventHandler()
    {
        var allLevels = _gameplayManager.GameplayData.levels;

        LoadLevel(allLevels[0]);
    }
}