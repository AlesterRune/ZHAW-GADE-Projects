﻿using UnityEngine;

namespace CyberspaceOlympics
{
    internal static class Vector2Extensions
    {
        public static Vector3 ToVector3(this Vector2 vector)
        {
            return new Vector3(vector.x, vector.y);
        }
    }
}