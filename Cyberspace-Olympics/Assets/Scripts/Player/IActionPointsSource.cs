using System.Collections.Generic;

namespace CyberspaceOlympics
{
    public interface IActionPointsSource
    {
        IEnumerable<IReadonlyActionPoint> ActionPoints { get; }
    }
}