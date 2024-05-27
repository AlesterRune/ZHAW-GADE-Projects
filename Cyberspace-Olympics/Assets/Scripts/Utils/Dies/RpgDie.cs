using System;

namespace CyberspaceOlympics
{
    public class RpgDie : IDie
    {
        private readonly Random _dieRng;

        internal RpgDie(int sides, int? seed = null)
        {
            Max = sides;
            _dieRng = seed switch
            {
                not null => new Random(seed.Value),
                _ => new Random()
            };
        }

        public int Max { get; }

        public int Roll()
        {
            var roll = _dieRng.Next(Max) + 1;
            if (roll < Max)
            {
                return roll;
            }

            return roll + Roll();
        }
    }
}