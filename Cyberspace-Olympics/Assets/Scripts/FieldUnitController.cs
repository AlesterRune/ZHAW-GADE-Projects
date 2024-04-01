using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CyberspaceOlympics
{
    [RequireComponent(typeof(Animator))]
    public class FieldUnitController : MonoBehaviour
    {
        [SerializeField]
        private Animator animator;

        [SerializeField]
        private int maxHp = 100;
        
        [SerializeField]
        private int hp = 100;

        [SerializeField]
        private int damageCalcs = 0;
        
        public int Hp
        {
            get => hp;
            set => hp = Mathf.Clamp(value, 0, maxHp);
        }
        
        public bool IsHurt { get; internal set; }

        private void Update()
        {
            IsHurt = hp <= 25;
            animator.SetBool("IsHurt", IsHurt);
        }

        private void FixedUpdate()
        {
            if (GameStateMachine.Instance.CurrentState is not GameState.RunningSimulation)
            {
                return;
            }
            
            var dieRoll = Random.Range(1, 100);
            Hp -= dieRoll switch
            {
                100 => 10,
                <50 => 0,
                <80 => 1,
                _ => 2
            };
            damageCalcs++;
        }
    }
}
