using System;

namespace CyberspaceOlympics
{
    public class GameStateMachine
    {
        private static GameStateMachine _instance;
        private GameState _currentState;

        private GameStateMachine()
        {
            CurrentState = GameState.Start;
        }

        public event Action<GameState> StateChanged;
        
        public static GameStateMachine Instance => _instance ??= new GameStateMachine();

        public GameState CurrentState
        {
            get => _currentState;
            private set
            {
                if (_currentState == value)
                {
                    return;
                }
                
                _currentState = value;
                StateChanged?.Invoke(value);
            }
        }

        public void Reset() => CurrentState = GameState.Start;

        /// <summary>
        ///     Moves the state machine to the desired state.
        /// </summary>
        /// <remarks>
        ///     Allowed transitions:<br/>
        ///         <see cref="GameState.Start" /> >> <see cref="GameState.RunningSimulation" /><br/>
        ///         <see cref="GameState.PlayerPhase" /> >> <see cref="GameState.RunningSimulation" /><br/>
        ///         <see cref="GameState.RunningSimulation" /> >> <see cref="GameState.PlayerPhase" />
        ///         <see cref="GameState.RunningSimulation" /> >> <see cref="GameState.PlayerLose" />
        ///         <see cref="GameState.RunningSimulation" /> >> <see cref="GameState.PlayerWin" />
        /// </remarks>
        /// <param name="nextState">
        ///     The desired <see cref="GameState" />
        /// </param>
        /// <returns>
        ///     <see langword="true" /> if the transition was successful. <see langword="false" /> otherwise. 
        /// </returns>
        public bool TransitionTo(GameState nextState)
        {
            switch (CurrentState)
            {
                case GameState.Start or GameState.PlayerPhase when nextState is GameState.RunningSimulation:
                    CurrentState = nextState;
                    return true;
                case GameState.RunningSimulation when nextState is GameState.PlayerPhase or GameState.PlayerLose or GameState.PlayerWin:
                    CurrentState = nextState;
                    return true;
                default:
                    return false;
            }
        }
    }

    public enum GameState
    {
        Start,
        RunningSimulation,
        PlayerPhase,
        PlayerWin,
        PlayerLose
    }
}