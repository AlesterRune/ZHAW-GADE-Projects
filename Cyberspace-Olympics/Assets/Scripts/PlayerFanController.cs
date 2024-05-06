using System;

namespace CyberspaceOlympics
{
    public class PlayerFanController : FanController
    {
        
        private IDisposable _criticalHitSubscription;

        private void Start()
        {
            _criticalHitSubscription = GameController.Instance.PlayerCritical.Subscribe(PlayCheer);
        }

        private void OnDestroy()
        {
            _criticalHitSubscription.Dispose();
        }
    }
}