using UnityEngine;
using UnityEngine.UI;

namespace CyberspaceOlympics
{
    public static class ImageExtensions
    {
        public static void SetEnabled(this Image self, bool isEnabled)
        {
            self.enabled = isEnabled;
            foreach (var childBehaviour in self.GetComponentsInChildren<Behaviour>())
            {
                childBehaviour.enabled = isEnabled;
            }
        }
    }
}