using System;
using System.Collections.Generic;
using System.Linq;
using R3;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CyberspaceOlympics
{
    public class GameController : MonoBehaviour
    {
        private int _currentSimulations = 0;

        private ISet<FieldUnitController> _playerUnits = new HashSet<FieldUnitController>();
        private ISet<FieldUnitController> _npcUnits = new HashSet<FieldUnitController>();
        
        [SerializeField]
        private int simulationsPerRound = 5;

        private float _timerCounter = 0f;

        public static GameController Instance { get; private set; }

        public IObservable<Unit> PlayerCritical { get; private set; }
        public IObservable<Unit> OpponentCritical { get; private set; }
        
        private event Action OnPlayerCritical;
        private event Action OnOpponentCritical;

        public void RegisterUnit(FieldUnitController unit)
        {
            if (unit.IsPlayerFieldUnit)
            {
                _playerUnits.Add(unit);
            }
            else
            {
                _npcUnits.Add(unit);
            }
        }
        
        private void Awake()
        {
            Instance = this;
            PlayerCritical = Observable.FromEvent(e => OnPlayerCritical += e, e => OnPlayerCritical -= e).AsSystemObservable();
            OpponentCritical = Observable.FromEvent(e => OnOpponentCritical += e, e => OnOpponentCritical -= e).AsSystemObservable();
        }

        private void FixedUpdate()
        {
            if (GameStateMachine.Instance.CurrentState is GameState.RunningSimulation)
            {
                _timerCounter += Time.deltaTime;
                if (_timerCounter > _currentSimulations)
                {
                    SimulateFight();
                    _currentSimulations++;
                }

                if (_npcUnits.All(u => u.Hp <= 0))
                {
                    GameStateMachine.Instance.TransitionTo(GameState.PlayerWin);
                }

                if (_playerUnits.All(u => u.Hp <= 0))
                {
                    GameStateMachine.Instance.TransitionTo(GameState.PlayerLose);
                }
                
                if (_currentSimulations > simulationsPerRound)
                {
                    GameStateMachine.Instance.TransitionTo(GameState.PlayerPhase);
                    _currentSimulations = 0;
                    _timerCounter = 0f;
                }
            }
        }

        private void SimulateFight()
        {
            const int ac = 12;
            var playerDie = RpgDieBuilder.OfType(DieType.D20).Build();
            var opponentDie = RpgDieBuilder.OfType(DieType.D20).Build();
            foreach (var (player, npc) in _playerUnits.Zip(_npcUnits, (p, n) => (Player: p, Npc: n)))
            {
                if (!player.IsDead)
                {
                    var opponent = _npcUnits
                        .Where(u => u.Hp > 0)
                        .OrderByDescending(u => u.ThreatLevel)
                        .FirstOrDefault();
                    var roll = playerDie.Roll();
                    Debug.Log($"player:{player} ;-; roll: {roll}");
                    if (roll > ac)
                    {
                        player.PlayAttack();
                        opponent?.UpdateHp(-Random.Range(2, 12), roll == 20);
                    }

                    if (roll == 20)
                    {
                        OnPlayerCritical?.Invoke();
                    }
                }

                if (!npc.IsDead)
                {
                    var opponent = _playerUnits
                        .Where(u => u.Hp > 0)
                        .OrderByDescending(u => u.ThreatLevel)
                        .FirstOrDefault();
                    var roll = opponentDie.Roll();
                    Debug.Log($"npc:{npc} ;-; roll: {roll}");
                    if (roll > ac)
                    {
                        npc.PlayAttack();
                        opponent?.UpdateHp(-Random.Range(2, 12), roll == 20);
                    }

                    if (roll == 20)
                    {
                        OnOpponentCritical?.Invoke();
                    }

                    if (roll == 1)
                    {
                        var unluckyTarget = _npcUnits.Skip(Random.Range(0, _npcUnits.Count - 2)).Take(1).SingleOrDefault();
                        unluckyTarget?.UpdateHp(-Random.Range(1, 8), true);
                        OnPlayerCritical?.Invoke();
                    }
                }
            }
        }
    }
}