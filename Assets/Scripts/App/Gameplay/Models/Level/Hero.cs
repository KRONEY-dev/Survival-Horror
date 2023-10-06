using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Models.Level
{
    public class Hero
    {
        public event Action<int> OnHitEvent;

        public int Health { get; private set; }

        public Transform SelfTransform { get; private set; }

        private GameObject _selfObject;

        private CharacterController _mainCharacterController;

        private Renderer _mainRenderer;

        private IInputManager _inputManager;
        private ISoundManager _soundManager;

        private CameraController _cameraController;

        private Defense _defense;

        private int _inputMoveIndex;

        private float _maxSpeed;

        public Hero(GameObject gameObject, GameplayData.HeroConfig config)
        {
            _selfObject = gameObject;
            SelfTransform = _selfObject.transform;

            var startPosition = SelfTransform.localPosition;
            startPosition.y = SelfTransform.localScale.y;
            SelfTransform.localPosition = startPosition;

            _mainCharacterController = _selfObject.GetComponent<CharacterController>();
            _mainRenderer = _selfObject.GetComponent<Renderer>();

            Health = config.health;
            _maxSpeed = config.maxSpeed;

            _inputManager = GameClient.Get<IInputManager>();
            _soundManager = GameClient.Get<ISoundManager>();
            _cameraController = GameClient.Get<IGameplayManager>().GetController<CameraController>();

            _defense = new Defense(config.defenseDurationAfterHit, _mainRenderer);

            _inputMoveIndex = _inputManager.RegisterInputHandler(InputManager.InputType.Joystick, 0, onInputEndParametrized: OnInputJoystickHandler);
        }

        public void StopInput()
        {
            _inputManager.UnregisterInputHandler(_inputMoveIndex);
        }

        public void Destroy()
        {
            Object.Destroy(_selfObject);

            _inputManager.UnregisterInputHandler(_inputMoveIndex);
        }

        public void Update()
        {
            _defense?.Update();
        }

        public void Hit(int healthHit)
        {
            if (_defense.IsActive) return;

            _soundManager.SetSound(SoundManager.SoundsNames.DamageSound);

            Health = Mathf.Clamp(Health - healthHit, 0, short.MaxValue);

            OnHitEvent?.Invoke(healthHit);

            if (Health == 0)
            {
                _soundManager.SetSound(SoundManager.SoundsNames.DeathSound);
                return;
            }

            _defense.TurnOn();
        }

        private void OnInputJoystickHandler(object MoveDirection)
        {
            object[] param = (object[])MoveDirection;

            Move((float)param[0], (float)param[1], (bool)param[2]);
        }

        private void Move(float horizontal, float vertical, bool isChangeForward)
        {
            Vector3 joysticDirection = new Vector2(horizontal, vertical);

            Vector3 moveDirection = _cameraController.GetMovementDirection(joysticDirection);

            _mainCharacterController.SimpleMove(moveDirection * _maxSpeed);
        }
    }
}