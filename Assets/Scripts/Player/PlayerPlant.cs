using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerPlant : MonoBehaviour
{
    public GameManager gameManager;
    public InventoryUIManager inventoryUIManager;
    [HideInInspector] public MudInteraction currentMud;

    public void OnPlantAction(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed)
            return;

        var selectedSlot = inventoryUIManager?.GetSelectedSlot();
        if (selectedSlot == null || selectedSlot.item == null)
        {
            Debug.LogWarning("No hay un ítem seleccionado para plantar.");
            return;
        }

        Item selectedItem = selectedSlot.item;

        if (selectedItem.itemType != ItemType.Plant || currentMud == null) return;

        currentMud.Plant(selectedItem);
        inventoryUIManager.playerInventory.RemoveItem(selectedItem, 1);

    }
}
