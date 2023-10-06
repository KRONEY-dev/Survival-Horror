using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameplayData", menuName = "AltaGamesTestTask/GameplayData", order = 1)]
public class GameplayData : ScriptableObject
{
    [SerializeField]
    public MainGameplayConfig mainGameplayConfig;

    [SerializeField]
    public LevelInfo[] levels;

    [SerializeField]
    public HeroConfig heroConfig;

    [SerializeField]
    public HeroCameraConfig heroCameraConfig;

    [SerializeField]
    public EnemyConfig enemyConfig;

    [SerializeField]
    public MineConfig mineConfig;


    [Serializable]
    public class MainGameplayConfig
    {
        public float enemiesPerMinute;

        public float safeAreaOffsetOnLevelFloor;

        public float matchEndDelayDuration;
    }

    [Serializable]
    public class LevelInfo
    {
        public int number;
        public GameObject prefab;
    }

    [Serializable]
    public class HeroConfig
    {
        public float maxSpeed;
        public int health;

        public float defenseDurationAfterHit;
        public float deathAnimationDuration;
    }

    [Serializable]
    public class HeroCameraConfig
    {
        public float distance;
        public float movementSpeed;
    }

    [Serializable]
    public class EnemyConfig
    {
        public float speed;

        public int heroHealthHit;
    }

    [Serializable]
    public class MineConfig
    {
        public float timeBeforeExplosion;
        public float eplosionRadius;

        public int heroHealthHit;
    }
}