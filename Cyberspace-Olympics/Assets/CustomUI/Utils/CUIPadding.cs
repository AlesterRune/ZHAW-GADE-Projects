using System;
using CUI;
using CUI.Panels;
using UnityEngine;

namespace CustomUI
{
    [Serializable]
    public class CUIPadding
    {
        [OnChangedCall(nameof(CUIObservableSO.InvokeChanged))]
        [SerializeField]
        private int top;
            
        [OnChangedCall(nameof(CUIObservableSO.InvokeChanged))]
        [SerializeField]
        private int bottom;
            
        [OnChangedCall(nameof(CUIObservableSO.InvokeChanged))]
        [SerializeField]
        private int left;
            
        [OnChangedCall(nameof(CUIObservableSO.InvokeChanged))]
        [SerializeField]
        private int right;
            
        public int Top
        {
            get => top; 
            set => top = value;
        }
            
        public int Bottom
        {
            get => bottom; 
            set => bottom = value;
        }
            
        public int Left
        {
            get => left; 
            set => left = value;
        }
            
        public int Right
        {
            get => right; 
            set => right = value;
        }

        public static implicit operator RectOffset(CUIPadding self)
        {
            return new RectOffset(self.left, self.right, self.top, self.bottom);
        }
    }
}