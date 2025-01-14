using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUIManager : MonoBehaviour
{
    public GameObject inventoryPanel; // Panel del inventario
    public GameObject inventorySlotPrefab; // Prefab de los slots del inventario
    public Transform inventoryGrid; // Contenedor para los slots
    public Inventory playerInventory; // Referencia al inventario del jugador

    private List<GameObject> slots = new List<GameObject>();

    private Inventory.InventorySlot activeSlot;

    private void Start()
    {
        // Desactiva el panel del inventario al inicio
        inventoryPanel.SetActive(true);
        inventoryPanel.SetActive(false);
        UpdateInventoryUI();
    }

    public void ToggleInventory()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        if (inventoryPanel.activeSelf)
        {
            UpdateInventoryUI();
        }
    }

    public void UpdateInventoryUI()
    {
        // Limpia los slots actuales
        foreach (GameObject slot in slots)
        {
            Destroy(slot);
        }
        slots.Clear();

        // Crea nuevos slots basados en el inventario
        foreach (var slot in playerInventory.slots)
        {
            if (slot.item != null)
            {
                GameObject newSlot = Instantiate(inventorySlotPrefab, inventoryGrid);
                slots.Add(newSlot);

                Image icon = newSlot.transform.GetComponent<Image>();
                icon.sprite = slot.item.icon;

                TMP_Text name = newSlot.transform.GetChild(0).GetComponent<TMP_Text>();
                name.text = slot.quantity.ToString();

                Button button = newSlot.GetComponent<Button>();
                button.onClick.AddListener(() => OnSlotClicked(slot));
            }
        }

    }

    private void OnSlotClicked(Inventory.InventorySlot slot)
    {
        activeSlot = slot;  // Almacenar el slot seleccionado como activo
        Debug.Log($"Has seleccionado: {slot.item.itemName}");
        // Aquí puedes realizar acciones adicionales al seleccionar el ítem
    }

    // Puedes crear métodos para usar el objeto activo más adelante
    public void UsarObjetoActivo()
    {
        if (activeSlot != null)
        {
            // Implementar la lógica para usar el objeto activo
            Debug.Log($"Usando el objeto: {activeSlot.item.itemName}");
            // Por ejemplo, instanciar su prefab en el mundo:
            // Instantiate(activeSlot.item.prefab, posiciónDeseada, rotaciónDeseada);
        }
        else
        {
            Debug.Log("No hay un objeto activo seleccionado.");
        }
    }

    public Inventory.InventorySlot GetSelectedSlot()
    {
        // Retorna el slot activo almacenado cuando el jugador hizo clic en uno
        return activeSlot;  // Asumiendo que almacenaste el slot activo como se explicó anteriormente
    }

}
