using JetBrains.Annotations;
using UnityEngine;

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

        [SerializeField]
        [CanBeNull]
        private AudioSource critCheer = null;
        
        private int _damageCache;

        public int MaxHp => maxHp;
        
        public int Hp
        {
            get => hp;
            set => hp = Mathf.Clamp(value, 0, maxHp);
        }

        public int ThreatLevel => threatLevel;
        
        public bool IsHurt { get; internal set; }
        
        public bool IsDead { get; internal set; }
        
        public bool IsPlayerFieldUnit { get; private set; }

        public void UpdateHp(int value, bool isCritical = false)
        {
            if (Hp == 0 || Hp == maxHp && value >= 0)
            {
                return;
            }

            if (isCritical && critCheer is not null)
            {
                critCheer?.Play();
            }
            
            Hp += value;
            TextPopupController.SignedNumeric(transform.position, value, isCritical);
        }

        public void PlayAttack()
        {
            animator.SetTrigger("TAttack");
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

                if (state is GameState.PlayerPhase && gameObject.CompareTag("OpponentFieldUnit"))
                {
                    UpdateHp(15 * GameStateMachine.Instance.RoundNo);
                }
            };

            GameStateMachine.Instance.RoundNoChanged += () =>
            {
                Hp = MaxHp;
                animator.SetTrigger("TRaise");
            };
        }

        private void Update()
        {
            IsHurt = hp <= 25;
            IsDead = hp == 0;
            animator.SetBool("IsHurt", IsHurt);
            animator.SetBool("IsDead", IsDead);
        }

        private void FixedUpdate()
        {
            if (GameStateMachine.Instance.CurrentState is not GameState.RunningSimulation)
            {
                return;
            }
        }
    }
}
