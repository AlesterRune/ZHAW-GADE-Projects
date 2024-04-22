using System;
using CUI.Themes;
using CustomUI;
using R3;
using UnityEngine;

namespace CUI.Panels
{
    [CreateAssetMenu(menuName = "CUI/View", fileName = "CUIViewSO")]
    public class CUIViewSO : CUIObservableSO
    {
        [OnChangedCall(nameof(OnChildDisposableChanged))]
        [SerializeField]
        private CUIThemeSO theme;

        [SerializeField]
        private CUIPadding padding = new();

        [OnChangedCall(nameof(InvokeChanged))]
        [SerializeField]
        private float spacing;

        public CUIThemeSO Theme => theme;
        
        public CUIPadding Padding => padding;

        public float Spacing => spacing;

        protected override IObservable<Unit> GetObservableChildren()
        {
            return theme;
        }
    }   
}
