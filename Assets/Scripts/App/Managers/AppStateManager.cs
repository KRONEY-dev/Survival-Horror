public class AppStateManager : IAppStateManager, IService
{
    public enum AppState
    {
        Unknown,

        Home,
        Game
    }

    public AppState CurrentAppState { get; private set; } = AppState.Unknown;

    private IUIManager _uiManager;

    public void Init()
    {
        _uiManager = GameClient.Get<IUIManager>();
    }

    public void Update()
    {
    }
    public void Dispose()
    {
    }

    public void ChangeAppState(AppState stateTo)
    {
        if (CurrentAppState == stateTo)
            return;

        CurrentAppState = stateTo;

        switch (stateTo)
        {
            case AppState.Home:
                _uiManager.SetPage<HomePage>();
                GameClient.Get<IGameplayManager>().StopGameplay();
                break;
            case AppState.Game:
                _uiManager.SetPage<GamePage>();
                GameClient.Get<IGameplayManager>().StartGameplay();
                break;
        }
    }
}
