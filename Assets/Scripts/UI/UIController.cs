using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerInput))]
public class UIController : MonoBehaviour
{
    [SerializeField] private InventoryUIManager _inventoryUIManager;

    public UnityEvent OnInventoryToggled; // Added UnityEvent

    private void Start()
    {
        _inventoryUIManager = GetComponent<InventoryUIManager>();
    }

    public void ToggleInventory(InputAction.CallbackContext context)
    {
        _inventoryUIManager.ToggleInventory();
        OnInventoryToggled.Invoke(); // Invoke the event
    }
}
