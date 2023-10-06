using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Pages.Components.GameEndPage
{
    public class GameEndInfoPanel
    {
        public event Action HomeButtonOnClickEvent;
        public event Action RepeatButtonOnClickEvent;

        private GameObject _selfObject;
        private Transform _selfTransform;

        private TextMeshProUGUI _titleText;
        private TextMeshProUGUI _ribonLevelText;

        private Button _homeButton;
        private Button _repeatButton;

        public GameEndInfoPanel(GameObject gameObject)
        {
            _selfObject = gameObject;
            _selfTransform = _selfObject.transform;

            _titleText = _selfTransform.Find("Text_Title").GetComponent<TextMeshProUGUI>();
            _ribonLevelText = _selfTransform.Find("Panel_RibonCurrentLevel/Text_Main").GetComponent<TextMeshProUGUI>();

            _homeButton = _selfTransform.Find("Panel_Navigation/Button_Home").GetComponent<Button>();
            _repeatButton = _selfTransform.Find("Panel_Navigation/Button_Repeat").GetComponent<Button>();

            _homeButton.onClick.AddListener(HomeButtonOnClickHandler);
            _repeatButton.onClick.AddListener(RepeatButtonOnClickHandler);
        }

        public void SetRibonSurvivalTime(float survivalTime)
        {
            _ribonLevelText.text = $"COMPLETED LEVEL: <color=red>{survivalTime}</color>";
        }

        private void HomeButtonOnClickHandler()
        {
            HomeButtonOnClickEvent?.Invoke();
        }

        private void RepeatButtonOnClickHandler()
        {
            RepeatButtonOnClickEvent?.Invoke();
        }
    }
}