using System;
using UnityEngine;

public interface IGameplayManager
{
    event Action GameplayStartedEvent;
    event Action GameplayEndedEvent;

    bool IsGameplayStarted { get; }
    GameplayData GameplayData { get; }
    GameObject GameplayObject { get; }
    Camera HeroCamera { get; }

    T GetController<T>() where T : IController;

    void StartGameplay();
    void StopGameplay();
    void RestartGameplay();
}