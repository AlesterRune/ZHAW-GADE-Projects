using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace CyberspaceOlympics
{
    public class ActionPointsDebug : MonoBehaviour, IActionPointsSource
    {
        private readonly IActionPoint[] _actionPoints = new IActionPoint[5];
        private Button _button;

        public IEnumerable<IReadonlyActionPoint> ActionPoints => _actionPoints.Cast<IReadonlyActionPoint>().ToArray();

        private void Awake()
        {
            for (var i = 0; i < _actionPoints.Length; i++)
            {
                _actionPoints[i] = new PlayerActionPoint();
            }
            _button = GetComponent<Button>();
            _button.onClick.AddListener(() =>
            {
                foreach (var actionPoint in _actionPoints)
                {
                    if (actionPoint.IsUsable)
                        actionPoint.Use();
                    else
                        actionPoint.Reset();
                }
            });
        }
    }
}

