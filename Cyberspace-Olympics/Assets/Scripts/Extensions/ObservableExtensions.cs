using System;

namespace CyberspaceOlympics
{
    public static class ObservableExtensions
    {
        public static IDisposable Subscribe<T>(this IObservable<T> self, Action onNext)
        {
            return self.Subscribe(SimpleObserver<T>.Create(_ => onNext()));
        }
        
        public static IDisposable Subscribe<T>(this IObservable<T> self, Action<T> onNext)
        {
            return self.Subscribe(SimpleObserver<T>.Create(onNext));
        }
        
        private class SimpleObserver<T> : IObserver<T>
        {
            private readonly Action<T> _onNext;
            private readonly Action<Exception> _onError;
            private readonly Action _onCompleted;

            private SimpleObserver(Action<T> onNext, Action<Exception> onError, Action onCompleted)
            {
                _onNext = onNext;
                _onError = onError;
                _onCompleted = onCompleted;
            }
        
            public static SimpleObserver<T> Create(Action<T> onNext, Action<Exception> onError = null, Action onCompleted = null)
            {
                onError ??= _ => { };
                onCompleted ??= () => { };

                return new SimpleObserver<T>(onNext, onError, onCompleted);
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
}