using Models.Level;
using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class HeroController : IController
{
    public event Action OnHeroLoadedEvent;
    public event Action<int> OnHeroHitEvent;

    private IGameplayManager _gameplayManager;

    private LevelController _levelController;
    private CameraController _cameraController;
    private MatchController _matchController;

    private GameObject _heroPrefab;

    private Hero _hero;

    public void Init()
    {
        _gameplayManager = GameClient.Get<IGameplayManager>();

        _levelController = _gameplayManager.GetController<LevelController>();
        _cameraController = _gameplayManager.GetController<CameraController>();
        _matchController = _gameplayManager.GetController<MatchController>();

        _heroPrefab = Resources.Load<GameObject>("Prefabs/Gameplay/Hero");

        _levelController.OnLevelLoadedEvent += OnLevelLoadedEventHandler;
        _matchController.OnMatchFinishedEvent += OnMatchFinishedEventHandler;
    }

    public void ResetAll()
    {
        _hero?.Destroy();
        _hero = null;
    }

    public void Dispose()
    {
        _hero = null;
    }

    public void Update()
    {
        if (!_gameplayManager.IsGameplayStarted || !_matchController.IsMatchActive) return;

        _hero?.Update();
    }

    public Transform GetHeroTransform()
    {
        return _hero.SelfTransform;
    }

    public int GetHeroHealth()
    {
        return _hero.Health;
    }

    public void HitHero(int healthHit)
    {
        _hero.Hit(healthHit);

        if (_hero.Health == 0)
        {
            _matchController.SetEndState();
        }
    }

    private void OnLevelLoadedEventHandler()
    {
        var heroObject = Object.Instantiate(_heroPrefab, _levelController.CurrentLevel.HeroSpawnPointTransform); 
        _hero = new Hero(heroObject, _gameplayManager.GameplayData.heroConfig);

        _cameraController.SetCameraTarget(heroObject.transform);

        _hero.OnHitEvent += OnHitEventHandler;

        OnHeroLoadedEvent?.Invoke();
    }

    private void OnHitEventHandler(int hit)
    {
        OnHeroHitEvent?.Invoke(hit);
    }

    private void OnMatchFinishedEventHandler()
    {
        _hero?.StopInput();
    }
}