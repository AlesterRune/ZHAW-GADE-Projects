using DG.Tweening;
using UnityEngine;

namespace CyberspaceOlympics
{
    public class HealVisualController : MonoBehaviour
    {
        [SerializeField]
        private Ease ease = Ease.OutBack;
        
        private void Awake()
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(new Vector3(2.8f, 2.8f), 0.5f).SetEase(ease).OnComplete(() => Destroy(gameObject));
        }
    }
}

