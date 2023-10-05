using System;
using UnityEngine;
using UnityEngine.UI;
using static SoundManager;

public class SoundSettingsButton
{
    public event Action<SoundType, bool> OnSettingsChangeEvent;

    private readonly Color OnColor = Color.white;
    private readonly Color OffColor = Color.black;

    public SoundType SoundType { get; }
    public bool IsMuted { get; private set; }

    private Button _mainButton;

    private Image _mainImage;

    public SoundSettingsButton(Transform selftransform, SoundType soundType, bool isMuted)
    {
        SoundType = soundType;
        IsMuted = isMuted;

        _mainButton = selftransform.GetComponent<Button>();
        _mainImage = selftransform.Find("Image_Main").GetComponent<Image>();

        _mainButton.onClick.AddListener(MainButtonOnClickHandler);

        UpdateVisualizationByCurrentState();
    }

    private void UpdateVisualizationByCurrentState()
    {
        _mainImage.color = IsMuted ? OffColor : OnColor;
    }

    private void MainButtonOnClickHandler()
    {
        IsMuted = !IsMuted;

        UpdateVisualizationByCurrentState();

        OnSettingsChangeEvent?.Invoke(SoundType, IsMuted);
    }
}