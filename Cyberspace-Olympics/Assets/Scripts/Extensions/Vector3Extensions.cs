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

        public static Vector2 ToVector2(this Vector3 self)
        {
            return new Vector2(self.x, self.y);
        }
    }
}