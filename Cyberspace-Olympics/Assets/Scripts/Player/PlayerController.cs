using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CyberspaceOlympics
{
    public class PlayerController : MonoBehaviour, IActionPointsSource
    {
        [SerializeField]
        private GameObject skillCursorVisual;
        
        [SerializeField]
        private PlayerInput playerInput;

        [SerializeField]
        private float movementSpeed = 2f;

        [SerializeField]
        private float healRange = 0.75f;

        [SerializeField]
        private Transform healVisualPrefab;

        [SerializeField]
        private int initialActionPoints = 3;

        private IList<IActionPoint> _actionPoints = new List<IActionPoint>();

        public IEnumerable<IReadonlyActionPoint> ActionPoints => _actionPoints;
        
        private void Awake()
        {
            for (var i = 0; i < initialActionPoints; i++)
            {
                _actionPoints.Add(new PlayerActionPoint());
            }
            
            ResetPosition();
            GameStateMachine.Instance.StateChanged += state =>
            {
                if (state is GameState.RunningSimulation)
                {
                    ResetPosition();
                    foreach (var actionPoint in _actionPoints)
                    {
                        actionPoint.Reset();
                    }
                }
            };
        }

        private void ResetPosition()
        {
            transform.position = new Vector3(6f, -12.5f);
        }

        private void Update()
        {
            if (GameStateMachine.Instance.CurrentState is not GameState.PlayerPhase)
            {
                Cursor.visible = true;
                return;
            }
            
            transform.position = MouseUtils.WorldPosition(Camera.main);
            var hasUsableActionPoints = _actionPoints.Any(ap => ap.IsUsable);
            Cursor.visible = !hasUsableActionPoints;
            skillCursorVisual.SetActive(hasUsableActionPoints);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, healRange);
        }

        private void OnMouseUp()
        {
            TriggerHeal();
        }

        private void OnInteract(InputValue input)
        {
            TriggerHeal();
        }

        private void TriggerHeal()
        {
            if (GameStateMachine.Instance.CurrentState is not GameState.PlayerPhase)
            {
                return;
            }

            if (_actionPoints.All(ap => !ap.IsUsable))
            {
                return;
            }

            var hitFieldUnits = Physics2D
                .OverlapCircleAll(transform.position.ToVector2(), healRange)
                .Where(h => h.CompareTag("PlayerFieldUnit"))
                .Select(hit => hit.GetComponent<FieldUnitController>())
                .ToArray();
            if (hitFieldUnits.Any(u => u.IsHurt))
            {
                Instantiate(healVisualPrefab, transform.position, Quaternion.identity);
                _actionPoints.First(ap => ap.IsUsable).Use();
            }
            
            foreach (var unit in hitFieldUnits)
            {
                unit.UpdateHp(25);
            }
        }

    }
}
