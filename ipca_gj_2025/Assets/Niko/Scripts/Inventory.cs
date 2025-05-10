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

    public GameObject InventoryUI;

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
            if(cellX >= sizeRows || cellY >= sizeColumns) continue;
            cells[cellY, cellX].DisableCell();
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
                cells[x, y] = new InventoryCell(y, x, this);
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

    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - transform.position).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - transform.position).y / cellSize);
    }

    private void SetCellLegal(int x, int y, bool isLegal)
    {
        if (x >= sizeRows || y >= sizeColumns) return;
        cells[y, x].isLegal = isLegal;
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
        if (x >= sizeRows || y >= sizeColumns) return false;
        return cells[y, x].isLegal;
    }
}
