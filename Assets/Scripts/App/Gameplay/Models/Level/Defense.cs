using DG.Tweening;
using Extensions;
using UnityEngine;

namespace Models.Level
{
    public class Defense
    {
        private const int FlashingCount = 10;

        public bool IsActive { get; private set; }

        private float _duration;

        private Renderer _targetRenderer;

        private Timer _timer;

        public Defense(float duration, Renderer targetRenderer)
        {
            _duration = duration;
            _targetRenderer = targetRenderer;

            _timer = new Timer();
        }

        public void Update()
        {
            _timer?.Update();
        }

        public void TurnOn()
        {
            IsActive = true;

            _targetRenderer.material.DOFade(0.3f, _duration / FlashingCount).SetLoops(FlashingCount, LoopType.Yoyo);
            _timer.Start(_duration, OnTimerEndHandler);
        }

        private void OnTimerEndHandler()
        {
            IsActive = false;
        }
    }
}