using System.ComponentModel;
using UnityEngine;

namespace CyberspaceOlympics
{
    [CreateAssetMenu(menuName = "Player/StatSheet", fileName = "StatSheet")]
    public class StatSheet : ScriptableObject
    {

        [field: SerializeField]
        private int Agility { get; set; }

        [Header("Physical")]
        [SerializeField]
        private int fortitude;

        [SerializeField]
        private int might;

        
        [Header("Mental")]

        [SerializeField]
        private int learning;

        [SerializeField]
        private int logic;

        [SerializeField]
        private int perception;

        [SerializeField]
        private int will;


        [Header("Social")]

        [SerializeField]
        private int deception;

        [SerializeField]
        private int persuasion;

        [SerializeField]
        private int presence;

        [Header("Extraordinary")]

        [SerializeField]
        private int alteration;

        [SerializeField]
        private int creation;

        [SerializeField]
        private int energy;

        [SerializeField]
        private int entropy;

        [SerializeField]
        private int influence;

        [SerializeField]
        private int movement;

        [SerializeField]
        private int prescience;

        [SerializeField]
        private int protection;
    }
}