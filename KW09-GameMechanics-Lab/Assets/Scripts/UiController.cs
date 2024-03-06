using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;
    
    [SerializeField]
    private TextMeshProUGUI unlocksText;

    [SerializeField]
    private Image unlocksBox; 

    [SerializeField]
    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        unlocksText.text = string.Empty;
        unlocksBox.enabled = false;
        player.PointsUpdated += OnPlayerTargetReached;
    }

    private void OnPlayerTargetReached(int points)
    {
        scoreText.text = $"Woobles: {points}";

        var unlocksTextBuilder = new StringBuilder();
        if (points >= 5)
        {
            unlocksBox.enabled = true;
            unlocksTextBuilder.AppendLine("Unlocked skills:");
            unlocksTextBuilder.AppendLine("- Flatten (Alt)");
        }
        if (points >= 10)
        {
            unlocksTextBuilder.AppendLine("- Enlarge (Ctrl)");
        }
        if (points >= 15)
        {
            unlocksTextBuilder.AppendLine("- Shrink (Shift)");
        }

        unlocksText.text = unlocksTextBuilder.ToString();
    }
}
