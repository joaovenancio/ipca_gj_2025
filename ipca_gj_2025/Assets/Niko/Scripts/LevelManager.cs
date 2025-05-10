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

    public Dir dir = Dir.Horizontal;

    private Vector3 invisTargetPos;
    private Quaternion invisTargetRot;
    public float invisFollowSpeed = 15f; // You can tweak this

    private void Awake()
    {
        instance = this;
    }

    public void SelectItem(ItemSO item)
    {
        Debug.Log("Selected " + item.name);
        selectedItem = item;

        if(invisItem) Destroy(invisItem);
        dir = Dir.Horizontal;

        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0f;

        Vector2Int size = GetRotatedSize(selectedItem, dir);
        Vector3 offset = new Vector3(
            -(size.x * 0.5f - 0.5f),
            -(size.y * 0.5f - 0.5f),
            0f
        ) * 3;

        invisItem = Instantiate(
            item.prefab,
            Vector3.zero, // Will be corrected immediately below
            Quaternion.identity
        );

        UpdateMousePos(Input.mousePosition); // This sets targetPos + rot

        // Instantly snap to target position without animation on first spawn
        invisItem.transform.position = invisTargetPos;
        invisItem.transform.rotation = invisTargetRot;
    }

    public void RotateItem()
    {
        if (!invisItem) return;

        dir = ItemSO.GetNextDir(dir);
        UpdateMousePos(Input.mousePosition); // Force update with new dir
    }

    public void UpdateMousePos(Vector2 mousePosition)
    {
        if (!invisItem) return;

        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0f;

        if (TryGetSnappingPosition(pos, out Vector3 snappedWorldPos, out Quaternion snappedRotation))
        {
            invisTargetPos = snappedWorldPos;
            invisTargetRot = snappedRotation;
        }
        else
        {
            Vector2Int size = GetRotatedSize(selectedItem, dir);
            Vector3 offset = new Vector3(
                -(size.x * 0.5f - 0.5f),
                -(size.y * 0.5f - 0.5f),
                0f
            ) * 3;

            invisTargetPos = pos + offset;
            invisTargetRot = Quaternion.Euler(0f, 0f, selectedItem.GetRotationAngle(dir));
        }
    }

    public static Vector2Int GetRotatedSize(ItemSO item, Dir dir)
    {
        return dir == Dir.Horizontal
            ? new Vector2Int(item.width, item.height)
            : new Vector2Int(item.height, item.width);
    }

    public bool TryGetSnappingPosition(Vector3 mouseWorldPos, out Vector3 snappedWorldPos, out Quaternion snappedRotation)
    {
        foreach (Inventory inv in legalInventories)
        {
            inv.GetXY(mouseWorldPos, out int x, out int y);

            if (x < 0 || y < 0 || x >= inv.sizeColumns || y >= inv.sizeRows) continue;

            Vector2Int baseGrid = new Vector2Int(x, y);
            List<Vector2Int> gridPosList = selectedItem.GetGridPositionList(baseGrid, dir);

            bool canBuild = true;
            foreach (Vector2Int pos in gridPosList)
            {
                if (pos.x < 0 || pos.y < 0 || pos.x >= inv.sizeColumns || pos.y >= inv.sizeRows ||
                    !inv.cells[pos.x, pos.y].isAvailable)
                {
                    canBuild = false;
                    break;
                }
            }

            if (canBuild)
            {
                Vector2Int offset = selectedItem.GetRotationOffset(dir);
                snappedWorldPos = inv.GetWorldPosition(x, y) + new Vector3(offset.x, offset.y, 0f) * inv.cellSize;
                snappedRotation = Quaternion.Euler(0f, 0f, selectedItem.GetRotationAngle(dir));
                return true;
            }
        }

        // If no valid spot
        snappedWorldPos = Vector3.zero;
        snappedRotation = Quaternion.identity;
        return false;
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
                    Vector2Int offset = selectedItem.GetRotationOffset(dir);
                    Vector3 worldPos = inv.GetWorldPosition(x, y) + new Vector3(offset.x, offset.y, 0f) * inv.cellSize;

                    foreach (Vector2Int gridPosition in gridPositionList)
                    {
                        inv.cells[gridPosition.x, gridPosition.y].DisableCell();
                    }

                    Instantiate(
                        selectedItem.prefab,
                        worldPos,
                        Quaternion.Euler(0f, 0f, selectedItem.GetRotationAngle(dir))
                    );
                    break;
                }
            }
        }

        if (invisItem)
        {
            invisItem.transform.position = Vector3.Lerp(
                invisItem.transform.position,
                invisTargetPos,
                Time.deltaTime * invisFollowSpeed
            );

            invisItem.transform.rotation = Quaternion.Lerp(
                invisItem.transform.rotation,
                invisTargetRot,
                Time.deltaTime * invisFollowSpeed
            );
        }
    }
}
