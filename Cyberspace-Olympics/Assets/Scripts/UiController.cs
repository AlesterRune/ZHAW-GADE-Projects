using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CyberspaceOlympics
{
    public class UiController : MonoBehaviour
    {
        [SerializeField]
        private Image menuBackground;
        
        [SerializeField]
        private Image overlayBackground;
        
        [SerializeField]
        private Button startButton;
        
        [SerializeField]
        private Button nextButton;

        [SerializeField]
        private Button restartButton;
        
        [SerializeField]
        private TMP_Text simulationInfo;
        
        [SerializeField]
        private TMP_Text gameInfo;

        public static UiController Instance { get; private set; }

        private static void NextRound()
        {
            GameStateMachine.Instance.TransitionTo(GameState.RunningSimulation);
        }

        private void Awake()
        {
            Instance = this;
            menuBackground.SetEnabled(true);
            overlayBackground.SetEnabled(false);
            gameInfo.enabled = false;
            restartButton.enabled = false;
            nextButton.interactable = false; 
            startButton.onClick.AddListener(StartGame);
            nextButton.onClick.AddListener(NextRound);
            restartButton.onClick.AddListener(Restart);
            
            GameStateMachine.Instance.StateChanged += OnGameStateChanged;
        }

        private void Restart()
        {
            GameStateMachine.Instance.Reset();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void OnGameStateChanged(GameState state)
        {
            overlayBackground.SetEnabled(state is GameState.RunningSimulation or GameState.PlayerLose or GameState.PlayerWin);
            simulationInfo.enabled = state is GameState.RunningSimulation;
            nextButton.interactable = state is GameState.PlayerPhase;
            
            restartButton.enabled = state is GameState.PlayerLose or GameState.PlayerWin;
            restartButton.interactable = state is GameState.PlayerLose or GameState.PlayerWin;
            gameInfo.SetText(state.ToString());
            gameInfo.enabled = state is GameState.PlayerLose or GameState.PlayerWin;
        }

        private void StartGame()
        {
            menuBackground.SetEnabled(false);
            GameStateMachine.Instance.TransitionTo(GameState.RunningSimulation);
        }
    }
}
