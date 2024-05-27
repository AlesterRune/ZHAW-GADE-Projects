using System;

namespace CyberspaceOlympics
{
    public interface IReadonlyActionPoint
    {
        bool IsUsable { get; }
        
        IObservable<bool> IsUsableChanged { get; }

        bool Use();
    }
}