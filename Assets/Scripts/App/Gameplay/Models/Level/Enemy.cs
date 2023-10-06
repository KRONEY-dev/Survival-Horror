using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Models.Level
{
    public class Enemy
    {
        public event Action OnHitHeroEvent;

        private GameObject _selfObject;
        private Transform _selfTransform;

        private CharacterController _mainCharacterController;
        private OnBehaviourHandler _onBehaviourHandler;

        private Transform _target;

        private float _speed;

        private int _heroLayer;

        public Enemy(GameObject gameObject, Transform target, GameplayData.EnemyConfig config)
        {
            _selfObject = gameObject;
            _selfTransform = _selfObject.transform;

            var startPosition = _selfTransform.localPosition;
            startPosition.y = _selfTransform.localScale.y / 2;
            _selfTransform.localPosition = startPosition;

            _mainCharacterController = _selfObject.GetComponent<CharacterController>();
            _onBehaviourHandler = _selfObject.GetComponent<OnBehaviourHandler>();

            _speed = config.speed;
            _target = target;

            _heroLayer = LayerMask.NameToLayer("Hero");

            _onBehaviourHandler.ControllerColliderHit += ControllerColliderHitHandler;
        }

        public void Update()
        {
            Move();
        }

        public void Destroy()
        {
            Object.Destroy(_selfObject);
        }

        public bool IsTheSameGameObject(GameObject gameObject)
        {
            return _selfObject == gameObject;
        }

        private void Move()
        {
            if (_target != null)
            {
                _selfTransform.LookAt(_target, Vector3.forward);
                _selfTransform.eulerAngles = new Vector3(0, _selfTransform.eulerAngles.y, 0);

                _mainCharacterController.SimpleMove(_selfTransform.forward * _speed);
            }
        }

        private void ControllerColliderHitHandler(ControllerColliderHit hit)
        {
            if (hit.gameObject.layer.Equals(_heroLayer))
            {
                OnHitHeroEvent?.Invoke();
            }
        }
    }
}