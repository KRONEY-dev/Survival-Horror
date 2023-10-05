using System.Collections.Generic;
using UnityEngine;

namespace Models.Level
{
    public class Level
    {
        public Camera LevelCamera { get; private set; }

        public Hero Hero { get; private set; }

        private GameObject _selfObject;
        private Transform _selfTransform;

        public Level(GameplayData.LevelInfo levelInfo, GameplayData gameplayData, Transform parent)
        {
            _selfObject = Object.Instantiate(levelInfo.prefab, parent);
            _selfTransform = _selfObject.transform;

            LevelCamera = _selfTransform.Find("LevelCamera").GetComponent<Camera>();
        }

        public void Update()
        {
        }

        public void Dispose()
        {
            Object.Destroy(_selfObject);
        }

        public void RegisteHero(GameObject heroObject)
        {
            Hero = new Hero(heroObject);
        }
    }
}