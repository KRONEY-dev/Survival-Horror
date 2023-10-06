using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Models.Level
{
    public class Level
    {
        public event Action OnMineHitHeroEvent;
        public event Action<List<GameObject>> OnMineHitEnemiesEvent;

        public Transform HeroSpawnPointTransform { get; private set; }
        public Transform EnemiesParentTransform { get; private set; }

        private GameObject _selfObject;
        private Transform _selfTransform;

        private List<Mine> _mines;

        private float _floorSizeX;
        private float _floorSizeZ;

        private float _safeAreaOffsetOnFloor;

        public Level(GameplayData.LevelInfo levelInfo, GameplayData gameplayData, Transform parent)
        {
            _safeAreaOffsetOnFloor = gameplayData.mainGameplayConfig.safeAreaOffsetOnLevelFloor;

            _selfObject = Object.Instantiate(levelInfo.prefab, parent);
            _selfTransform = _selfObject.transform;

            HeroSpawnPointTransform = _selfTransform.Find("HeroSpawnPoint");
            EnemiesParentTransform = _selfTransform.Find("Enemies");

            var floorScale = _selfTransform.Find("Ground/Floor").lossyScale;
            _floorSizeX = floorScale.x / 2;
            _floorSizeZ = floorScale.z / 2;

            _mines = new List<Mine>();
            RegisterAllMines(gameplayData.mineConfig);
        }

        public void Update()
        {
            foreach (var mine in _mines)
            {
                mine.Update();
            }
        }

        public void Dispose()
        {
            Object.Destroy(_selfObject);
        }

        public Vector3 GetRandomSafePosition()
        {
            /*            var floorSafeSizeX = _floorSizeX - _safeAreaOffsetOnFloor;
                        var floorSafeSizeZ = _floorSizeZ - _safeAreaOffsetOnFloor;

                        Vector3 randomPoint = new Vector3(Random.Range(-_floorSizeX / 2 + _safeAreaOffsetOnFloor, _floorSizeX / 2 - _safeAreaOffsetOnFloor), 0.0f,
                            Random.Range(-_floorSizeZ / 2 + _safeAreaOffsetOnFloor, _floorSizeZ / 2 - _safeAreaOffsetOnFloor));

                        if (randomPoint.x < _safeAreaOffsetOnFloor || randomPoint.x > floorSafeSizeX)
                        {
                            randomPoint.x = Random.Range(_safeAreaOffsetOnFloor, floorSafeSizeX);
                        }

                        if (randomPoint.z < _safeAreaOffsetOnFloor || randomPoint.z > floorSafeSizeZ)
                        {
                            randomPoint.z = Random.Range(_safeAreaOffsetOnFloor, floorSafeSizeZ);
                        }

                        return randomPoint;*/

            var floorSafeSizeX = _floorSizeX - _safeAreaOffsetOnFloor;
            var floorSafeSizeZ = _floorSizeZ - _safeAreaOffsetOnFloor;
            Vector3 randomPoint = Vector3.zero;

            randomPoint.x = Random.Range(-floorSafeSizeX, floorSafeSizeX);
            randomPoint.z = Random.Range(-floorSafeSizeZ, floorSafeSizeZ);

            return randomPoint;
        }

        private void RegisterAllMines(GameplayData.MineConfig mineConfig)
        {
            var minesParent = _selfTransform.Find("[Mines]");

            Mine mine = null;
            for (int i = 0; i < minesParent.childCount; i++)
            {
                mine = new Mine(minesParent.GetChild(i).gameObject, mineConfig);
                mine.OnHitHeroEvent += OnMineHitHeroEventHandler;
                mine.OnHitEnemiesEvent += OnMineHitEnemiesEventHandler;

                _mines.Add(mine);
            }
        }

        private void OnMineHitHeroEventHandler()
        {
            OnMineHitHeroEvent?.Invoke();
        }

        private void OnMineHitEnemiesEventHandler(List<GameObject> enemies)
        {
            OnMineHitEnemiesEvent?.Invoke(enemies);
        }
    }
}