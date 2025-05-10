using CodeMonkey.Utils;
using JetBrains.Annotations;
using NUnit;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public List<Inventory> legalInventories = new ();
    //public List<Inventory> illegalInventories = new ();

    public List<SelectableItem> itemsToPlace = new();
    public GameObject listParent;
    public GameObject selectableItemPrefab;

    public SelectableItem selectedItem;
    public GameObject invisItem;

    public Dir dir = Dir.Horizontal;

    private Vector3 invisTargetPos;
    private Quaternion invisTargetRot;
    public float invisFollowSpeed = 15f; // You can tweak this

    private void Awake()
    {
        instance = this;
    }

    public SelectableItem FindUIItemFor(ItemSO itemSO)
    {
        foreach (var ui in itemsToPlace)
        {
            if (ui.itemSO == itemSO) return ui;
        }
        return null;
    }

    public void PlaceItem()
    {
        if (selectedItem == null) return;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        foreach (Inventory inv in legalInventories)
        {
            inv.GetXY(mousePosition, out int x, out int y);

            if (x < 0 || y < 0 || x >= inv.sizeColumns || y >= inv.sizeRows) continue;
            
            List<Vector2Int> gridPositionList = selectedItem.itemSO.GetGridPositionList(new Vector2Int(x, y), dir);

            bool canBuild = true;

            foreach (Vector2Int gridPosition in gridPositionList)
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

            if (canBuild)
            {
                Vector2Int offset = selectedItem.itemSO.GetRotationOffset(dir);
                Vector3 worldPos = inv.GetWorldPosition(x, y) + new Vector3(offset.x, offset.y, -2f) * inv.cellSize;

                GameObject placedObj = Instantiate(
                    selectedItem.itemSO.prefab,
                    worldPos,
                    Quaternion.Euler(0f, 0f, selectedItem.itemSO.GetRotationAngle(dir))
                );

                var placedItem = placedObj.AddComponent<PlacedItem>();
                placedObj.AddComponent<PlacedItemDebug>();
                placedItem.itemSO = selectedItem.itemSO;
                placedItem.inventory = inv;
                placedItem.placedDir = dir;

                List<Vector2Int> occupied = selectedItem.itemSO.GetGridPositionList(new Vector2Int(x, y), dir);
                placedItem.occupiedCells = new List<Vector2Int>(occupied);

                foreach (Vector2Int pos in occupied)
                {
                    inv.SetCellItem(pos.x, pos.y, placedItem);
                    inv.cells[pos.x, pos.y].DisableCell();
                }

                selectedItem.amount--;

                if (selectedItem.amount <= 0)
                {
                    itemsToPlace.Remove(selectedItem);
                    selectedItem.DeleteItem();
                    DeselectItem();
                }
                else
                {
                    selectedItem.SetAmount(selectedItem.amount);
                    DeselectItem();
                }

                break;
            }
        }
    }

    public void DeselectItem()
    {
        if (selectedItem != null)
            Debug.Log("Deselected " + selectedItem.name);

        if (invisItem) Destroy(invisItem);
        selectedItem = null;
    }

    public void SelectItem(SelectableItem item)
    {
        Debug.Log("Selected " + item.itemSO.name);

        if (selectedItem != null && selectedItem != item)
        {
            // Do not destroy existing UI
            DeselectItem();
        }

        selectedItem = item;
        Debug.Log($"[Select] Selected {item.itemSO.name}");

        if (invisItem) Destroy(invisItem);
        dir = Dir.Horizontal;

        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = -5f;

        Vector2Int size = GetRotatedSize(selectedItem.itemSO, dir);
        Vector3 offset = new Vector3(
            -(size.x * 0.5f - 0.5f),
            -(size.y * 0.5f - 0.5f),
            0f
        ) * 3;

        invisItem = Instantiate(
            item.itemSO.prefab,
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
        if (!invisItem || selectedItem == null) return;

        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = -5f;

        if (TryGetSnappingPosition(pos, out Vector3 snappedWorldPos, out Quaternion snappedRotation))
        {
            invisTargetPos = snappedWorldPos;
            invisTargetRot = snappedRotation;
        }
        else
        {
            Vector2Int size = GetRotatedSize(selectedItem.itemSO, dir);
            Vector3 offset = new Vector3(
                -(size.x * 0.5f - 0.5f),
                -(size.y * 0.5f - 0.5f),
                0f
            ) * 3;

            invisTargetPos = pos + offset;
            invisTargetRot = Quaternion.Euler(0f, 0f, selectedItem.itemSO.GetRotationAngle(dir));
        }
    }

    public static Vector2Int GetRotatedSize(ItemSO item, Dir dir)
    {
        return dir == Dir.Horizontal
            ? new Vector2Int(item.width, item.height)
            : new Vector2Int(item.height, item.width);
    }

    public void TryPickupFromCell()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        Debug.Log($"[Pickup] Mouse World Pos: {mouseWorldPos}");

        foreach (Inventory inv in legalInventories)
        {
            inv.GetXY(mouseWorldPos, out int x, out int y);
            Debug.Log($"[Pickup] Grid Pos in Inventory: {x}, {y}");

            if (x < 0 || y < 0 || x >= inv.sizeColumns || y >= inv.sizeRows)
            {
                Debug.Log("[Pickup] Clicked outside grid bounds.");
                continue;
            }

            InventoryCell cell = inv.cells[x, y];

            Debug.Log($"[Pickup] Cell Debug: inv={inv.name}, x={x}, y={y}, isAvailable={cell.isAvailable}, item={(cell.item != null ? cell.item.name : "null")}");

            if (cell.item == null)
            {
                Debug.Log("[Pickup] No item in this cell.");
                continue;
            }

            PlacedItem placedItem = cell.item;
            Debug.Log($"[Pickup] Found PlacedItem: {placedItem.itemSO.name}");

            // Clear all occupied cells
            foreach (Vector2Int pos in placedItem.occupiedCells)
            {
                Debug.Log($"[Pickup] Clearing cell: {pos}");
                placedItem.inventory.cells[pos.x, pos.y].item = null;
                placedItem.inventory.cells[pos.x, pos.y].EnableCell();
            }

            Destroy(placedItem.gameObject);

            // Return item to UI or increase stack
            SelectableItem existing = FindUIItemFor(placedItem.itemSO);
            if (existing != null)
            {
                existing.amount++;
                existing.SetAmount(existing.amount);
                SelectItem(existing);
            }
            else
            {
                GameObject obj = Instantiate(selectableItemPrefab, listParent.transform);
                SelectableItem newItem = obj.GetComponent<SelectableItem>();
                newItem.itemSO = placedItem.itemSO;
                newItem.amount = 1;
                newItem.SetItem();
                itemsToPlace.Add(newItem);
                SelectItem(newItem);
            }

            Debug.Log("[Pickup] Pickup complete.");
            return;
        }

        Debug.Log("[Pickup] No valid item found at clicked position.");
    }

    public bool TryGetSnappingPosition(Vector3 mouseWorldPos, out Vector3 snappedWorldPos, out Quaternion snappedRotation)
    {
        if (selectedItem == null)
        {
            snappedWorldPos = Vector3.zero;
            snappedRotation = Quaternion.identity;
            return false;
        }

        foreach (Inventory inv in legalInventories)
        {
            inv.GetXY(mouseWorldPos, out int x, out int y);

            if (x < 0 || y < 0 || x >= inv.sizeColumns || y >= inv.sizeRows) continue;

            Vector2Int baseGrid = new Vector2Int(x, y);
            List<Vector2Int> gridPosList = selectedItem.itemSO.GetGridPositionList(baseGrid, dir);

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
                Vector2Int offset = selectedItem.itemSO.GetRotationOffset(dir);
                snappedWorldPos = inv.GetWorldPosition(x, y) + new Vector3(offset.x, offset.y, -2f) * inv.cellSize;
                snappedRotation = Quaternion.Euler(0f, 0f, selectedItem.itemSO.GetRotationAngle(dir));
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
        if (Input.GetMouseButtonDown(1))
        {
            // Right-click = cancel if we have something selected
            if (selectedItem != null)
            {
                Debug.Log("Cancelled selection");

                if (!itemsToPlace.Contains(selectedItem))
                {
                    itemsToPlace.Add(selectedItem);
                    selectedItem.SetAmount(1); // or restore original amount if you store it
                }

                DeselectItem();
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && selectedItem != null)
        {
            //dir = ItemSO.GetNextDir(dir);
            RotateItem();
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (selectedItem != null)
            {
                PlaceItem();
            }
            else
            {
                TryPickupFromCell();
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
