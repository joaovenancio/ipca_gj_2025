using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(PlacedItem))]
public class PlacedItemDebug : MonoBehaviour
{
    public Color gizmoColor = new Color(1f, 0.5f, 0f, 0.4f); // semi-transparent orange

    private PlacedItem item;

    private void OnDrawGizmos()
    {
        if (item == null) item = GetComponent<PlacedItem>();
        if (item == null || item.occupiedCells == null || item.inventory == null) return;

        Gizmos.color = gizmoColor;

        foreach (Vector2Int gridPos in item.occupiedCells)
        {
            Vector3 worldPos = item.inventory.GetWorldPosition(gridPos.x, gridPos.y);
            Gizmos.DrawCube(worldPos + new Vector3(0.5f, 0.5f, -2f) * item.inventory.cellSize,
                            Vector3.one * item.inventory.cellSize * 0.95f);
        }
    }
}