using CustomUI;
using UnityEngine;

namespace CUI.Themes
{
    [CreateAssetMenu(menuName = "CUI/Theme", fileName = "CUIThemeSO")]
    public class CUIThemeSO : CUIObservableSO
    {
        [Header("Primary")]
        
        [OnChangedCall(nameof(InvokeChanged))]
        [SerializeField]
        private Color primary;
        [OnChangedCall(nameof(InvokeChanged))]
        [SerializeField]
        private Color onPrimary;
        
        [Header("Secondary")]
        
        [OnChangedCall(nameof(InvokeChanged))]
        [SerializeField]
        private Color secondary;
        [OnChangedCall(nameof(InvokeChanged))]
        [SerializeField]
        private Color onSecondary;
        
        [Header("Tertiary")]
        
        [OnChangedCall(nameof(InvokeChanged))]
        [SerializeField]
        private Color tertiary;
        [OnChangedCall(nameof(InvokeChanged))]
        [SerializeField]
        private Color onTertiary;

        [Header("Misc")]
        
        [OnChangedCall(nameof(InvokeChanged))]
        [SerializeField]
        private Color disabled;

        public Color Primary => primary;
        public Color OnPrimary => onPrimary;
        public Color Secondary => secondary;
        public Color OnSecondary => onSecondary;
        public Color Tertiary => tertiary;
        public Color OnTertiary => onTertiary;
        public Color Disabled => disabled;
    }
}