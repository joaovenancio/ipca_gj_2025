using System;
using UnityEngine;

public class InventoryCell
{
    public Inventory inventory;
    public ItemSO item = null;
    public int cellX;
    public int cellY;
    public bool isOccupied = false;
    public bool isAvailable = true;

    public void DisableCell()
    {
        isAvailable = false;
        Debug.Log("Cell " + cellX + ", " + cellY + " from " + inventory.name + " is disabled");
    }

    public InventoryCell(int x, int y, Inventory inv)
    {
        inventory = inv;
        cellX = x;
        cellY = y;
        Debug.Log("Cell created at [" + x + ", " + y + "]");
    }
}
