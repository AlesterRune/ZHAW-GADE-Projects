using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class WorldController : MonoBehaviour
{
    private const int North = 10;
    private const int East = 25;
    private const int South = -10;
    private const int West = -25;
    private const int Center = 0;
    
    [SerializeField]
    private Sprite[] sprites;

    [SerializeField]
    private GameObject tilePrefab;

    [ContextMenu("Redraw")]
    private void RedrawField()
    {
        ClearAndDestroyTiles();
        DrawField();
    }
    
    private void DrawField()
    {
        for (var x = West; x <= East; x++)
        {
            for (var y = South; y <= North; y++)
            {
                var tile = Instantiate(tilePrefab, new Vector3(x, y), Quaternion.identity, transform);
                tile.name = $"Tile_{x}:{y}";
                var spriteRenderer = tile.GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = GetPositionBasedSprite(x, y);
            }
        }
    }

    private void ClearAndDestroyTiles()
    {
        var tiles = GetComponentsInChildren<Transform>().Where(go => go.CompareTag("Tile"));
        foreach (var tile in tiles)
        {
            if (Application.isPlaying)
            {
                Destroy(tile.gameObject);
            }
#if UNITY_EDITOR
            else
            {
                DestroyImmediate(tile.gameObject);
            }
#endif
        }
    }

    private Sprite GetPositionBasedSprite(int x, int y)
    {
        return (x, y) switch
        {
            (East, North) => sprites.First(s => s.name == "corner-ne"),
            (West, North) => sprites.First(s => s.name == "corner-nw"),
            (Center, North) => sprites.First(s => s.name == "north-t"),
            (_, North) when Mathf.CeilToInt(East / 2f) == x => sprites.First(s => s.name == "north-t"),
            (_, North) when Mathf.CeilToInt(West / 2f) == x => sprites.First(s => s.name == "north-t"),
            (_, North) => sprites.First(s => s.name == "north"),
            (East, South) => sprites.First(s => s.name == "corner-se"),
            (West, South) => sprites.First(s => s.name == "corner-sw"),
            (Center, South) => sprites.First(s => s.name == "south-t"),
            (_, South) when Mathf.CeilToInt(East / 2f) == x => sprites.First(s => s.name == "south-t"),
            (_, South) when Mathf.CeilToInt(West / 2f) == x => sprites.First(s => s.name == "south-t"),
            (_, South) => sprites.First(s => s.name == "south"),
            (_, _) when Mathf.CeilToInt(East / 2f) == x => sprites.First(s => s.name == "center-i"),
            (_, _) when Mathf.CeilToInt(West / 2f) == x => sprites.First(s => s.name == "center-i"),
            (East, _) => sprites.First(s => s.name == "east"),
            (West, _) => sprites.First(s => s.name == "west"),
            (Center, _) => sprites.First(s => s.name == "center-i"),
            _ => sprites.First(s => s.name == "center")
        };
    }
}
