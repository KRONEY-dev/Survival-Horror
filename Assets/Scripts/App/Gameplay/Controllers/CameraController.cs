using UnityEngine;

public class CameraController : IController
{
    private IGameplayManager _gameplayManager;

    private Camera _camera;
    private Transform _cameraTransform;

    private Transform _cameraTarget;

    private float _cameraDistance;
    private float _cameraMovementSpeed;

    private Vector3 _cameraForward, _cameraRight;

    private Vector3 _cameraOffset = new Vector3(0, 1, 0);

    public void Init()
    {
        _gameplayManager = GameClient.Get<IGameplayManager>();

        _gameplayManager.GameplayStartedEvent += GameplayStartedEventHandler;
    }

    public void ResetAll()
    {
    }

    public void Dispose()
    {
    }

    public void Update()
    {
        if (!_gameplayManager.IsGameplayStarted)
            return;

        if (_cameraTarget != null)
        {
            UpdateCameraPosition();
        }
    }

    public void SetCameraTarget(Transform cameraTarget)
    {
        _cameraTarget = cameraTarget;
    }

    public Vector3 GetMovementDirection(Vector2 direction)
    {
        Vector3 rightMove = _cameraRight * direction.x;
        Vector3 upMove = _cameraForward * direction.y;
        return rightMove + upMove;
    }

    private void GameplayStartedEventHandler()
    {
        _camera = _gameplayManager.HeroCamera;
        _cameraTransform = _camera.transform;

        var heroCameraConfig = _gameplayManager.GameplayData.heroCameraConfig;
        _cameraDistance = heroCameraConfig.distance;
        _cameraMovementSpeed = heroCameraConfig.movementSpeed;

        _cameraForward = _cameraTransform.forward;
        _cameraForward.y = 0;
        _cameraForward = Vector3.Normalize(_cameraForward);
        _cameraRight = Quaternion.Euler(new Vector3(0, 90, 0)) * _cameraForward;
    }

    private void UpdateCameraPosition()
    {
        var newPosition = _cameraTarget.position + _cameraOffset;
        newPosition -= _cameraTransform.forward * _cameraDistance;

        _cameraTransform.position =
            Vector3.Lerp(_cameraTransform.position,
                newPosition, Time.deltaTime * _cameraMovementSpeed);
    }
}