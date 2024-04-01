using UnityEngine;

namespace CyberspaceOlympics
{
    internal static class Vector3Extensions
    {
        public static bool InRange(this Vector3 self, Vector3 target, float range)
        {
            return (self.x >= target.x - range && self.x <= target.x + range) &&
                (self.y >= target.y - range && self.y <= target.y + range);
        }
    }
}