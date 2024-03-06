using System;
using UnityEngine;

namespace DefaultNamespace
{
    public static class VectorExtensions
    {
        private const float Tolerance = 0.001f;

        public static bool IsSmaller(this Vector3 self, Vector3 other)
        {
            if ((Math.Abs(self.x - self.y) > Tolerance &&
                Math.Abs(self.x - self.z) > Tolerance) || 
                (Math.Abs(other.x - other.y) > Tolerance && 
                 Math.Abs(other.x - other.z) > Tolerance))
                throw new ArgumentException();

            return self.x <= other.x;
        }

        public static bool IsBigger(this Vector3 self, Vector3 other)
        {
            if ((Math.Abs(self.x - self.y) > Tolerance &&
                Math.Abs(self.x - self.z) > Tolerance) || 
                (Math.Abs(other.x - other.y) > Tolerance && 
                 Math.Abs(other.x - other.z) > Tolerance))
                throw new ArgumentException();

            return self.x >= other.x;
        }
    }
}