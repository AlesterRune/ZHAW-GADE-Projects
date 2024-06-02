using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace CyberspaceOlympics
{
    public class UiController : MonoBehaviour
    {
        [SerializeField]
        private Image menuBackground;
        
        [SerializeField]
        private Button startButton;
        
        [SerializeField]
        private Button hudActionButtonButton;

        [SerializeField]
        private Button exitButton;
        
        [SerializeField]
        private TMP_Text simulationInfo;
        
        [SerializeField]
        private TMP_Text gameInfo;

        public static UiController Instance { get; private set; }
        
        private void OnCancel(InputValue input)
        {
            if (GameStateMachine.Instance.CurrentState is GameState.RunningSimulation or GameState.Start)
            {
                return;
            }

            ToggleMenu();
        }

        private void ToggleMenu()
        {
            var menuGo = menuBackground.gameObject;
            menuGo.SetActive(!menuGo.activeSelf);
        }

        private static void StartSimulation()
        {
            GameStateMachine.Instance.TransitionTo(GameState.RunningSimulation);
        }

        private void Awake()
        {
            Instance = this;
            menuBackground.SetEnabled(true);
            gameInfo.enabled = false;
            hudActionButtonButton.interactable = false; 
            hudActionButtonButton.onClick.AddListener(StartSimulation);
            startButton.onClick.AddListener(StartGame);
            exitButton.onClick.AddListener(ExitGame);
            
            GameStateMachine.Instance.StateChanged += OnGameStateChanged;
        }

        private static void ExitGame()
        {
#if UNITY_EDITOR
            // Application.Quit() does not work in the editor so
            // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void OnGameStateChanged(GameState state)
        {
            simulationInfo.enabled = state is GameState.RunningSimulation;
            hudActionButtonButton.interactable = state is GameState.PlayerPhase or GameState.PlayerLose or GameState.PlayerWin;
            gameInfo.SetText(state.ToString());
            gameInfo.enabled = state is GameState.PlayerLose or GameState.PlayerWin;

            if (state is GameState.PlayerLose or GameState.PlayerWin)
            {
                hudActionButtonButton.GetComponentInChildren<TMP_Text>().SetText("Next Round");
                hudActionButtonButton.onClick.RemoveListener(StartSimulation);
                hudActionButtonButton.onClick.AddListener(NextRound);
            }
        }

        private void NextRound()
        {
            hudActionButtonButton.onClick.RemoveListener(NextRound);
            hudActionButtonButton.onClick.AddListener(StartSimulation);
            hudActionButtonButton.GetComponentInChildren<TMP_Text>().SetText("Continue");
            StartSimulation();
        }

        private void StartGame()
        {
            startButton.onClick.RemoveListener(StartGame);
            startButton.onClick.AddListener(ToggleMenu);
            menuBackground.gameObject.SetActive(false);
            startButton.GetComponentInChildren<TMP_Text>().SetText("Back");
            GameStateMachine.Instance.TransitionTo(GameState.RunningSimulation);
        }
    }
}
