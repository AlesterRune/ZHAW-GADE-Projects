using TMPro;
using UnityEngine;

public class UiController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textMesh;

    [SerializeField]
    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        player.PointsUpdated += OnPlayerTargetReached;
    }

    private void OnPlayerTargetReached(int points)
    {
        textMesh.text = $"Points: {points}";
    }
}
