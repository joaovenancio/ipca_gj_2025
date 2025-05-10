using CodeMonkey.Utils;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public List<Inventory> legalInventories = new ();
    public List<Inventory> illegalInventories = new ();
    public List<ItemSO> itemsToPlace = new();

    public ItemSO selectedItem;
    public Dir dir = Dir.Down;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            dir = ItemSO.GetNextDir(dir);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            foreach(Inventory inv in legalInventories)
            {
                inv.GetXY(mousePosition, out int x, out int y);

                if (x < 0 || y < 0 || x >= inv.sizeColumns || y >= inv.sizeRows) continue;

                List<Vector2Int> gridPositionList = selectedItem.GetGridPositionList(new Vector2Int(x, y), dir);

                bool canBuild = true;

                foreach(Vector2Int gridPosition in gridPositionList)
                {
                    if (gridPosition.x < 0 || gridPosition.y < 0 || gridPosition.x >= inv.sizeColumns || gridPosition.y >= inv.sizeRows)
                    {
                        canBuild = false;
                        break;
                    }

                    if (!inv.cells[gridPosition.x, gridPosition.y].isAvailable)
                    {
                        canBuild = false;
                        break;
                    }
                }

                if(canBuild)
                {
                    Vector2Int rotationOffset = selectedItem.GetRotationOffset(dir);
                    Vector3 placedObjectWorldPosition = inv.GetWorldPosition(x, y) + new Vector3(rotationOffset.x, rotationOffset.y, 0f) * inv.cellSize;
                    foreach (Vector2Int gridPosition in gridPositionList)
                    {
                        inv.cells[gridPosition.x, gridPosition.y].DisableCell();
                    }
                    Instantiate(
                        selectedItem.prefab,
                        placedObjectWorldPosition, 
                        Quaternion.Euler(0f, 0f, selectedItem.GetRotationAngle(dir))
                    );
                    break;
                }
            }
        }
    }
}
