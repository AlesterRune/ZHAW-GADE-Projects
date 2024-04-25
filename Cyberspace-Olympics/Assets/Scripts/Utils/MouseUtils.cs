using UnityEngine;
using UnityEngine.InputSystem;

namespace CyberspaceOlympics
{
    public static class MouseUtils
    {
        public static Vector2 Position()
        {
            return Mouse.current.position.ReadValue();
        }

        public static Vector2 WorldPosition(Camera camera)
        {
            return camera.ScreenToWorldPoint(Position());
        }
    }
}