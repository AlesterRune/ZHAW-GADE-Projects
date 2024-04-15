using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CyberspaceOlympics
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private PlayerInput playerInput;

        [SerializeField]
        private float movementSpeed = 2f;

        [SerializeField]
        private float healRange = 0.75f;

        [SerializeField]
        private Animator skillAnimator;

        private void Awake()
        {
            GameStateMachine.Instance.StateChanged += state =>
            {
                if (state is GameState.RunningSimulation)
                    transform.position = new Vector3(6f, -12.5f);
            };
        }

        private void Update()
        {
            if (GameStateMachine.Instance.CurrentState is not GameState.PlayerPhase)
            {
                return;
            }
            
            var moveAction = playerInput.actions["Move"];
            
            if (moveAction.IsPressed())
                transform.position += moveAction.ReadValue<Vector2>().ToVector3() * (Time.deltaTime * movementSpeed);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, healRange);
        }

        private void OnInteract(InputValue input)
        {
            if (GameStateMachine.Instance.CurrentState is not GameState.PlayerPhase)
            {
                return;
            }

            var hits = Physics2D.OverlapCircleAll(transform.position.ToVector2(), healRange);
            skillAnimator.SetTrigger("Heal");
            foreach (var hit in hits)
            {
                var unitController = hit.GetComponent<FieldUnitController>();
                unitController.UpdateHp(100);
            }
        }
    }
}
