using UnityEngine.UI;

public class GamePage : BasePage
{
    protected override string PrefabName => "GamePage";

    private Button _homeButton;
    private Button _restartButton;

    private IAppStateManager _appStateManager;
    private IGameplayManager _gameplayManager;

    public override void Init()
    {
        base.Init();

        _appStateManager = GameClient.Instance.GetService<IAppStateManager>();
        _gameplayManager = GameClient.Instance.GetService<IGameplayManager>();

        _homeButton = SelfTransform.Find("Panel_Navigation/Button_Home").GetComponent<Button>();
        _restartButton = SelfTransform.Find("Panel_Navigation/Button_Restart").GetComponent<Button>();

        _homeButton.onClick.AddListener(HomeButtonOnClickHandler);
        _restartButton.onClick.AddListener(RestartButtonOnClickHandler);
    }

    private void HomeButtonOnClickHandler()
    {
        SoundManager.SetSound(global::SoundManager.SoundsNames.ButtonClick);
        _appStateManager.ChangeAppState(AppStateManager.AppState.Home);
    }

    private void RestartButtonOnClickHandler()
    {
        SoundManager.SetSound(global::SoundManager.SoundsNames.ButtonClick);
        _gameplayManager.RestartGameplay();
    }
}