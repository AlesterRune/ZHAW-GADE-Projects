using System;
using UnityEngine;
using UnityEngine.UI;

namespace CyberspaceOlympics
{
    public class ActionPointUiController : MonoBehaviour
    {
        [SerializeField]
        private Sprite actionPointUsableSprite;
        
        [SerializeField]
        private Sprite actionPointUsedSprite;

        private IReadonlyActionPoint _actionPoint;
        private Image _image;
        private IDisposable _actionPointSubscription;

        public void SetSource(IReadonlyActionPoint source)
        {
            _actionPointSubscription?.Dispose();
            _actionPoint = source;
            OnIsUsableChanged(_actionPoint.IsUsable);
            _actionPointSubscription = _actionPoint.IsUsableChanged.Subscribe(OnIsUsableChanged);
        }

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        private void OnDestroy()
        {
            _actionPointSubscription?.Dispose();
        }

        private void OnIsUsableChanged(bool state)
        {
            _image.sprite = state ? actionPointUsableSprite : actionPointUsedSprite;
        }
    }
}
