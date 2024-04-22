using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using R3;
using UnityEngine;

namespace CUI
{
    public class CUIObservableSO : ScriptableObject, IObservable<Unit>
    {
        private readonly HashSet<IObserver<Unit>> _observers = new();
        
        [CanBeNull]
        private IDisposable _childrenSubscription = null;
        
        protected virtual IObservable<Unit> GetObservableChildren()
        {
            return Observable.Empty<Unit>().AsSystemObservable();
        }

        public IDisposable Subscribe(IObserver<Unit> observer)
        {
            _observers.Add(observer);
            return Disposable.Create(() => _observers.Remove(observer));
        }

        internal void InvokeChanged()
        {
            foreach (var observer in _observers)
            {
                observer?.OnNext(Unit.Default);
            }
        }

        internal void OnChildDisposableChanged()
        {
            _childrenSubscription?.Dispose();
            _childrenSubscription = GetObservableChildren().Subscribe(InvokeChanged);
        }

        private void Awake()
        {
            OnChildDisposableChanged();
        }

        private void OnValidate()
        {
            _childrenSubscription ??= GetObservableChildren().Subscribe(InvokeChanged);
        }

        private void OnDestroy()
        {
            _childrenSubscription?.Dispose();
        }
    }
}