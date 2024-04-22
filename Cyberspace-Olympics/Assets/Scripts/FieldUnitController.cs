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

        [SerializeField]
        private int threatLevel = 10;

        private int _damageCache;
        
        public int Hp
        {
            get => hp;
            set => hp = Mathf.Clamp(value, 0, maxHp);
        }

        public int ThreatLevel => threatLevel;
        
        public bool IsHurt { get; internal set; }
        
        public bool IsPlayerFieldUnit { get; private set; }

        public void UpdateHp(int value, bool isCritical = false)
        {
            if (Hp == 0 && value <= 0 || Hp == maxHp && value >= 0)
            {
                return;
            }
            
            Hp += value;
            TextPopupController.SignedNumeric(transform.position, value, isCritical);
        }

        private void Awake()
        {
            IsPlayerFieldUnit = CompareTag("PlayerFieldUnit");
        }

        private void Start()
        {
            GameController.Instance.RegisterUnit(this);
            GameStateMachine.Instance.StateChanged += state =>
            {
                if (state is GameState.PlayerPhase && _damageCache > 0)
                {
                    UpdateHp(-_damageCache, _damageCache > Hp / 2 || _damageCache > 130);
                    _damageCache = 0;
                }
            };
        }

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
            
            // var dieRoll = Random.Range(1, 100);
            // var damage = dieRoll switch
            // {
            //     100 => 10,
            //     <50 => 0,
            //     <80 => 1,
            //     _ => 2
            // };
            //
            // _damageCache += damage;
            // damageCalcs++;
            //
            // if (damageCalcs % 50 == 0)
            // {
            //     UpdateHp(-_damageCache);
            //     _damageCache = 0;
            // }
        }
    }
}
