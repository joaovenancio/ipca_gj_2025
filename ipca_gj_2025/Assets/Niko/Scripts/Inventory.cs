using CodeMonkey.Utils;
using System.Collections.Generic;
using UnityEngine;

public enum InventorySection { Cockpit, PreCockpit, Middle, Cargo, Wings};

public class Inventory : MonoBehaviour
{
    public int sizeRows = 1;
    public int sizeColumns = 1;
    public float cellSize = 10f;
    public InventorySection section;
    public InventoryCell[,] cells;
    public List<string> cellsToDisable = new();

    private void Start()
    {
        SetupInventory();
        DisableCells();
    }

    private void DisableCells()
    {
        foreach(string cell in cellsToDisable)
        {
            int cellX = int.Parse(cell[0].ToString());
            int cellY = int.Parse(cell[1].ToString());
            if(cellX >= sizeColumns || cellY >= sizeRows) continue;
            cells[cellX, cellY].DisableCell();
        }
    }

    public void SetupInventory()
    {
        cells = new InventoryCell[sizeColumns, sizeRows];

        Debug.Log("Setting up inventory of " + sizeRows + " rows and " + sizeColumns + " columns from section "+ section);

        for (int x = 0; x < cells.GetLength(0); x++)
        {
            for (int y = 0; y < cells.GetLength(1); y++)
            {
                cells[x, y] = new InventoryCell(x, y, this);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, sizeRows), GetWorldPosition(sizeColumns, sizeRows), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(sizeColumns, 0), GetWorldPosition(sizeColumns, sizeRows), Color.white, 100f);
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + transform.position;
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GetXY(mousePos, out int x, out int y);

        if (x >= 0 && y >= 0 && x < sizeColumns && y < sizeRows)
        {
            Gizmos.color = Color.red;
            Vector3 world = GetWorldPosition(x, y);
            Gizmos.DrawWireCube(world + new Vector3(0.5f, 0.5f, -2f) * cellSize, Vector3.one * cellSize * 0.95f);
        }
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - transform.position).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - transform.position).y / cellSize);
    }

    public Vector2Int GetXYWorld(Vector2Int gridCoords)
    {
        return new Vector2Int(gridCoords.x, gridCoords.y);
    }

    public void SetCellItem(int x, int y, PlacedItem item)
    {
        if (x >= sizeColumns || y >= sizeRows) return;
        cells[x,y].item = item;
    }

    private void SetCellLegal(int x, int y, bool isLegal)
    {
        if (x >= sizeColumns || y >= sizeRows) return;
        cells[x,y].isLegal = isLegal;
    }
    
    public void SetInventoryLegal(bool isLegal)
    {
        for (int x = 0; x < cells.GetLength(0); x++)
        {
            for (int y = 0; y < cells.GetLength(1); y++)
            {
                SetCellLegal(x, y, isLegal);
            }
        }
    }

    public bool CheckCellLegality(int x, int y)
    {
        if (x >= sizeColumns || y >= sizeRows) return false;
        return cells[x,y].isLegal;
    }
}
