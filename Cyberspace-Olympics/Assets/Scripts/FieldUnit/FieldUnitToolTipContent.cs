using System.Text;
using UnityEngine;

namespace CyberspaceOlympics
{
    public class FieldUnitToolTipContent : MonoBehaviour, ITooltipContentProvider
    {
        private FieldUnitController _unit;

        private void Awake()
        {
            _unit = GetComponent<FieldUnitController>();
        }

        public string GetTooltipHeader()
        {
            return _unit.ThreatLevel switch
            {
                15 => "Tank Unit",
                12 => "Off-Tank Unit",
                11 => "Melee DPS Unit",
                _ => "Range DPS Unit"
            };
        }

        public string GetTooltipContent()
        {
            var builder = new StringBuilder();
            builder.AppendLine($"HP: {_unit.Hp} / {_unit.MaxHp}");
            builder.AppendLine($"Thread: {_unit.ThreatLevel}");
            return builder.ToString();
        }
    }
}