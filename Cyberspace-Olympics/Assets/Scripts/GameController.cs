using UnityEngine;

namespace CyberspaceOlympics
{
    public class GameController : MonoBehaviour
    {
        private int _currentSimulations = 0;
        
        [SerializeField]
        private int simulationsPerRound = 100;

        
        private void FixedUpdate()
        {
            if (GameStateMachine.Instance.CurrentState is GameState.RunningSimulation)
            {
                _currentSimulations++;
                if (_currentSimulations > simulationsPerRound)
                {
                    GameStateMachine.Instance.TransitionTo(GameState.PlayerPhase);
                    _currentSimulations = 0;
                }
            }
        }
    }
}