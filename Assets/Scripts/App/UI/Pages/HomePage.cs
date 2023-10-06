using Models;
using TMPro;
using UI.Pages.Components.HomePage;
using UnityEngine;
using UnityEngine.UI;
using static SoundManager;

public class HomePage : BasePage
{
    protected override string PrefabName => "HomePage";

    private SoundSettingsButton[] _soundSettingsButtons;

    private Button _playButton;
    private TextMeshProUGUI _currentMaxSurvivalTimeText;

    private ISoundManager _soundManager;

    public override void Init()
    {
        base.Init();

        _soundManager = GameClient.Get<ISoundManager>();

        Transform soundSettingsPanel = SelfTransform.Find("Content/Panel_Settings");
        _soundSettingsButtons = new SoundSettingsButton[]
        {
            GetSoundSettingsButtonByType(SoundType.SpecialEffects, "Item_SpecialEffects", soundSettingsPanel),
            GetSoundSettingsButtonByType(SoundType.BackgroundMusic, "Item_BackgroundMusic", soundSettingsPanel)
        };

        _currentMaxSurvivalTimeText = SelfTransform.Find("Content/Panel_MaxSurvivalTime/Text_LevelValue").GetComponent<TextMeshProUGUI>();
        _playButton = SelfTransform.Find("Content/Button_GameStart").GetComponent<Button>();

        _playButton.onClick.AddListener(PlayButtonOnClickHandler);
    }

    public override void Show()
    {
        base.Show();

        SetCurrentLevelText();
    }

    private void SetCurrentLevelText()
    {
        var maxSurvivalTime = GameClient.Get<IDataManager>().CachedUserLocalData.maxSurvivalTime;

        _currentMaxSurvivalTimeText.text = $"{maxSurvivalTime.TotalSeconds}s";
    }

    private SoundSettingsButton GetSoundSettingsButtonByType(SoundType type, string pathAtPanel, Transform soundSettingsPanel)
    {
        SoundsSettings.SoundSetting soundSetting = _soundManager.GetSoundSettingByType(type);
        SoundSettingsButton soundSettingsButton = new SoundSettingsButton(soundSettingsPanel.Find(pathAtPanel), type, soundSetting != null && soundSetting.IsMuted);

        soundSettingsButton.OnSettingsChangeEvent += OnSettingsChangeEventHandler;
        return soundSettingsButton;
    }

    private void OnSettingsChangeEventHandler(SoundType type, bool isMuted)
    {
        SoundManager.SetSound(SoundsNames.ButtonClick);

        _soundManager.SwitchSoundsByType(type, isMuted);
    }

    private void PlayButtonOnClickHandler()
    {
        SoundManager.SetSound(SoundsNames.ButtonClick);

        GameClient.Get<IAppStateManager>().ChangeAppState(AppStateManager.AppState.Game);
    }
}