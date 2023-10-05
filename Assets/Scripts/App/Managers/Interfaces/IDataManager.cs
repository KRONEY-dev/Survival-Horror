using System;
using Models;

public interface IDataManager
{
    event Action DataLoadedEvent;
    CachedUserData CachedUserLocalData { get; set; }
    void SaveAllData();
    void SaveData(DataManager.GameDataType gameDataType);
}
