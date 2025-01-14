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

        if (selectedItem.itemType != ItemType.Plant || selectedQuantity <= 0 || currentMud == null) return;

        currentMud.Plant(selectedItem);
        playerInventory.RemoveItem(selectedItem, 1);

    }
}
