using DG.Tweening;
using System;
using UnityEngine;

namespace Models.Level
{
    public class Hero
    {
        private GameObject _selfObject;
        private Transform _selfTransform;

        public Hero(GameObject gameObject)
        {
            _selfObject = gameObject;
            _selfTransform = _selfObject.transform;
        }
    }
}