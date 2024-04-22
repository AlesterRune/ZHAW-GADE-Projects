using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace CUI.Panels
{
    public class CUIView : MonoBehaviour
    {
        [OnChangedCall(nameof(OnViewDataChanged))]
        [SerializeField]
        private CUIViewSO viewData;

        [Header("Containers")]
        [SerializeField]
        private GameObject containerTop;
        [SerializeField]
        private GameObject containerCenter;
        [SerializeField]
        private GameObject containerBottom;

        private VerticalLayoutGroup _verticalLayoutGroup;

        private Image _imageTop;
        private Image _imageCenter;
        private Image _imageBottom;

        [CanBeNull]
        private IDisposable ViewDataSubscription { get; set; }

        public void OnViewDataChanged()
        {
            if (viewData is not null)
            {
                ViewDataSubscription?.Dispose();
                ViewDataSubscription = viewData.Subscribe(_ => Init());
            }
            else
            {
                Debug.LogError("View Data Change registering failed.");
            }

            Init();
        }

        private void Awake()
        {
            OnViewDataChanged();
        }

        private void OnValidate()
        {
            ViewDataSubscription ??= viewData.Subscribe(_ => Init());
            Init();
        }

        private void OnDestroy()
        {
            ViewDataSubscription?.Dispose();
        }

        [ContextMenu("Apply Changes")]
        public void Init()
        {
            Setup();
            Configure();
        }

        private void Setup()
        {
            try
            {
                _verticalLayoutGroup = GetComponent<VerticalLayoutGroup>();
                _imageTop = containerTop.GetComponent<Image>();
                _imageCenter = containerCenter.GetComponent<Image>();
                _imageBottom = containerBottom.GetComponent<Image>();
            }
            catch (MissingReferenceException)
            {
                ViewDataSubscription?.Dispose();
            }
        }

        private void Configure()
        {
            _verticalLayoutGroup.padding = viewData.Padding;
            _verticalLayoutGroup.spacing = viewData.Spacing;
            
            _imageTop.color = viewData.Theme.Secondary;
            _imageCenter.color = viewData.Theme.Primary;
            _imageBottom.color = viewData.Theme.Tertiary;
        }
    }
}




