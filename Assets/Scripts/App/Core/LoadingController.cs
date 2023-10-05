using System;

public class LoadingController
{
    private static IUIManager _uiManager;

    public LoadingController()
    {
        _uiManager = GameClient.Instance.GetService<IUIManager>();
    }

    public void StartGame()
    {
        _uiManager.SetPage<IntroPage>(new Action(OnIntroCompleteHandler));
    }

    public void OnIntroCompleteHandler()
    {
        _uiManager.SetPage<HomePage>();
    }
}