using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class GameplayManager : IGameplayManager, IService
{
    public event Action GameplayStartedEvent;
    public event Action GameplayEndedEvent;

    public bool IsGameplayStarted { get; private set; }
    public GameplayData GameplayData { get; private set; }

    public GameObject GameplayObject { get; private set; }
    public Camera HeroCamera { get; private set; }

    private List<IController> _controllers;

    public void Dispose()
    {
        foreach (var item in _controllers)
            item.Dispose();
    }

    public void Init()
    {
        GameplayData = Resources.Load<GameplayData>("Data/GameplayData");

        _controllers = new List<IController>()
        {
            new LevelController(),
            new HeroController(),
            new EnemiesController(),
            new CameraController(),
            new MatchController()
        };

        foreach (var item in _controllers)
            item.Init();
    }

    public void Update()
    {
        foreach (var item in _controllers)
            item.Update();
    }

    public T GetController<T>() where T : IController
    {
        foreach (var item in _controllers)
        {
            if (item is T controller)
            {
                return controller;
            }
        }

        throw new Exception("Controller " + typeof(T).ToString() + " have not implemented");
    }

    public void StartGameplay()
    {
        if (IsGameplayStarted)
            return;

        GameplayObject = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Gameplay/[Gameplay]"));
        HeroCamera = GameplayObject.transform.Find("CameraContainer/HeroCamera").GetComponent<Camera>();

        IsGameplayStarted = true;

        GameplayStartedEvent?.Invoke();
    }

    public void StopGameplay()
    {
        if (!IsGameplayStarted)
            return;

        foreach (var item in _controllers)
            item.ResetAll();

        Object.Destroy(GameplayObject);
        GameplayObject = null;

        IsGameplayStarted = false;

        GameplayEndedEvent?.Invoke();
    }

    public void RestartGameplay()
    {
        StopGameplay();
        StartGameplay();
    }
}