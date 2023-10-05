using static SoundManager;

public class GameEndPage : BasePage
{
    private IGameplayManager _gameplayManager;
    private IAppStateManager _appStateManager;
    private IDataManager _dataManager;

    private GameEndInfoPanel _gameEndInfoPanel;

    protected override string PrefabName => "GameEndPage";

    public override void Init()
    {
        _gameplayManager = GameClient.Instance.GetService<IGameplayManager>();
        _appStateManager = GameClient.Instance.GetService<IAppStateManager>();
        _dataManager = GameClient.Instance.GetService<IDataManager>();

        _gameEndInfoPanel = new GameEndInfoPanel(SelfTransform.Find("Panel_GameEndInfo").gameObject);

        _gameEndInfoPanel.HomeButtonOnClickEvent += HomeButtonOnClickEventHandler;
        _gameEndInfoPanel.RepeatButtonOnClickEvent += RepeatButtonOnClickEventHandler;
    }

    private void HomeButtonOnClickEventHandler()
    {
        SoundManager.SetSound(SoundsNames.ButtonClick);

        _appStateManager.ChangeAppState(AppStateManager.AppState.Home);
    }

    private void RepeatButtonOnClickEventHandler()
    {
        SoundManager.SetSound(SoundsNames.ButtonClick);
        UiManager.SetPage<GamePage>();

        _gameplayManager.RestartGameplay();
    }

    public override void Show(object data)
    {
        base.Show(data);

        float survivalTime = (float)data;

        SoundManager.SetSound(SoundsNames.GameEndSound);

        _gameEndInfoPanel.SetRibonSurvivalTime(survivalTime);
    }
}