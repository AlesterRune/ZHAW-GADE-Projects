using System;

namespace CUI
{
    public static class ObservableExtensions
    {
        public static IDisposable Subscribe<T>(this IObservable<T> self, Action onNext)
        {
            return self.Subscribe(CustomUI.Observer<T>.Create(_ => onNext()));
        }
        
        public static IDisposable Subscribe<T>(this IObservable<T> self, Action<T> onNext)
        {
            return self.Subscribe(CustomUI.Observer<T>.Create(onNext));
        }
    }
}