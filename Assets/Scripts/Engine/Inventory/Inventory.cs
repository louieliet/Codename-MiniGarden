using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInventory", menuName = "Inventory/Inventory")]
public class Inventory : ScriptableObject
{
    [System.Serializable]
    public class InventorySlot
    {
        public Item item;  // Referencia al ScriptableObject del ítem
        public int quantity; // Cantidad de este ítem
    }

    public List<InventorySlot> slots = new List<InventorySlot>();

    /// <summary>
    /// Agregar un ítem al inventario.
    /// </summary>
    public bool AddItem(Item item, int quantity = 1)
    {
        // Verificar si el ítem ya está en el inventario
        foreach (var slot in slots)
        {
            if (slot.item == item && slot.quantity < item.maxStackSize)
            {
                int spaceAvailable = item.maxStackSize - slot.quantity;
                int quantityToAdd = Mathf.Min(spaceAvailable, quantity);

                slot.quantity += quantityToAdd;
                quantity -= quantityToAdd;

                if (quantity <= 0)
                    return true; // Todo el ítem fue agregado
            }
        }

        // Si no se puede apilar, crear un nuevo slot
        while (quantity > 0)
        {
            if (slots.Count >= 20) // Máximo slots de inventario (opcional)
            {
                Debug.Log("El inventario está lleno.");
                return false; // No se puede agregar más
            }

            int quantityToAdd = Mathf.Min(item.maxStackSize, quantity);
            slots.Add(new InventorySlot { item = item, quantity = quantityToAdd });
            quantity -= quantityToAdd;
        }

        return true;
    }

    /// <summary>
    /// Remover un ítem del inventario.
    /// </summary>
    public bool RemoveItem(Item item, int quantity = 1)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].item == item)
            {
                if (slots[i].quantity > quantity)
                {
                    slots[i].quantity -= quantity;
                    return true;
                }
                else
                {
                    quantity -= slots[i].quantity;
                    slots.RemoveAt(i);
                    i--;
                    if (quantity <= 0)
                        return true;
                }
            }
        }

        Debug.LogWarning("No hay suficientes elementos para eliminar.");
        return false;
    }
}
