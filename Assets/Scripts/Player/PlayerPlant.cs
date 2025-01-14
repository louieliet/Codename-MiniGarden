using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPlace : MonoBehaviour
{
    public GameObject plantPrefab;

    private IPlaceSystem placeSystem;

    private void Start()
    {
        GridManager gridManager = FindObjectOfType<GridManager>();
        if (gridManager == null)
        {
            Debug.LogError("No se encontró GridManager en la escena.");
            return;
        }

        placeSystem = new PlacingService(gridManager);
    }

    public void OnPlant(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed)
            return;

        if (placeSystem != null)
        {
            Debug.Log("Intentando colocar el objeto...");
            Vector3 playerPosition = transform.position;
            Debug.Log($"Posición del jugador: {playerPosition}");

            if (placeSystem.Place(playerPosition, plantPrefab))
            {
                Debug.Log("Objeto colocada con éxito.");
            }
            else
            {
                Debug.Log("No se pudo colocar el objeto.");
            }
        }
        else
        {
            Debug.LogError("El sistema de colocación no está configurado.");
        }
    }


}
