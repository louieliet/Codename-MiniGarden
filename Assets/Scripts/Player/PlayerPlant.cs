using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPlant : MonoBehaviour
{
    // Referencia al administrador de UI del inventario para obtener el ítem seleccionado
    public InventoryUIManager inventoryUIManager;

    // Referencia al área de barro actual en la que está el jugador
    [HideInInspector] public MudInteraction currentMud;

    // Método vinculado a la acción "Plant" del Input System
    public void OnPlantAction(InputAction.CallbackContext context)
    {
        // Asegurarse que la acción se ejecuta en el momento correcto (al presionar la tecla)
        if (context.performed)
        {
            // Obtener el item activo (seleccionado) del inventario
            var selectedSlot = inventoryUIManager?.GetSelectedSlot();
            if (selectedSlot != null && selectedSlot.item != null)
            {
                // Verificar que el jugador está en un área de barro
                if (currentMud != null)
                {
                    // Llamar al método Plant del área de barro actual con el item seleccionado
                    currentMud.Plant(selectedSlot.item);

                    // Opcional: eliminar la planta del inventario si se desea
                    // inventoryUIManager.playerInventory.RemoveItem(selectedSlot.item, 1);
                }
                else
                {
                    Debug.Log("No estás en un área de barro para plantar.");
                }
            }
            else
            {
                Debug.Log("No hay ninguna planta seleccionada para plantar.");
            }
        }
    }
}
