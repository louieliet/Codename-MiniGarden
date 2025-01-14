using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public Inventory inventory; // Referencia al ScriptableObject de inventario

    public void AddToInventory(Item item, int quantity)
    {
        if (inventory.AddItem(item, quantity))
        {
            Debug.Log($"Se agregó {quantity} {item.itemName} al inventario.");
        }
        else
        {
            Debug.Log("No se pudo agregar el objeto al inventario.");
        }
    }

    public void RemoveFromInventory(Item item, int quantity)
    {
        if (inventory.RemoveItem(item, quantity))
        {
            Debug.Log($"Se eliminó {quantity} {item.itemName} del inventario.");
        }
        else
        {
            Debug.Log("No se pudo eliminar el objeto del inventario.");
        }
    }
}
