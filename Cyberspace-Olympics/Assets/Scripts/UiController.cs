using UnityEngine;
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
        private Texture2D cursorVisual;

        private static void NextRound()
        {
            GameStateMachine.Instance.TransitionTo(GameState.RunningSimulation);
        }

        private void Awake()
        {
            menuBackground.SetEnabled(true);
            overlayBackground.SetEnabled(false);
            nextButton.interactable = false; 
            startButton.onClick.AddListener(StartGame);
            nextButton.onClick.AddListener(NextRound);
            
            var hotspot = new Vector2(cursorVisual.width / 2, cursorVisual.height / 2);
            Cursor.SetCursor(cursorVisual, hotspot, CursorMode.Auto);
            
            GameStateMachine.Instance.StateChanged += OnGameStateChanged;
        }

        private void OnGameStateChanged(GameState state)
        {
            overlayBackground.SetEnabled(state is GameState.RunningSimulation);
            nextButton.interactable = state is GameState.PlayerPhase;
        }

        private void StartGame()
        {
            menuBackground.SetEnabled(false);
            GameStateMachine.Instance.TransitionTo(GameState.RunningSimulation);
        }
    }
}
