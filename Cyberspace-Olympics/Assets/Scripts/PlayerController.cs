using System;
using System.Linq;
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

        private void Awake()
        {
            GameStateMachine.Instance.StateChanged += state =>
            {
                if (state is GameState.RunningSimulation)
                    transform.position = new Vector3(6f, -12.5f);
            };
        }

        void Update()
        {
            if (GameStateMachine.Instance.CurrentState is not GameState.PlayerPhase)
            {
                return;
            }
            
            var moveAction = playerInput.actions["Move"];
            
            if (moveAction.IsPressed())
                transform.position += moveAction.ReadValue<Vector2>().ToVector3() * (Time.deltaTime * movementSpeed);
        }

        private void OnInteract(InputValue input)
        {
            if (GameStateMachine.Instance.CurrentState is not GameState.PlayerPhase)
            {
                return;
            }
            
            var position = transform.position;

            var playerFieldUnits = GameObject.FindGameObjectsWithTag("PlayerFieldUnit");
            var unitsInRange = playerFieldUnits.Where(unit => unit.transform.position.InRange(position, healRange)).ToArray();
            
            foreach (var unit in unitsInRange)
            {
                unit.GetComponent<FieldUnitController>().Hp += 100;
            }
        }
    }
}
