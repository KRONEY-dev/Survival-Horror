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


    [Serializable]
    public class MainGameplayConfig
    {

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

    }
}