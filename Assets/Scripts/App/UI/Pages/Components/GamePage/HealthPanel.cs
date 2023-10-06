using System.Collections.Generic;
using UnityEngine;

namespace UI.Pages.Components.GamePage
{
    public class HealthPanel
    {
        private GameObject _selfObject;
        private Transform _selfTransform;

        private GameObject _heartPrefab;

        private List<Heart> _hearts;

        private int _currentHeartsCount;

        public HealthPanel(GameObject gameObject)
        {
            _selfObject = gameObject;
            _selfTransform = _selfObject.transform;

            _heartPrefab = Resources.Load<GameObject>("Prefabs/UI/Pages/Components/GamePage/Heart");

            _hearts = new List<Heart>();
        }

        public void SetHearts(int heartsCount)
        {
            foreach (Heart heart in _hearts)
            {
                heart.Destroy();
            }

            _hearts.Clear();

            for (int i = 0; i < heartsCount; i++)
            {
                _hearts.Add(new Heart(Object.Instantiate(_heartPrefab, _selfTransform)));
            }

            _currentHeartsCount = heartsCount;
        }

        public void RemoveHearts(int heartsToRemove)
        {
            for (int i = 0; i < heartsToRemove; i++)
            {
                if (_currentHeartsCount != 0)
                {
                    _currentHeartsCount--;

                    _hearts[_currentHeartsCount].SetState(false);
                }
                else break;
            }
        }
    }
}