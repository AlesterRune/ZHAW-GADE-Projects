using UnityEngine;
using UnityEngine.EventSystems;

namespace CyberspaceOlympics
{
    public class TooltipTarget : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private ITooltipContentProvider _tooltipContentProvider;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (GameStateMachine.Instance.CurrentState is not GameState.PlayerPhase)
            {
                return;
            }
            
            TooltipSystem.SetPosition(transform.position, new Vector2(0, -62));
            var header = _tooltipContentProvider?.GetTooltipHeader() ?? string.Empty;
            var content = _tooltipContentProvider?.GetTooltipContent() ?? string.Empty;
            TooltipSystem.Show(content, header);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            TooltipSystem.Hide();
        }

        private void Start()
        {
            _tooltipContentProvider = GetComponent<ITooltipContentProvider>();
        }
    }
}