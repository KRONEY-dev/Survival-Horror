using System;

public class MatchController : IController
{
    public event Action OnMatchFinishedEvent;

    private IUIManager _uiManager;
    private IDataManager _dataManager;

    private HeroController _heroController;

    public void Init()
    {
        _uiManager = GameClient.Get<IUIManager>();
        _dataManager = GameClient.Get<IDataManager>();

        _heroController = GameClient.Get<IGameplayManager>().GetController<HeroController>();
    }

    public void ResetAll()
    {
    }

    public void Dispose()
    {
    }

    public void Update()
    {
    }

    public void SetEndState()
    {
        OnMatchFinishedEvent?.Invoke();

        TimeSpan currentSurvivalTime = TimeSpan.Zero;//Need real data
        bool isNewRecord = false;

        if (currentSurvivalTime > _dataManager.CachedUserLocalData.maxSurvivalTime)
        {
            _dataManager.CachedUserLocalData.maxSurvivalTime = currentSurvivalTime;
            isNewRecord = true;
        }

        _uiManager.SetPage<GameEndPage>(currentSurvivalTime, isNewRecord);
    }
}