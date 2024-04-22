using System;

namespace CustomUI
{
    public class Observer<T> : IObserver<T>
    {
        private readonly Action<T> _onNext;
        private readonly Action<Exception> _onError;
        private readonly Action _onCompleted;

        private Observer(Action<T> onNext, Action<Exception> onError, Action onCompleted)
        {
            _onNext = onNext;
            _onError = onError;
            _onCompleted = onCompleted;
        }
        
        public static Observer<T> Create(Action<T> onNext, Action<Exception> onError = null, Action onCompleted = null)
        {
            onError ??= _ => { };
            onCompleted ??= () => { };

            return new Observer<T>(onNext, onError, onCompleted);
        }
        
        public void OnCompleted()
        {
            _onCompleted();
        }

        public void OnError(Exception error)
        {
            _onError(error);
        }

        public void OnNext(T value)
        {
            _onNext(value);
        }
    }
}