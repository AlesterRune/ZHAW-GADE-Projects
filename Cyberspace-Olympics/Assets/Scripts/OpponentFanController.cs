using System;

namespace CyberspaceOlympics
{
    public class OpponentFanController : FanController
    {
        private IDisposable _criticalHitSubscription;

        private void Start()
        {
            _criticalHitSubscription = GameController.Instance.OpponentCritical.Subscribe(PlayCheer);
        }

        private void OnDestroy()
        {
            _criticalHitSubscription.Dispose();
        }
    }
}