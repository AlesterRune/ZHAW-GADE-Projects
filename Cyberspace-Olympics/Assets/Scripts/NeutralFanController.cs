using System;

namespace CyberspaceOlympics
{
    public class NeutralFanController : FanController
    {
        private IDisposable _playerCriticalHitSubscription;
        private IDisposable _opponentCriticalHitSubscription;

        private void Start()
        {
            _playerCriticalHitSubscription = GameController.Instance.PlayerCritical.Subscribe(PlayCheer);
            _opponentCriticalHitSubscription = GameController.Instance.OpponentCritical.Subscribe(PlayCheer);
        }

        private void OnDestroy()
        {
            _playerCriticalHitSubscription.Dispose();
            _opponentCriticalHitSubscription.Dispose();
        }
    }
}