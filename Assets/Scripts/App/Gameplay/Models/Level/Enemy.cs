using UnityEngine;
using Object = UnityEngine.Object;

namespace Models.Level
{
    public class Enemy
    {
        private GameObject _selfObject;
        private Transform _selfTransform;

        private CharacterController _mainCharacterController;

        private Transform _target;

        private float _speed;

        public Enemy(GameObject gameObject, Transform target, GameplayData.EnemyConfig config)
        {
            _selfObject = gameObject;
            _selfTransform = _selfObject.transform;

            _mainCharacterController = _selfObject.GetComponent<CharacterController>();

            _speed = config.speed;

            _target = target;
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
    }
}