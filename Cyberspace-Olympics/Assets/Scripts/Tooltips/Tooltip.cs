using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CyberspaceOlympics
{
    [ExecuteInEditMode]
    public class Tooltip : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text headerControl;

        [SerializeField]
        private TMP_Text contentControl;

        [SerializeField]
        private LayoutElement layoutElement;

        [SerializeField]
        private int wrapLimit;

        public void SetText(string content, string header = "")
        {
            headerControl.gameObject.SetActive(!string.IsNullOrEmpty(header));
            headerControl.text = header ?? string.Empty;
            contentControl.text = content;
            UpdateSize();
        }
        
#if UNITY_EDITOR
        private void Update()
        {
            UpdateSize();
        }
#endif

        [ContextMenu("Update Size")]
        private void UpdateSize()
        {
            var headerLength = headerControl.text.Length;
            var contentLength = contentControl.text.Length;

            layoutElement.enabled = headerLength > wrapLimit || contentLength > wrapLimit;
        }
    }
}
