using Unity.VisualScripting;

namespace CyberspaceOlympics
{
    public static class DieTypeExtensions
    {
        public static int Sides(this DieType dieType)
        {
            var typeInfo = dieType.GetType().GetField(dieType.ToString());
            var attribute = typeInfo.GetAttribute(typeof(SidesAttribute));
            return (attribute as SidesAttribute)?.Sides ?? -1;
        }
    }
}