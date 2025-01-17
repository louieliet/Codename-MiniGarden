using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private Inventory inventory; // Referencia al ScriptableObject de inventario

    private void Start()
    {
        if (inventory == null)
        {
            Debug.LogError("No se ha asignado un inventario al jugador.");
        }
    }

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

    public Inventory GetInventory()
    {
        return inventory;
    }
}
