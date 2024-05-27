using UnityEngine;

namespace CyberspaceOlympics
{
    public class Testing : MonoBehaviour
    {
        [SerializeField]
        private Transform spawnTarget = null;
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var value = Random.Range(-10, 10);
                TextPopupController.SignedNumeric(spawnTarget?.position ?? MouseWorldPosition(), value, isCritical: value is < -8 or > 8);
            }
        }

        private static Vector3 MouseWorldPosition()
        {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition).Flatten();
        }
    }
}
