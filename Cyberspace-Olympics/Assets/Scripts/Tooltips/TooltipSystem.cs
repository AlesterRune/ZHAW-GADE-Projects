using UnityEngine;

namespace CyberspaceOlympics
{
    public class TooltipSystem : MonoBehaviour
    {
        private static TooltipSystem _instance;
        
        [SerializeField]
        private Tooltip tooltip;

        private Camera Camera { get; set; }

        private Tooltip TooltipInstance { get; set; }
        
        public static void Show(string content, string header = "")
        {
            _instance.TooltipInstance.SetText(content, header);
            _instance.TooltipInstance.gameObject.SetActive(true);
        }

        public static void Hide()
        {
            _instance.TooltipInstance.gameObject.SetActive(false);
        }

        public static void SetPosition(Vector2 position, Vector2 offset)
        {
            _instance.TooltipInstance.transform.position = Camera.main.WorldToScreenPoint(position) + offset.ToVector3();
        }
        
        private void Awake()
        {
            TooltipInstance = Instantiate(tooltip, transform);
            TooltipInstance.gameObject.SetActive(false);
            Camera = Camera.main;
            _instance = this;
        }
    }
}
