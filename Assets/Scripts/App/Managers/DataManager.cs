using Extentions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Models;

public class DataManager : IService, IDataManager
{
    public enum GameDataType
    {
        UserData
    }

    public event Action DataLoadedEvent;

    public static readonly string BaseDataPath = $"{Application.persistentDataPath}/CacheData/";

    private const string StaticDataPrivateKey = "LYxxzb9PHSfas8215125asgddh6012GAsgajgASHHA12525zb7PWSRMpHMtWXAYUCun";

    public CachedUserData CachedUserLocalData { get; set; }

    public readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings()
    {
        Converters = { new StringEnumConverter(), }
    };

    private Dictionary<GameDataType, string> _gameDataPathes;

    public void Init()
    {
        _gameDataPathes = new Dictionary<GameDataType, string>()
        {
            { GameDataType.UserData, $"{BaseDataPath}userData.dat" },
        };

        if (!Directory.Exists(BaseDataPath))
        {
            Directory.CreateDirectory(BaseDataPath);
        }

        LoadAllData();
    }

    public void Update()
    {
    }

    public void Dispose()
    {
        SaveAllData();
    }

    public void SaveAllData()
    {
        foreach (GameDataType key in _gameDataPathes.Keys)
        {
            SaveData(key);
        }
    }

    public void LoadAllData()
    {
        foreach (GameDataType key in _gameDataPathes.Keys)
        {
            LoadData(key);
        }

        DataLoadedEvent?.Invoke();
    }

    public void SaveData(GameDataType gameDataType)
    {
        string data = string.Empty;
        string dataPath = _gameDataPathes[gameDataType];

        switch (gameDataType)
        {
            case GameDataType.UserData:
                data = Serialize(CachedUserLocalData);
                break;

            default: break;
        }

        if (data.Length > 0)
        {
            if (!File.Exists(dataPath))
            {
                File.Create(dataPath).Close();
            }

            File.WriteAllText(dataPath, data);
        }
    }

    public void LoadData(GameDataType gameDataType)
    {
        string dataPath = _gameDataPathes[gameDataType];

        switch (gameDataType)
        {
            case GameDataType.UserData:
                CachedUserLocalData = DeserializeFromPath<CachedUserData>(dataPath, gameDataType);
                if (CachedUserLocalData == null)
                {
                    CachedUserLocalData = new CachedUserData();
                    CachedUserLocalData.maxSurvivalTime = TimeSpan.Zero;

                    CachedUserLocalData.soundsSettings = new SoundsSettings();
                    foreach (SoundManager.SoundType sound in Enum.GetValues(typeof(SoundManager.SoundType)))
                    {
                        CachedUserLocalData.soundsSettings.SettingsData.Add(sound, new SoundsSettings.SoundSetting());
                    }

                    SaveData(GameDataType.UserData);
                }

                GameClient.Get<ISoundManager>().SetSoundsSettings(CachedUserLocalData.soundsSettings);
                break;

            default: break;
        }
    }

    private T DeserializeFromPath<T>(string path, GameDataType type) where T : class
    {
        if (!File.Exists(path))
            return null;

        return JsonConvert.DeserializeObject<T>(Decrypt(File.ReadAllText(path)), JsonSerializerSettings);
    }

    private string Serialize(object @object, Formatting formatting = Formatting.Indented)
    {
        return Encrypt(JsonConvert.SerializeObject(@object, formatting));
    }

    private string Decrypt(string data)
    {
        return GeneralExtentions.Decrypt(data, StaticDataPrivateKey);
    }

    private string Encrypt(string data)
    {
        return GeneralExtentions.Encrypt(data, StaticDataPrivateKey);
    }
}