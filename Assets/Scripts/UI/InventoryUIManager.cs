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
        playerInventory.selectedSlot = slot;
        Debug.Log($"Has seleccionado: {slot.item.itemName}");
    }

}
