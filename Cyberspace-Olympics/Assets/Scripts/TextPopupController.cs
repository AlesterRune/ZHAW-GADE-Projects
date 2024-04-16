using System;
using TMPro;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class TextPopupController : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro textMesh;

    private Color _textColor;
    
    public static TextPopupController SignedNumeric(Vector3 position, int value, bool isCritical = false)
    {
        var popup = Instantiate(SceneAssets.Instance.TextPopupPrefab, position, Quaternion.identity);
        var controller = popup.GetComponent<TextPopupController>();
        controller.SetNumeric(value, isCritical);
        return controller;
    }
    
    private void SetNumeric(int value, bool isCritical)
    {
        var sign = value <= 0 ? "" : "+";
        _textColor = value < 0 ? Color.red : value > 0 ? Color.green : Color.white;
        textMesh.SetText($"{sign}{value}");
        textMesh.fontSize = isCritical ? 12 : 8;
        textMesh.color = _textColor;
    }

    private void Start()
    {
        var targetPosition = transform.position + new Vector3(Random.Range(0.2f, 0.6f), 1f);
        transform
            .DOMove(targetPosition, 1f)
            .SetEase(Ease.Linear)
            .OnComplete(FadeAndKill);
    }

    private void FadeAndKill()
    {
        transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBounce).OnComplete(() => Destroy(gameObject));
    }
}
