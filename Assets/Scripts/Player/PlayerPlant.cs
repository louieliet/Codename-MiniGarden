using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPlant : MonoBehaviour
{
    public GameObject plantPrefab;

    private IPlantSystem plantSystem;

    private void Start()
    {
        GridManager gridManager = FindObjectOfType<GridManager>();
        if (gridManager == null)
        {
            Debug.LogError("No se encontró GridManager en la escena.");
            return;
        }

        plantSystem = new PlantingService(gridManager);
    }

    public void OnPlant(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed)
            return;

        if (plantSystem != null)
        {
            Debug.Log("Intentando plantar...");
            Vector3 playerPosition = transform.position;
            Debug.Log($"Posición del jugador: {playerPosition}");

            if (plantSystem.Plant(playerPosition, plantPrefab))
            {
                Debug.Log("Planta colocada con éxito.");
            }
            else
            {
                Debug.Log("No se pudo plantar.");
            }
        }
        else
        {
            Debug.LogError("El sistema de plantación no está configurado.");
        }
    }


}
