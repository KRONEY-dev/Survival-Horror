using Models.Level;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : IController
{
    public event Action OnLevelLoadedEvent;

    public bool Initialized { get; private set; }

    public Level CurrentLevel { get; private set; }

    private IGameplayManager _gameplayManager;

    private HeroController _heroController;
    private EnemiesController _enemiesController;

    private GameplayData.MineConfig _mineConfig;

    public void Init()
    {
        _gameplayManager = GameClient.Get<IGameplayManager>();
        _heroController = _gameplayManager.GetController<HeroController>();
        _enemiesController = _gameplayManager.GetController<EnemiesController>();

        _mineConfig = _gameplayManager.GameplayData.mineConfig;

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
        CurrentLevel = null;
    }

    private void LoadLevel(GameplayData.LevelInfo levelInfo)
    {
        CurrentLevel = new Level(levelInfo, _gameplayManager.GameplayData, _gameplayManager.GameplayObject.transform.Find("[Level]"));
        CurrentLevel.OnMineHitHeroEvent += OnMineHitHeroEventHandler;
        CurrentLevel.OnMineHitEnemiesEvent += OnMineHitEnemiesEventHandler;

        Initialized = true;

        OnLevelLoadedEvent?.Invoke();
    }

    private void GameplayStartedEventHandler()
    {
        var allLevels = _gameplayManager.GameplayData.levels;

        LoadLevel(allLevels[0]);
    }

    private void OnMineHitHeroEventHandler()
    {
        _heroController.HitHero(_mineConfig.heroHealthHit);
    }

    private void OnMineHitEnemiesEventHandler(List<GameObject> enemies)
    {
        _enemiesController.KillEnemies(enemies);
    }
}