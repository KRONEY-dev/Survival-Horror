using System;
using UnityEngine;

namespace Extensions
{
    public class Timer
    {
        private float _baseTime;

        private float _timerTime;
        private bool _isTimerActive;

        private Action _callback;

        private bool _isRepeatable;

        public void Start(float durationInSeconds, Action callback, bool isRepeatable = false)
        {
            _baseTime = durationInSeconds;

            _timerTime = _baseTime;

            _callback = callback;
            _isRepeatable = isRepeatable;

            _isTimerActive = true;
        }

        public void Stop()
        {
            _isTimerActive = false;
        }

        public void Update()
        {
            if (_isTimerActive)
            {
                _timerTime -= Time.deltaTime;

                if (_timerTime <= 0f)
                {
                    if (_isRepeatable)
                    {
                        _timerTime = _baseTime;
                    }
                    else
                    {
                        _isTimerActive = false;
                    }

                    _callback?.Invoke();
                }
            }
        }
    }
}