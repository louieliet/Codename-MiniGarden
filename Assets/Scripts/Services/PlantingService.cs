using UnityEngine;

public class PlantingService : IPlantSystem
{
    private GridManager gridManager;

    public PlantingService(GridManager gridManager)
    {
        this.gridManager = gridManager;
    }

    public bool Plant(Vector3 position, GameObject plantPrefab)
    {
        if (gridManager.PlantAtPosition(position))
        {
            Vector3 plantPosition = gridManager.GetWorldPosition(
                Mathf.FloorToInt(position.x / gridManager.cellSize),
                Mathf.FloorToInt(position.y / gridManager.cellSize),
                Mathf.FloorToInt(position.z / gridManager.cellSize)
            );

            Object.Instantiate(plantPrefab, plantPosition, Quaternion.identity);
            Debug.Log("Planta colocada en " + plantPosition);
            return true;
        }
        else
        {
            Debug.Log("No se pudo plantar, la celda ya está ocupada.");
        }

        return false;
    }
}
