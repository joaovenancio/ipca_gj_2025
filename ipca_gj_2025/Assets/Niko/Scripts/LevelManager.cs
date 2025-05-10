using CodeMonkey.Utils;
using JetBrains.Annotations;
using NUnit;
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
    public GameObject invisItem;

    public Dir dir = Dir.Down;

    private void Awake()
    {
        instance = this;
    }

    public void SelectItem(ItemSO item)
    {
        Debug.Log("Selected " + item.name);
        selectedItem = item;
        if(invisItem) Destroy(invisItem);
        dir = Dir.Down;
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        invisItem = Instantiate(
            selectedItem.prefab,
            new Vector3(pos.x, pos.y, 0f),
            Quaternion.Euler(0f, 0f, selectedItem.GetRotationAngle(dir))
        );
        Debug.Log(invisItem.transform.name);
    }

    public void RotateItem()
    {
        if(!invisItem) return;

        dir = ItemSO.GetNextDir(dir);
        Vector2Int rotationOffset = selectedItem.GetRotationOffset(dir);
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0f;
        Vector3 placedObjectWorldPosition = pos + new Vector3(rotationOffset.x, rotationOffset.y, 0f);
        invisItem.transform.rotation = Quaternion.Euler(0f, 0f, selectedItem.GetRotationAngle(dir));
        //invisItem.transform.position = placedObjectWorldPosition;
    }

    public void UpdateMousePos(Vector2 mousePosition)
    {
        if (!invisItem) return;

        Vector2Int rotationOffset = selectedItem.GetRotationOffset(dir);
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0f;
        //Vector3 placedObjectWorldPosition = pos + new Vector3(rotationOffset.x, rotationOffset.y, 0f);
        //invisItem.transform.position = placedObjectWorldPosition;
        invisItem.transform.position = pos;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            //dir = ItemSO.GetNextDir(dir);
            RotateItem();
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
