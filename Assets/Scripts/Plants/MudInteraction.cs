using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class MudInteraction : MonoBehaviour, IInteractable
{
    #region Fields & Properties

    [Header("Configuración de Plantación")]
    [SerializeField] private bool canPlant = true; // Indica si el terreno está libre para plantar
    private BoxCollider boxCollider;

    // Almacena la planta instanciada en este barro (si existe)
    private PlantGrowth currentPlant;

    // Evento opcional para acciones adicionales al plantar (por ejemplo, reproducir sonido o actualizar UI)
    public UnityEvent<Item> OnPlant;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true; // Asegurarse de que el collider sea un trigger
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShowWarning();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HideWarning();
        }
    }

    #endregion

    #region IInteractable Implementation

    /// <summary>
    /// Al interactuar, se verifica el estado del barro:
    /// - Si está libre (canPlant es true), se intenta plantar una semilla.
    /// - Si ya hay una planta (canPlant es false), se delega la interacción al componente PlantInteraction de la planta.
    /// </summary>
    public void Interact()
    {
        if (canPlant)
        {
            TryPlant();
        }
        else
        {
            // Delegamos la interacción a la planta instanciada.
            if (currentPlant != null)
            {
                PlantInteraction plantInteraction = currentPlant.GetComponent<PlantInteraction>();
                if (plantInteraction != null)
                {
                    plantInteraction.Interact();
                }
                else
                {
                    Debug.LogWarning("La planta no tiene componente PlantInteraction.");
                }
            }
            else
            {
                Debug.LogWarning("No hay una planta asignada para interactuar.");
            }
        }
    }

    public void ShowWarning()
    {
        // Aquí puedes activar la UI (por ejemplo, "Pulsa E para interactuar")
    }

    public void HideWarning()
    {
        // Aquí puedes desactivar la UI.
    }

    #endregion

    #region Planting Logic

    /// <summary>
    /// Intenta obtener el ítem seleccionado del inventario y plantarlo en el barro.
    /// </summary>
    private void TryPlant()
    {
        var inventory = PlayerInventory.Instance.GetInventory();
        var selectedSlot = inventory.selectedSlot;

        if (selectedSlot == null || selectedSlot.item == null)
        {
            Debug.LogWarning("No hay un ítem seleccionado para plantar.");
            return;
        }

        Item selectedItem = selectedSlot.item;
        int selectedQuantity = selectedSlot.quantity;

        // Validar que el ítem seleccionado sea del tipo Plant y que haya cantidad disponible.
        if (selectedItem.itemType != ItemType.Plant || selectedQuantity <= 0)
        {
            Debug.Log("El ítem seleccionado no es una semilla o no hay suficiente cantidad.");
            return;
        }

        // Si se planta correctamente, se remueve una unidad del inventario.
        if (Plant(selectedItem))
        {
            inventory.RemoveItem(selectedItem, 1);
        }
    }

    /// <summary>
    /// Instancia la planta a partir del prefab del ítem y la asigna a este barro.
    /// </summary>
    /// <param name="plantItem">El ítem de planta a sembrar.</param>
    /// <returns>True si la plantación fue exitosa, false en caso contrario.</returns>
    public bool Plant(Item plantItem)
    {
        if (!canPlant)
        {
            Debug.LogWarning("No se puede plantar en este momento; el terreno ya está ocupado.");
            return false;
        }

        if (plantItem == null || plantItem.prefab == null)
        {
            Debug.LogWarning("No se puede plantar: el ítem o su prefab son nulos.");
            return false;
        }

        // Instanciar la planta en la posición actual del barro.
        GameObject newPlant = Instantiate(plantItem.prefab, transform.position, Quaternion.identity);
        PlantGrowth plantGrowth = newPlant.GetComponent<PlantGrowth>();
        if (plantGrowth == null)
        {
            Debug.LogWarning("El prefab de la planta no contiene el script PlantGrowth.");
            Destroy(newPlant);
            return false;
        }

        // Asignar este barro como origen para que la planta pueda liberar el espacio al morir o cosecharse.
        plantGrowth.originMud = this;

        // Guardar la referencia a la planta instanciada para interactuar posteriormente.
        currentPlant = plantGrowth;

        // Marcar que ya no se puede plantar en este barro hasta que se libere nuevamente.
        SetCanPlant(false);

        Debug.Log($"Plantaste: {plantItem.itemName}");
        OnPlant?.Invoke(plantItem);

        return true;
    }

    /// <summary>
    /// Permite establecer si se puede o no plantar en este barro.
    /// </summary>
    /// <param name="value">True si se puede plantar; false si no.</param>
    public void SetCanPlant(bool value)
    {
        canPlant = value;
    }

    #endregion

    #region Watering Logic
    // La lógica de riego se maneja ahora desde el componente PlantInteraction de la planta.
    // Por ello, MudInteraction no requiere un método TryWaterPlant propio.
    #endregion
}
