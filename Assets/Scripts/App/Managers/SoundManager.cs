using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using Models;

public class SoundManager : MonoBehaviour, ISoundManager, IService
{
    public enum SoundType
    {
        BackgroundMusic,
        SpecialEffects
    }

    private const float BackgroundMusicFadeOutDuration = 1.0f;
    private const float BackgroundMusicFadeInDuration = 1.0f;

    [SerializeField]
    private AudioSource backgroundMusicAudioSource;

    [SerializeField]
    private List<Sound> sounds;

    private Dictionary<Type, string> _pagesBackgroundMusic;

    private Sound _currentBackgroundMusic;

    private SoundsSettings _soundsSettings;

    private IDataManager _dataManager;

    public void Init()
    {
        _dataManager = GameClient.Get<IDataManager>();

        _pagesBackgroundMusic = new Dictionary<Type, string>()
        {
            { typeof(HomePage), SoundsNames.HomeBackground },
            { typeof(GamePage), SoundsNames.GameBackground },
        };

        foreach (Sound sound in sounds)
        {
            switch (sound.Type)
            {
                case SoundType.SpecialEffects:
                    sound.Source = SetupAudioSourceForSound(gameObject.AddComponent<AudioSource>(), sound);
                    break;
                case SoundType.BackgroundMusic:
                    sound.Source = backgroundMusicAudioSource;
                    break;
            }
        }
    }

    public void Update()
    {
    }

    public void Dispose()
    {
    }

    public void SetSound(string nameKey)
    {
        sounds.Find(it => it.Name.Equals(nameKey))?.Source.Play();
    }

    public void SetBackgroundMusicByPage<T>() where T : IPage
    {
        var lastBackgroundMusicFadeTween = backgroundMusicAudioSource.DOFade(0, BackgroundMusicFadeOutDuration).SetEase(Ease.Linear);

        if (_pagesBackgroundMusic.TryGetValue(typeof(T), out string musicName))
        {
            Sound backgroundMusic = sounds.Find(it => it.Name.Equals(musicName));
            if (backgroundMusic != null)
            {
                _currentBackgroundMusic = backgroundMusic;

                if (backgroundMusicAudioSource.clip != null)
                {
                    lastBackgroundMusicFadeTween.onComplete += SetCurrentBackgroundMusic;
                }
                else
                {
                    SetCurrentBackgroundMusic();
                }
            }
        }
        else
        {
            _currentBackgroundMusic = null;
        }
    }

    public void SwitchSoundsByType(SoundType type, bool isMuted, bool isNeedSave = true)
    {
        switch (type)
        {
            case SoundType.SpecialEffects:
                SwitchSpecialEffects(isMuted, isNeedSave);
                break;
            case SoundType.BackgroundMusic:
                SwitchBackgroundMusic(isMuted, isNeedSave);
                break;
        }
    }

    public void SwitchBackgroundMusic(bool isMuted, bool isNeedSave = true)
    {
        backgroundMusicAudioSource.mute = isMuted;

        if (isNeedSave)
        {
            _soundsSettings.SettingsData[SoundType.BackgroundMusic].IsMuted = isMuted;

            _dataManager.CachedUserLocalData.soundsSettings = _soundsSettings;
        }
    }

    public void SwitchSpecialEffects(bool isMuted, bool isNeedSave = true)
    {
        foreach (Sound sound in sounds)
        {
            if (sound.Type == SoundType.SpecialEffects)
            {
                sound.Source.mute = isMuted;
            }
        }

        if (isNeedSave)
        {
            _soundsSettings.SettingsData[SoundType.SpecialEffects].IsMuted = isMuted;

            _dataManager.CachedUserLocalData.soundsSettings = _soundsSettings;
        }
    }

    public SoundsSettings.SoundSetting GetSoundSettingByType(SoundType type)
    {
        if(_soundsSettings.SettingsData.TryGetValue(type, out SoundsSettings.SoundSetting setting))
        {
            return setting;
        }
        else
        {
            return null;
        }
    }

    public void SetSoundsSettings(SoundsSettings settings)
    {
        _soundsSettings = settings;

        SoundsSettings.SoundSetting data = null;
        foreach (var soundSetting in _soundsSettings.SettingsData)
        {
            data = soundSetting.Value;
            switch (soundSetting.Key)
            {
                case SoundType.BackgroundMusic:
                    SwitchBackgroundMusic(data.IsMuted, false);
                    break;
                case SoundType.SpecialEffects:
                    SwitchSpecialEffects(data.IsMuted, false);
                    break;
            }
        }
    }

    private void SetCurrentBackgroundMusic()
    {
        backgroundMusicAudioSource.volume = 0;
        SetupAudioSourceForSound(backgroundMusicAudioSource, _currentBackgroundMusic, false);

        backgroundMusicAudioSource.Play();
        backgroundMusicAudioSource.DOFade(_currentBackgroundMusic.Volume, BackgroundMusicFadeInDuration).SetEase(Ease.Linear);
    }

    private AudioSource SetupAudioSourceForSound(AudioSource source, Sound sound, bool isNeedSetVolume = true)
    {
        source.clip = sound.Clip;
        source.pitch = sound.Pitch;
        source.loop = sound.Type == SoundType.BackgroundMusic;
        source.clip.LoadAudioData();

        if (isNeedSetVolume)
        {
            source.volume = sound.Volume;
        }

        return source;
    }

    public static class SoundsNames
    {
        public const string ButtonClick = "ButtonClick";
        public const string GameEndSound = "GameEndSound";

        public const string GameBackground = "GameBackground";
        public const string HomeBackground = "HomeBackground";
    }

    [Serializable]
    public class Sound
    {
        public string Name;

        public AudioClip Clip;

        public SoundType Type;

        [Range(0f, 1f)]
        public float Volume = 1f;

        [Range(0.1f, 3f)]
        public float Pitch = 1f;

        [HideInInspector]
        public AudioSource Source;
    }
}