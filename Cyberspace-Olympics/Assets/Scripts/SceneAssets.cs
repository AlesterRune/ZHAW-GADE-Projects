using UnityEngine;

public class SceneAssets : MonoBehaviour
{
    [SerializeField]
    private Transform textPopupPrefab;
    
    public static SceneAssets Instance { get; private set; }

    public Transform TextPopupPrefab => textPopupPrefab;
    
    private void Awake()
    {
        Instance = this;
    }
}
