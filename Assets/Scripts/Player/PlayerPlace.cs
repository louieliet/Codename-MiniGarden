using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPlace : MonoBehaviour
{
    public InventoryUIManager inventoryUIManager;
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

        if (inventoryUIManager == null)
        {
            inventoryUIManager = FindObjectOfType<InventoryUIManager>();
            if (inventoryUIManager == null)
            {
                Debug.LogError("No se encontró InventoryUIManager en la escena.");
                return;
            }
        }

        placeSystem = new PlacingService(gridManager);
    }

    public void OnPlant(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed)
            return;

        var selectedSlot = inventoryUIManager?.GetSelectedSlot();
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

        if (placeSystem != null && plantPrefab != null)
        {
            Debug.Log("Intentando colocar el objeto...");
            Vector3 playerPosition = transform.position;
            Debug.Log($"Posición del jugador: {playerPosition}");

            if (placeSystem.Place(playerPosition, plantPrefab))
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
