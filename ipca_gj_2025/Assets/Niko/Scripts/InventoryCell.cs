using System;
using UnityEngine;

public class InventoryCell
{
    public Inventory inventory;
    public PlacedItem item = null;
    public int cellX;
    public int cellY;
    public bool isAvailable = true;
    public bool isLegal = true;

    public void DisableCell()
    {
        isAvailable = false;
        Debug.Log("Cell " + cellX + ", " + cellY + " from " + inventory.name + " is disabled");
    }

    public void EnableCell()
    {
        isAvailable = true;
        Debug.Log("Cell " + cellX + ", " + cellY + " from " + inventory.name + " is enabled");
    }

    public bool IsItemLegal() => item.itemSO.type != ItemType.Illegal;

    public InventoryCell(int x, int y, Inventory inv)
    {
        inventory = inv;
        cellX = x;
        cellY = y;
        Debug.Log("Cell created at [" + x + ", " + y + "]");
    }
}
