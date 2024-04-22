using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
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
            foreach (var (player, npc) in _playerUnits.Zip(_npcUnits, (p, n) => (Player: p, Npc: n)))
            {
                if (player.Hp > 0)
                {
                    var opponent = _npcUnits
                        .Where(u => u.Hp > 0)
                        .OrderByDescending(u => u.ThreatLevel)
                        .FirstOrDefault();
                    var roll = Random.Range(1, 20);
                    if (roll > ac)
                    {
                        opponent?.UpdateHp(-Random.Range(2, 12), roll == 20);
                    }
                }

                if (npc.Hp > 0)
                {
                    var opponent = _playerUnits
                        .Where(u => u.Hp > 0)
                        .OrderByDescending(u => u.ThreatLevel)
                        .FirstOrDefault();
                    var roll = Random.Range(1, 20);
                    if (roll > ac)
                    {
                        opponent?.UpdateHp(-Random.Range(2, 12), roll == 20);
                    }

                    if (roll == 1)
                    {
                        var unluckyTarget = _npcUnits.Skip(Random.Range(0, _npcUnits.Count - 2)).Take(1).SingleOrDefault();
                        unluckyTarget?.UpdateHp(-Random.Range(1, 8), true);
                    }
                }
            }
        }
    }
}