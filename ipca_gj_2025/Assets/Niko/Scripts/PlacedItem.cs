using System.Collections.Generic;
using UnityEngine;

public class PlacedItem : MonoBehaviour
{
    public ItemSO itemSO;
    public Dir placedDir;
    public Inventory inventory;
    public List<Vector2Int> occupiedCells = new();
}
