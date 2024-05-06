using System.Collections.Generic;
using System.Linq;

namespace CyberspaceOlympics
{
    public class RpgDieBuilder
    {
        private readonly List<DieType> _statDies = new();
        private DieType _baseDie;
        private int? _seed = null;

        private RpgDieBuilder()
        { }

        public static RpgDieBuilder OfType(DieType baseDie)
        {
            return new RpgDieBuilder { _baseDie = baseDie };
        }

        public RpgDieBuilder WithStatDie(int stat)
        {
            switch (stat)
            {
                case 10:
                    _statDies.AddRange(new [] { DieType.D8, DieType.D8, DieType.D8, DieType.D8 });
                    break;
                case 9:
                    _statDies.AddRange(new [] { DieType.D10, DieType.D10, DieType.D10 });
                    break;
                case 8:
                    _statDies.AddRange(new [] { DieType.D8, DieType.D8, DieType.D8 });
                    break;
                case 7:
                    _statDies.AddRange(new [] { DieType.D10, DieType.D10 });
                    break;
                case 6:
                    _statDies.AddRange(new [] { DieType.D8, DieType.D8 });
                    break;
                case 5:
                    _statDies.AddRange(new [] { DieType.D6, DieType.D6 });
                    break;
                case 4:
                    _statDies.Add(DieType.D10);
                    break;
                case 3:
                    _statDies.Add(DieType.D8);
                    break;
                case 2:
                    _statDies.Add(DieType.D6);
                    break;
                case 1:
                    _statDies.Add(DieType.D4);
                    break;
            }
            return this;
        }

        public RpgDieBuilder WithSeed(int seed)
        {
            _seed = seed;
            return this;
        }

        public IDie Build()
        {
            var baseDie = new RpgDie(_baseDie.Sides(), _seed);
            if (_statDies.Any())
            {
                return new CompositeRpgDie(
                    baseDie,
                    _statDies.Select(d => new RpgDie(d.Sides(), _seed)).ToArray()
                );
            }

            return baseDie;
        }
    }
}