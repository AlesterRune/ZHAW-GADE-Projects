using System;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class WorldController : MonoBehaviour
{
    private const int North = 7;
    private const int East = 16;
    private const int South = -7;
    private const int West = -16;
    private const int Center = 0;
 
    
    // ReSharper disable InconsistentNaming
    private const string TBorderSouth = "t-border-s";
    private const string TBorderNorth = "t-border-n";
    // ReSharper restore InconsistentNaming
    private const string CornerNorthEast = "corner-ne";
    private const string CornerNorthWest = "corner-nw";
    private const string CornerSouthEast = "corner-se";
    private const string CornerSouthWest = "corner-sw";
    private const string BorderNorth = "border-n";
    private const string BorderSouth = "border-s";
    private const string BorderEast = "border-e";
    private const string BorderWest = "border-w";
    private const string CenterWithLine = "center-line";
    private const string CenterNoLine = "center";
    
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
            (East, North) => sprites.First(s => s.name == CornerNorthEast),
            (West, North) => sprites.First(s => s.name == CornerNorthWest),
            (Center, North) => sprites.First(s => s.name == TBorderNorth),
            (_, North) when Mathf.CeilToInt(East / 2f) == x => sprites.First(s => s.name == TBorderNorth),
            (_, North) when Mathf.CeilToInt(West / 2f) == x => sprites.First(s => s.name == TBorderNorth),
            (_, North) => sprites.First(s => s.name == BorderNorth),
            (East, South) => sprites.First(s => s.name == CornerSouthEast),
            (West, South) => sprites.First(s => s.name == CornerSouthWest),
            (Center, South) => sprites.First(s => s.name == TBorderSouth),
            (_, South) when Mathf.CeilToInt(East / 2f) == x => sprites.First(s => s.name == TBorderSouth),
            (_, South) when Mathf.CeilToInt(West / 2f) == x => sprites.First(s => s.name == TBorderSouth),
            (_, South) => sprites.First(s => s.name == BorderSouth),
            (_, _) when Mathf.CeilToInt(East / 2f) == x => sprites.First(s => s.name == CenterWithLine),
            (_, _) when Mathf.CeilToInt(West / 2f) == x => sprites.First(s => s.name == CenterWithLine),
            (East, _) => sprites.First(s => s.name == BorderEast),
            (West, _) => sprites.First(s => s.name == BorderWest),
            (Center, _) => sprites.First(s => s.name == CenterWithLine),
            _ => sprites.First(s => s.name == CenterNoLine)
        };
    }
}
