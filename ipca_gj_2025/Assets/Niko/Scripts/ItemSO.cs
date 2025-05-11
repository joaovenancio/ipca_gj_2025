using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { Trash, Illegal }
public enum Dir
{
    Horizontal, // Default orientation
    Vertical    // Rotated 90° CCW
}

[CreateAssetMenu()]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public GameObject prefab;
    public Sprite icon;
    public ItemType type;
    public int width;
    public int height;

    public static Dir GetNextDir(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.Horizontal:
                return Dir.Vertical;
            case Dir.Vertical:
                return Dir.Horizontal;
        }
    }

    public int GetRotationAngle(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.Horizontal: return 0;
            case Dir.Vertical: return 90;
        }
    }

    public Vector2Int GetRotationOffset(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.Horizontal: // Down
                return new Vector2Int(0, 0);
            case Dir.Vertical:   // Left (rotated)
                return new Vector2Int(height, 0);
        }
    }

    public List<Vector2Int> GetGridPositionList(Vector2Int basePos, Dir dir)
    {
        List<Vector2Int> list = new();

        switch (dir)
        {
            case Dir.Horizontal: // Down
                for (int x = 0; x < width; x++)
                    for (int y = 0; y < height; y++)
                        list.Add(basePos + new Vector2Int(x, y));
                break;

            case Dir.Vertical: // Left (rotated)
                for (int x = 0; x < height; x++)
                    for (int y = 0; y < width; y++)
                        list.Add(basePos + new Vector2Int(-x, y));
                break;
        }

        return list;
    }
}
