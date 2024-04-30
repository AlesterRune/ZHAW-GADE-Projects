using System;

namespace CyberspaceOlympics
{
    [AttributeUsage(AttributeTargets.Field)]
    public class SidesAttribute : Attribute
    {
        public SidesAttribute(int sides)
        {
            Sides = sides;
        }
        
        public int Sides { get; }
    }
}