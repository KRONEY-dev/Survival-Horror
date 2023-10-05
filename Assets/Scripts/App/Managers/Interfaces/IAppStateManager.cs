using static AppStateManager;

public interface IAppStateManager
{
    AppState CurrentAppState { get; }
    void ChangeAppState(AppStateManager.AppState stateTo);
}
