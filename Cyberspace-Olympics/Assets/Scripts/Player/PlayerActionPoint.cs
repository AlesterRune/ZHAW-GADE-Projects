using System;
using R3;

namespace CyberspaceOlympics
{
    public class PlayerActionPoint : IActionPoint
    {
        private bool _isUsable;
        
        private event Action<bool> IsUsableChangedInternal;

        public bool IsUsable
        {
            get => _isUsable;
            private set
            {
                if (_isUsable != value)
                {
                    _isUsable = value;
                    IsUsableChangedInternal?.Invoke(value);
                }
            }
        }

        public IObservable<bool> IsUsableChanged { get; }

        public PlayerActionPoint()
        {
            _isUsable = true;
            IsUsableChanged = Observable
                .FromEvent<bool>(e => IsUsableChangedInternal += e, e => IsUsableChangedInternal -= e)
                .AsSystemObservable();
        }
        
        public bool Use()
        {
            if (!IsUsable)
            {
                return false;
            }
            
            IsUsable = false;
            return true;

        }

        public void Reset()
        {
            IsUsable = true;
        }
    }
}