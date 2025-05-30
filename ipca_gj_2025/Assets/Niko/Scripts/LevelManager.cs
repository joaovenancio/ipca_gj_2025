using EasyTextEffects;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    public Button validateButton;
    public int currentPlanet = 1; // can be 1, 2, or 3 depending on level

    public Image fader;
    public TMP_Text resultText;

    private void Awake()
    {
        instance = this;
    }

    public void ValidateShip()
    {
        bool passed = false;

        switch (currentPlanet)
        {
            case 1:
                passed = LevelEndChecker.CheckPlanet1(legalInventories);
                break;
            case 2:
                passed = LevelEndChecker.CheckPlanet2(legalInventories);
                break;
            case 3:
                passed = LevelEndChecker.CheckPlanet3(legalInventories);
                break;
            default:
                Debug.LogWarning("Invalid planet number for validation.");
                return;
        }
        StartCoroutine(FadeOutAndShowResult(passed));

        if (passed)
        {
            Debug.Log("<color=green> You passed! The authorities found no illegal cargo.</color>");
        }
        else
        {
            Debug.Log("<color=red> You failed! The authorities found something illegal.</color>");
        }
    }

    private IEnumerator FadeOutAndShowResult(bool passed)
    {
        float duration = 1f;
        float elapsed = 0f;
        Color color = fader.color;

        fader.gameObject.SetActive(true);
        resultText.gameObject.SetActive(false);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsed / duration);
            fader.color = color;
            yield return null;
        }

        resultText.gameObject.SetActive(true);
        resultText.text = passed ? "You passed inspection!" : "Caught with illegal cargo!";
        resultText.GetComponent<TextEffect>()?.Refresh();

        yield return new WaitForSeconds(2f);

        if (passed)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
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
                Vector3 worldPos = inv.GetWorldPosition(x, y) + new Vector3(offset.x, offset.y, -2f) * 3;

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
        pos.z = 0f;

        if (TryGetSnappingPosition(pos, out Vector3 snappedWorldPos, out Quaternion snappedRotation))
        {
            invisTargetPos = snappedWorldPos;
            invisTargetRot = snappedRotation;
        }
        else
        {
            Vector2Int size = GetRotatedSize(selectedItem.itemSO, dir);
            Vector2 centerOffset = new Vector3(
                -(size.x * 0.5f - 0.5f),
                -(size.y * 0.5f - 0.5f),
                0f
            );

            // Rotate the offset based on the actual angle
            float angle = selectedItem.itemSO.GetRotationAngle(dir) * Mathf.Deg2Rad;
            Vector2 rotatedOffset = new Vector2(
                centerOffset.x * Mathf.Cos(angle) - centerOffset.y * Mathf.Sin(angle),
                centerOffset.x * Mathf.Sin(angle) + centerOffset.y * Mathf.Cos(angle)
            );

            invisTargetPos = pos + new Vector3(rotatedOffset.x, rotatedOffset.y, 0f) * 3;
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
        validateButton.interactable = (selectedItem == null && itemsToPlace.Count == 0);

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

        if (Input.GetMouseButtonDown(2) && selectedItem != null)
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
