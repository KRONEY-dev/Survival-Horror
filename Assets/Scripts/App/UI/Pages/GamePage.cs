using UI.Pages.Components.GamePage;
using UnityEngine;
using UnityEngine.UI;

public class GamePage : BasePage
{
    protected override string PrefabName => "GamePage";

    private Transform _controllersParent;

    private Button _homeButton;
    private Button _restartButton;

    private HealthPanel _healthPanel;

    private IAppStateManager _appStateManager;
    private IGameplayManager _gameplayManager;

    private HeroController _heroController;

    public override void Init()
    {
        base.Init();

        _appStateManager = GameClient.Get<IAppStateManager>();
        _gameplayManager = GameClient.Get<IGameplayManager>();

        _controllersParent = SelfTransform.Find("Controllers");

        _healthPanel = new HealthPanel(SelfTransform.Find("Panel_Health").gameObject);

        _heroController = _gameplayManager.GetController<HeroController>();

        _homeButton = SelfTransform.Find("Panel_Navigation/Button_Home").GetComponent<Button>();
        _restartButton = SelfTransform.Find("Panel_Navigation/Button_Restart").GetComponent<Button>();

        _heroController.OnHeroLoadedEvent += OnHeroLoadedEventHandler;
        _heroController.OnHeroHitEvent += OnHeroHitEventHandler;

        _homeButton.onClick.AddListener(HomeButtonOnClickHandler);
        _restartButton.onClick.AddListener(RestartButtonOnClickHandler);
    }

    public Transform GetControllsParent()
    {
        return _controllersParent;
    }

    private void OnHeroLoadedEventHandler()
    {
        _healthPanel.SetHearts(_heroController.GetHeroHealth());
    }

    private void OnHeroHitEventHandler(int hit)
    {
        _healthPanel.RemoveHearts(hit);
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