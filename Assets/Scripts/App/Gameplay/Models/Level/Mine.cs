using UnityEngine;
using Extensions;
using System;
using System.Collections.Generic;
using DG.Tweening;

namespace Models.Level
{
    public class Mine
    {
        public event Action OnHitHeroEvent;
        public event Action<List<GameObject>> OnHitEnemiesEvent;

        private const int FlashingCount = 10;

        private GameObject _selfObject;
        private Transform _selfTransform;

        private OnBehaviourHandler _onBehaviourHandler;
        private ParticleSystem _particleSystem;
        private Renderer _mainRenderer;
        private AudioSource _mainSound;

        private int _heroLayer;
        private int _enemyLayer;

        private GameplayData.MineConfig _mainConfig;

        private Timer _timer;

        private bool _isExploded;

        public Mine(GameObject gameObject, GameplayData.MineConfig config)
        {
            _selfObject = gameObject;
            _selfTransform = _selfObject.transform;

            _mainConfig = config;

            _mainRenderer = _selfObject.GetComponent<Renderer>();

            _onBehaviourHandler = _selfTransform.Find("DetectingCollider").GetComponent<OnBehaviourHandler>();
            _particleSystem = _selfTransform.Find("Explosion").GetComponent<ParticleSystem>();

            _mainSound = _particleSystem.transform.Find("MainSound").GetComponent<AudioSource>();

            _heroLayer = LayerMask.NameToLayer("Hero");
            _enemyLayer = LayerMask.NameToLayer("Enemy");

            _timer = new Timer();

            _onBehaviourHandler.TriggerEntered += TriggerEnteredHandler;
        }

        public void Update()
        {
            _timer?.Update();
        }

        public void SetActive(bool active)
        {
            _selfObject.SetActive(active);
        }

        private void Explode()
        {
            _particleSystem.Play();

            Collider[] colliders = Physics.OverlapSphere(_selfTransform.position, _mainConfig.eplosionRadius);

            _timer.Start(_particleSystem.main.duration, () =>
            {
                SetActive(false);
            });

            _mainSound.Play();

            ChechExlosionRadius(colliders);
        }

        private void ChechExlosionRadius(Collider[] hitedColliders)
        {
            var hitEnemies = new List<GameObject>();

            foreach (Collider collider in hitedColliders)
            {
                var objectLayer = collider.gameObject.layer;

                if (objectLayer.Equals(_heroLayer))
                {
                    OnHitHeroEvent?.Invoke();
                }
                else if (objectLayer.Equals(_enemyLayer))
                {
                    hitEnemies.Add(collider.gameObject);
                }
            }

            if (hitEnemies.Count > 0)
            {
                OnHitEnemiesEvent?.Invoke(hitEnemies);
            }
        }

        private void TriggerEnteredHandler(Collider collider)
        {
            if (!_isExploded && collider.gameObject.layer.Equals(_heroLayer))
            {
                _isExploded = true;
                _onBehaviourHandler.enabled = false;

                _mainRenderer.material.DOColor(Color.red, _particleSystem.main.duration / FlashingCount).SetLoops(FlashingCount, LoopType.Yoyo);
                _timer.Start(_mainConfig.timeBeforeExplosion, Explode);
            }
        }
    }
}