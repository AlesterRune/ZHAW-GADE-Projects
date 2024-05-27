using System.Linq;

namespace CyberspaceOlympics
{
    public class CompositeRpgDie : IDie
    {
        private readonly IDie _baseDie;
        private readonly IDie[] _sideDies;

        public CompositeRpgDie(IDie baseDie, IDie[] sideDies)
        {
            _baseDie = baseDie;
            _sideDies = sideDies;
        }
        
        public int Roll()
        {
            return _baseDie.Roll() + _sideDies.Select(d => d.Roll()).Sum();
        }
    }
}