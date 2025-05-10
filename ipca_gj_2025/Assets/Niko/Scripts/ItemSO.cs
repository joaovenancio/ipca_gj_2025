using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { Trash, Illegal }
public enum Dir { Up, Down, Left, Right }

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
            case Dir.Down:
                return Dir.Left;
            case Dir.Left:
                return Dir.Up;
            case Dir.Up:
                return Dir.Right;
            case Dir.Right:
                return Dir.Down;
        }
    }

    public int GetRotationAngle(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.Down:
                return 0;
            case Dir.Left:
                return 90;
            case Dir.Up:
                return 180;
            case Dir.Right:
                return 270;
        }
    }

    public Vector2Int GetRotationOffset(Dir dir)
    {
        switch (dir)
        {
            case Dir.Down: // 0°
            default:
                return new Vector2Int(0, 0);

            case Dir.Left: // 90° CCW — pivot ends up bottom-RIGHT
                return new Vector2Int(height, 0);

            case Dir.Up: // 180° — pivot ends up top-RIGHT
                return new Vector2Int(width, height);

            case Dir.Right: // 270° CCW — pivot ends up top-LEFT
                return new Vector2Int(height - 1, 1);
        }
    }

    public List<Vector2Int> GetGridPositionList(Vector2Int basePosition, Dir dir)
    {
        List<Vector2Int> gridPositionList = new List<Vector2Int>();

        int w = width;
        int h = height;

        switch (dir)
        {
            default:
            case Dir.Down: // 0°
                for (int x = 0; x < w; x++)
                    for (int y = 0; y < h; y++)
                        gridPositionList.Add(basePosition + new Vector2Int(x, y));
                break;

            case Dir.Left: // 90°
                for (int x = 0; x < h; x++)
                    for (int y = 0; y < w; y++)
                        gridPositionList.Add(basePosition + new Vector2Int(-x, y));
                break;

            case Dir.Up: // 180°
                for (int x = 0; x < w; x++)
                    for (int y = 0; y < h; y++)
                        gridPositionList.Add(basePosition + new Vector2Int(-x, -y));
                break;

            case Dir.Right: // 270°
                for (int x = 0; x < h; x++)
                    for (int y = 0; y < w; y++)
                        gridPositionList.Add(basePosition + new Vector2Int(x, -y));
                break;
        }

        return gridPositionList;
    }

    //public List<Vector2Int> GetGridPositionList(Vector2Int offset, Dir dir)
    //{
    //    List<Vector2Int> gridPositionList = new List<Vector2Int>();
    //    switch (dir)
    //    {
    //        default:
    //        case Dir.Down:
    //        case Dir.Up:
    //            for (int x = 0; x < width; x++)
    //            {
    //                for (int y = 0; y < height; y++)
    //                {
    //                    gridPositionList.Add(offset + new Vector2Int(x, y));
    //                }
    //            }
    //            break;
    //        case Dir.Left:
    //        case Dir.Right:
    //            for (int x = 0; x < height; x++)
    //            {
    //                for (int y = 0; y < width; y++)
    //                {
    //                    gridPositionList.Add(offset + new Vector2Int(x, y));
    //                }
    //            }
    //            break;
    //    }
    //    return gridPositionList;
    //}
}
