using Extensions;
using Models.Level;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesController : IController
{
    private IGameplayManager _gameplayManager;

    private MatchController _matchController;
    private LevelController _levelController;
    private HeroController _heroController;

    private GameplayData.MainGameplayConfig _mainGameplayConfig;
    private GameplayData.EnemyConfig _enemyConfig;

    private GameObject _enemyPrefab;
    private Transform _enemyParentTransform;

    private List<Enemy> _enemies;

    private Transform _enemiesTarget;

    private Timer _enemiesSpawnTimer;

    private bool _canSpawnEnemy;

    public void Init()
    {
        _gameplayManager = GameClient.Get<IGameplayManager>();

        _matchController = _gameplayManager.GetController<MatchController>();
        _levelController = _gameplayManager.GetController<LevelController>();
        _heroController = _gameplayManager.GetController<HeroController>();

        var gameplayData = _gameplayManager.GameplayData;
        _mainGameplayConfig = gameplayData.mainGameplayConfig;
        _enemyConfig = gameplayData.enemyConfig;

        _enemyPrefab = Resources.Load<GameObject>("Prefabs/Gameplay/Enemy");
        _enemies = new List<Enemy>();

        _enemiesSpawnTimer = new Timer();

        _matchController.OnMatchFinishedEvent += OnMatchFinishedEventHandler;
        _levelController.OnLevelLoadedEvent += OnLevelLoadedEventHandler;
        _heroController.OnHeroLoadedEvent += OnHeroLoadedEventHandler;
    }

    public void ResetAll()
    {
        foreach (var enemy in _enemies)
        {
            enemy.Destroy();
        }

        _enemies.Clear();
    }

    public void Dispose()
    {
        _enemies.Clear();
    }

    public void Update()
    {
        if (!_gameplayManager.IsGameplayStarted || !_matchController.IsMatchActive) return;

        _enemiesSpawnTimer?.Update();

        foreach (Enemy enemy in _enemies)
        {
            enemy.Update();
        }
    }

    public void KillEnemies(List<GameObject> enemiesObjects)
    {
        var enemiesToKill = new List<Enemy>();

        foreach (Enemy enemy in _enemies)
        {
            foreach (GameObject enemyObject in enemiesObjects)
            {
                if (enemy.IsTheSameGameObject(enemyObject))
                {
                    enemiesToKill.Add(enemy);
                    break;
                }
            }
        }

        foreach (Enemy enemy in enemiesToKill)
        {
            enemy.Destroy();
            _enemies.Remove(enemy);
        }
    }

    private void SpawnEnemy()
    {
        var enemyObject = Object.Instantiate(_enemyPrefab, _levelController.GetRandomSafePosition(), Quaternion.identity, _enemyParentTransform);
        var enemy = new Enemy(enemyObject, _enemiesTarget, _enemyConfig);
        enemy.OnHitHeroEvent += OnHitHeroEventHandler;

        _enemies.Add(enemy);
    }

    private void OnMatchFinishedEventHandler()
    {
        _enemiesSpawnTimer?.Stop();

        _canSpawnEnemy = false;
    }

    private void OnLevelLoadedEventHandler()
    {
        _enemyParentTransform = _levelController.CurrentLevel.EnemiesParentTransform;
    }

    private void OnHeroLoadedEventHandler()
    {
        _enemiesTarget = _heroController.GetHeroTransform();

        _canSpawnEnemy = true;

        _enemiesSpawnTimer.Start(60 / _mainGameplayConfig.enemiesPerMinute, EnemiesSpawnTimerCallback, true);
    }

    private void EnemiesSpawnTimerCallback()
    {
        if (_canSpawnEnemy)
        {
            SpawnEnemy();
        }
    }

    private void OnHitHeroEventHandler()
    {
        _heroController.HitHero(_enemyConfig.heroHealthHit);
    }
}