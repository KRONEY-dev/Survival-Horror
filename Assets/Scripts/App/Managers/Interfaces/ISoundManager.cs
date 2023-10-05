using static SoundManager;
using Models;

public interface ISoundManager
{
    void SetSound(string nameKey);
    void SetBackgroundMusicByPage<T>() where T : IPage;
    void SwitchSoundsByType(SoundType type, bool isMuted, bool isNeedSave = true);
    void SwitchBackgroundMusic(bool isActive, bool isNeedSave = true);
    void SwitchSpecialEffects(bool isActive, bool isNeedSave = true);
    SoundsSettings.SoundSetting GetSoundSettingByType(SoundType type);
    void SetSoundsSettings(SoundsSettings settings);
}