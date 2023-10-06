using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

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

        public Level(GameplayData.LevelInfo levelInfo, GameplayData gameplayData, Transform parent)
        {
            _selfObject = Object.Instantiate(levelInfo.prefab, parent);
            _selfTransform = _selfObject.transform;

            HeroSpawnPointTransform = _selfTransform.Find("HeroSpawnPoint");
            EnemiesParentTransform = _selfTransform.Find("Enemies");

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