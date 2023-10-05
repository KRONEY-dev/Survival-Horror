using Models.Level;
using System;
using UnityEngine;

public class HeroController : IController
{
    public event Action<Vector3> OnHeroPositionChangedEvent;

    private LevelController _levelController;
    private MatchController _matchController;

    private Hero _hero;

    public void Init()
    {
        var gameplayManager = GameClient.Instance.GetService<IGameplayManager>();

        _levelController = gameplayManager.GetController<LevelController>();
        _matchController = gameplayManager.GetController<MatchController>();

        _levelController.OnLevelLoadedEvent += OnLevelLoadedEventHandler;
    }

    public void ResetAll()
    {
    }

    public void Dispose()
    {
    }

    public void Update()
    {
    }

    private void OnLevelLoadedEventHandler()
    {
        _hero = _levelController.CurrentLevel.Hero;
    }
}