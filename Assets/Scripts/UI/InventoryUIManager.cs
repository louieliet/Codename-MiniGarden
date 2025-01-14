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

    private void Start()
    {
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

                // Configura la imagen del ítem
                Image icon = newSlot.transform.GetComponent<Image>();
                icon.sprite = slot.item.icon;

                // Configura el nombre del ítem
                TMP_Text name = newSlot.transform.GetChild(0).GetComponent<TMP_Text>();
                name.text = slot.item.itemName;

                // Configura el botón
                Button button = newSlot.GetComponent<Button>();
                button.onClick.AddListener(() => OnSlotClicked(slot));
            }
        }

    }

    private void OnSlotClicked(Inventory.InventorySlot slot)
    {
        Debug.Log($"Has seleccionado: {slot.item.itemName}");
        // Aquí puedes implementar acciones como usar o descartar el ítem
    }
}
