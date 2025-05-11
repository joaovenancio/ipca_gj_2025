using System.Collections.Generic;
using UnityEngine;

public static class LevelEndChecker
{
    public static bool CheckPlanet1(List<Inventory> inventories)
    {
        foreach (var inv in inventories)
        {
            if (inv.section == InventorySection.Middle || inv.section == InventorySection.PreCockpit || inv.section == InventorySection.Cargo)
            {
                for (int x = 0; x < inv.sizeColumns; x++)
                {
                    for (int y = 2; y <= 3; y++) // rows 3 and 4 (0-based index)
                    {
                        if (IsIllegal(inv, x, y)) return false;
                    }
                }
            }

            if (inv.section == InventorySection.Cockpit)
            {
                for (int x = 0; x < inv.sizeColumns; x++)
                {
                    for (int y = 0; y < inv.sizeRows; y++)
                    {
                        if (IsIllegal(inv, x, y)) return false;
                    }
                }
            }

            if (inv.section == InventorySection.Wings || inv.section == InventorySection.Middle)
            {
                for (int x = 0; x < inv.sizeColumns; x++)
                {
                    for (int y = 0; y < inv.sizeRows; y++)
                    {
                        if (IsIllegal(inv, x, y)) return false;
                    }
                }
            }
        }
        return true;
    }

    public static bool CheckPlanet2(List<Inventory> inventories)
    {
        foreach (var inv in inventories)
        {
            if (inv.section == InventorySection.Cargo || inv.section == InventorySection.Cockpit)
            {
                for (int x = 0; x < inv.sizeColumns; x++)
                {
                    for (int y = 0; y < inv.sizeRows; y++)
                    {
                        if (IsIllegal(inv, x, y)) return false;
                    }
                }
            }
            else
            {
                for (int x = 0; x < inv.sizeColumns; x++)
                {
                    if (x % 2 == 0) // even index = odd column (1-based)
                    {
                        for (int y = 0; y < inv.sizeRows; y++)
                        {
                            if (IsIllegal(inv, x, y)) return false;
                        }
                    }
                }
            }
        }
        return true;
    }

    public static bool CheckPlanet3(List<Inventory> inventories)
    {
        foreach (var inv in inventories)
        {
            if (inv.section == InventorySection.Cargo || inv.section == InventorySection.Wings)
            {
                for (int x = 0; x < inv.sizeColumns; x++)
                {
                    for (int y = 0; y < inv.sizeRows; y++)
                    {
                        if (IsIllegal(inv, x, y)) return false;
                    }
                }
            }

            for (int x = 0; x < inv.sizeColumns; x++)
            {
                if (inv.sizeRows > 1 && IsIllegal(inv, x, 1)) return false; // row 2
                if (inv.sizeRows > 4 && IsIllegal(inv, x, 4)) return false; // row 5
            }
        }
        return true;
    }

    private static bool IsIllegal(Inventory inv, int x, int y)
    {
        if (x < 0 || y < 0 || x >= inv.sizeColumns || y >= inv.sizeRows) return false;
        var cell = inv.cells[x, y];
        return cell.item != null && cell.item.itemSO.type == ItemType.Illegal;
    }
}
