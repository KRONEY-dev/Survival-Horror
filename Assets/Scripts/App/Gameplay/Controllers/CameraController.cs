using UnityEngine;

public class CameraController : IController
{
    private IGameplayManager _gameplayManager;

    private HeroController _heroController;

    private Camera _camera;
    private Transform _cameraTransform;

    private Vector3 _cameraDistance;

    public void Init()
    {
        _gameplayManager = GameClient.Instance.GetService<IGameplayManager>();

        _heroController = _gameplayManager.GetController<HeroController>();

        _gameplayManager.GameplayStartedEvent += GameplayStartedEventHandler;
        _heroController.OnHeroPositionChangedEvent += OnHeroPositionChangedEventHandler;
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

    private void GameplayStartedEventHandler()
    {
        _camera = _gameplayManager.HeroCamera;
        _cameraTransform = _camera.transform;
        _cameraDistance = new Vector3(); //Need to set real data
    }

    private void OnHeroPositionChangedEventHandler(Vector3 position)
    {
        _cameraTransform.position = position + _cameraDistance;
    }
}