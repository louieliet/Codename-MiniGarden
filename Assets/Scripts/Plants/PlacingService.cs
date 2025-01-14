using UnityEngine;

public class PlacingService : IPlaceSystem
{
    private GridManager gridManager;

    public PlacingService(GridManager gridManager)
    {
        this.gridManager = gridManager;
    }

    public bool Place(Vector3 position, GameObject plantPrefab)
    {
        if (gridManager.PlantAtPosition(position) && plantPrefab.GetComponent<PlantGrowth>() != null)
        {
            Vector3 plantPosition = gridManager.GetWorldPosition(
                Mathf.FloorToInt((position.x / gridManager.cellSize)),
                Mathf.FloorToInt((position.y / gridManager.cellSize)),
                Mathf.FloorToInt((position.z / gridManager.cellSize))
            ) + new Vector3(gridManager.cellSize / 2, 0, gridManager.cellSize / 2);

            Object.Instantiate(plantPrefab, plantPosition, Quaternion.identity);
            Debug.Log("Objeto colocada en " + plantPosition);
            return true;
        }
        else
        {
            Debug.Log("No se pudo colocar el objeto, la celda ya está ocupada.");
        }

        return false;
    }
}
