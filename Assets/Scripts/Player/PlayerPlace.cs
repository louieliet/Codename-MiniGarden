using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPlace : MonoBehaviour
{
    public Inventory playerInventory;
    private IPlaceSystem placeSystem;

    private void Start()
    {
        GridManager gridManager = FindObjectOfType<GridManager>();

        if (gridManager == null)
        {
            Debug.LogError("No se encontró GridManager en la escena.");
            return;
        }

        if (playerInventory == null)
        {
            Debug.LogError("No se encontró Inventory en el jugador.");
        }

        placeSystem = new PlacingService(gridManager);
    }

    public void OnPlace(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed)
            return;

        var selectedSlot = playerInventory.selectedSlot;
        if (selectedSlot == null || selectedSlot.item == null)
        {
            Debug.LogWarning("No hay un ítem seleccionado para colocar.");
            return;
        }

        Item selectedItem = selectedSlot.item;

        if (selectedItem.itemType != ItemType.Tool)
        {
            Debug.Log("El ítem seleccionado no es una herramienta para colocar barro.");
            return;
        }

        GameObject objectToPlace = selectedItem.prefab;

        if (placeSystem != null && objectToPlace != null)
        {
            Debug.Log("Intentando colocar el objeto...");
            Vector3 playerPosition = transform.position;
            Debug.Log($"Posición del jugador: {playerPosition}");

            if (placeSystem.Place(playerPosition, objectToPlace))
            {
                Debug.Log("Objeto colocado con éxito.");
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
