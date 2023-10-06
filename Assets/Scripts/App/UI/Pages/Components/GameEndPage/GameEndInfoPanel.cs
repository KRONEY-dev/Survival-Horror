using Extentions;
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
        private TextMeshProUGUI _ribonSurvivalTimeText;

        private Button _homeButton;
        private Button _repeatButton;

        private string _defaultTitleText;
        private string _newRecordTitleText;

        public GameEndInfoPanel(GameObject gameObject)
        {
            _selfObject = gameObject;
            _selfTransform = _selfObject.transform;

            _titleText = _selfTransform.Find("Text_Title").GetComponent<TextMeshProUGUI>();
            _ribonSurvivalTimeText = _selfTransform.Find("Panel_RibonSurvivalTime/Text_Main").GetComponent<TextMeshProUGUI>();

            _homeButton = _selfTransform.Find("Panel_Navigation/Button_Home").GetComponent<Button>();
            _repeatButton = _selfTransform.Find("Panel_Navigation/Button_Repeat").GetComponent<Button>();

            _defaultTitleText = $"GAME OVER";
            _newRecordTitleText = "CONGRATULATIONS!<br><color=red>NEW RECORD<color=red>";

            _homeButton.onClick.AddListener(HomeButtonOnClickHandler);
            _repeatButton.onClick.AddListener(RepeatButtonOnClickHandler);
        }

        public void SetRibonSurvivalTime(TimeSpan survivalTime, bool isNewRecord)
        {
            _titleText.text = isNewRecord ? _newRecordTitleText : _defaultTitleText;

            _ribonSurvivalTimeText.text = $"SURVIVAL TIME: <color=red>{GeneralExtentions.GetFormattedTimeString(survivalTime)}</color>";
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