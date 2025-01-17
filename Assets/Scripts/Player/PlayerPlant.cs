using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.AI;

public class PlayerPlant : MonoBehaviour
{
    public Inventory playerInventory;
    [HideInInspector] public MudInteraction currentMud;

    private void Start()
    {
        if (playerInventory == null)
        {
            Debug.LogError("No se encontró Inventory en el jugador.");
        }
    }

    public void OnPlantAction(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed)
            return;

        var selectedSlot = playerInventory.selectedSlot;
        if (selectedSlot == null || selectedSlot.item == null)
        {
            Debug.LogWarning("No hay un ítem seleccionado para plantar.");
            return;
        }

        Item selectedItem = selectedSlot.item;
        int selectedQuantity = selectedSlot.quantity;

        // Validaciones básicas
        if (selectedItem.itemType != ItemType.Plant || selectedQuantity <= 0 || currentMud == null)
        {
            return;
        }

        // Llamamos a Plant y guardamos el resultado
        bool wasPlanted = currentMud.Plant(selectedItem);

        // Solo si la siembra fue exitosa, removemos 1 del inventario
        if (wasPlanted)
        {
            playerInventory.RemoveItem(selectedItem, 1);
        }
        else
        {
            Debug.Log("La siembra no fue exitosa, no se descuenta el ítem del inventario.");
        }
    }
}
