using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPlace : MonoBehaviour
{
    [SerializeField] private Inventory playerInventory; // Referencia al inventario del jugador
    private IPlaceSystem placeSystem; // Sistema encargado de colocar objetos

    private void Start()
    {
        // Usamos el GridManager como singleton para inicializar el sistema de colocación
        GridManager gridManager = GridManager.Instance;
        if (gridManager == null)
        {
            Debug.LogError("No se encontró GridManager en la escena.");
            return;
        }

        if (playerInventory == null)
        {
            Debug.LogError("No se encontró el inventario en el jugador.");
            return;
        }

        // Inicializamos el servicio de colocación
        placeSystem = new PlacingService(gridManager);
    }

    // Este método se llama desde el InputSystem
    public void OnPlace(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed)
            return;

        TryPlaceSelectedItem();
    }

    // Método encargado de validar y colocar el ítem seleccionado
    private void TryPlaceSelectedItem()
    {
        // Validamos que exista un slot e ítem seleccionado
        if (playerInventory.selectedSlot == null || playerInventory.selectedSlot.item == null)
        {
            //Debug.LogWarning("No hay un ítem seleccionado para colocar.");
            return;
        }

        Item selectedItem = playerInventory.selectedSlot.item;

        // Verificamos que el ítem sea del tipo herramienta (tool) para colocar barro
        if (selectedItem.itemType != ItemType.Tool)
        {
            //Debug.Log("El ítem seleccionado no es una herramienta para colocar barro.");
            return;
        }

        // Verificamos que el ítem tenga un prefab asociado
        if (selectedItem.prefab == null)
        {
            //Debug.LogWarning("El ítem seleccionado no tiene un prefab asociado.");
            return;
        }

        // Obtenemos la posición actual del jugador para intentar colocar el objeto
        Vector3 playerPosition = transform.position;

        // Delegamos la colocación al sistema correspondiente
        bool placed = placeSystem.Place(playerPosition, selectedItem.prefab);

        if (placed)
        {
            //Debug.Log("Objeto colocado con éxito.");
            // Opcional: aquí puedes actualizar el inventario (por ejemplo, reducir la cantidad o quitar el ítem)
        }
        else
        {
            //Debug.Log("No se pudo colocar el objeto.");
        }
    }
}
