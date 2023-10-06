using DG.Tweening;
using Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Pages.Components.GamePage
{
    public class Heart
    {
        private const float ShowHideAnimationDuration = 1f;

        private GameObject _selfObject;

        private Image _fullStateImage;

        public Heart(GameObject gameObject)
        {
            _selfObject = gameObject;
            var selfTransform = _selfObject.transform;

            _fullStateImage = selfTransform.Find("Image_Full").GetComponent<Image>();
            _fullStateImage.SetAlpha(0);

            SetState(true);
        }

        public void SetState(bool isActive)
        {
            _fullStateImage.DOFade(isActive ? 1 : 0, ShowHideAnimationDuration);
        }

        public void Destroy()
        {
            Object.Destroy(_selfObject);
        }
    }
}