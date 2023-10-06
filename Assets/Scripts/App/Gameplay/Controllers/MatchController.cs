using Extensions;
using System;

public class MatchController : IController
{
    public event Action OnMatchFinishedEvent;

    public bool IsMatchActive;

    private IUIManager _uiManager;
    private IDataManager _dataManager;

    private Timer _matchEndDelayTimer;
    private float _matchEndDelayDuration;

    private DateTime _matchStartDate;

    public void Init()
    {
        var gameplayManager = GameClient.Get<IGameplayManager>();
        _uiManager = GameClient.Get<IUIManager>();
        _dataManager = GameClient.Get<IDataManager>();

        _matchEndDelayTimer = new Timer();
        _matchEndDelayDuration = gameplayManager.GameplayData.mainGameplayConfig.matchEndDelayDuration;

        gameplayManager.GetController<LevelController>().OnLevelLoadedEvent += OnLevelLoadedEventHandler;
    }

    public void ResetAll()
    {
    }

    public void Dispose()
    {
    }

    public void Update()
    {
        _matchEndDelayTimer?.Update();
    }

    public void SetEndState()
    {
        OnMatchFinishedEvent?.Invoke();

        IsMatchActive = false;

        TimeSpan currentSurvivalTime = DateTime.Now - _matchStartDate;
        bool isNewRecord = false;

        if (currentSurvivalTime > _dataManager.CachedUserLocalData.maxSurvivalTime)
        {
            _dataManager.CachedUserLocalData.maxSurvivalTime = currentSurvivalTime;
            isNewRecord = true;
        }

        _matchEndDelayTimer.Start(_matchEndDelayDuration, () =>
        {
            _uiManager.SetPage<GameEndPage>(new object[] { currentSurvivalTime, isNewRecord });
        });
    }

    private void OnLevelLoadedEventHandler()
    {
        _matchStartDate = DateTime.Now;

        IsMatchActive = true;
    }
}