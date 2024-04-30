using System;

namespace CyberspaceOlympics
{
    public class RpgDie : IDie
    {
        private readonly int _sides;
        private readonly Random _dieRng;

        internal RpgDie(int sides, int? seed = null)
        {
            _sides = sides;
            _dieRng = seed switch
            {
                not null => new Random(seed.Value),
                _ => new Random()
            };
        }

        public int Max => _sides + 1;
        
        public int Roll()
        {
            var roll = _dieRng.Next(_sides) + 1;
            if (roll < Max)
            {
                return roll;
            }

            return roll + Roll();
        }
    }
}