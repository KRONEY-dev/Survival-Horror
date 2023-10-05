using Unity.VisualScripting;
using UnityEngine;

public class GameClient : DIBase
{
    private static object _sync = new object();

    public static Transform ParentForPrefabedManagers;

    private static GameClient _Instance;
    public static GameClient Instance
    {
        get
        {
            if (_Instance == null)
            {
                lock (_sync)
                {
                    _Instance = new GameClient();
                }
            }
            return _Instance;
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GameClient"/> class.
    /// </summary>
    internal GameClient() : base()
    {
        GameObject soundManagerPrefab = Resources.Load("Prefabs/Managers/SoundManager") as GameObject;
        AddService<ISoundManager>(Object.Instantiate(soundManagerPrefab, ParentForPrefabedManagers).GetComponent<SoundManager>());

        AddService<IInputManager>(new InputManager());
        AddService<IDataManager>(new DataManager());
        AddService<IUIManager>(new UIManager());
        AddService<IAppStateManager>(new AppStateManager());
        AddService<IGameplayManager>(new GameplayManager());
    }

    public static T Get<T>()
    {
        return Instance.GetService<T>();
    }
}
