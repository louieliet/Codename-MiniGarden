using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class MudInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private bool canPlant = true;
    private BoxCollider boxCollider;

    // Evento opcional para acciones adicionales al plantar
    public UnityEvent<Item> OnPlant;

    // Referencia al inventario del jugador en rango
    public PlayerInventory playerInventory;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true; // Asegúrate de que sea Trigger
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Si el jugador tiene un PlayerInventory, lo almacenamos
            var tempInventory = other.GetComponent<PlayerInventory>();
            if (tempInventory != null)
            {
                playerInventory = tempInventory;
                ShowWarning(); // Opcional: Muestra mensaje "Pulsa E para plantar"
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Si el jugador que sale es el mismo que teníamos asignado, limpiamos la ref.
            var tempInventory = other.GetComponent<PlayerInventory>();
            if (tempInventory == playerInventory)
            {
                playerInventory = null;
                HideWarning(); // Opcional: Oculta mensaje
            }
        }
    }

    // MÉTODOS DE LA INTERFAZ -------------------------------------------------
    public void Interact()
    {
        // Aquí ocurre la magia al presionar el mismo botón de "Interacción" que usas para NPC
        TryPlant();
    }

    public void ShowWarning()
    {
        // Aquí puedes activar un texto en pantalla "Pulsa E para plantar", etc.
        Debug.Log("Mostrar UI de 'Pulsa F para plantar'");
    }

    public void HideWarning()
    {
        // Aquí desactivas el texto
        Debug.Log("Ocultar UI de 'Pulsa F para plantar'");
    }
    // ------------------------------------------------------------------------

    private void TryPlant()
    {
        // Verificamos que haya un inventario de jugador en rango
        if (playerInventory == null)
        {
            Debug.LogWarning("No hay PlayerInventory en rango para plantar.");
            return;
        }

        // Obtenemos el slot seleccionado del inventario
        var selectedSlot = playerInventory.GetInventory().selectedSlot;
        if (selectedSlot == null || selectedSlot.item == null)
        {
            Debug.LogWarning("No hay un ítem seleccionado para plantar.");
            return;
        }

        Item selectedItem = selectedSlot.item;
        int selectedQuantity = selectedSlot.quantity;

        // Validaciones: ¿es una planta? ¿hay cantidad?
        if (selectedItem.itemType != ItemType.Plant || selectedQuantity <= 0)
        {
            Debug.Log("El ítem seleccionado no es una semilla o no hay suficiente cantidad.");
            return;
        }

        // Intentamos plantar
        bool wasPlanted = Plant(selectedItem);
        if (wasPlanted)
        {
            // Si la siembra tuvo éxito, quitamos 1 item del inventario
            playerInventory.GetInventory().RemoveItem(selectedItem, 1);
        }
    }

    /// <summary>
    /// Instancia la planta en este barro.
    /// </summary>
    public bool Plant(Item plantItem)
    {
        // Primero, validamos si se puede plantar
        if (!canPlant)
        {
            Debug.LogWarning("No se puede plantar en este momento, esta tierrita está ocupada.");
            return false;
        }

        // Validamos el item y su prefab
        if (plantItem == null || plantItem.prefab == null)
        {
            Debug.LogWarning("No se puede plantar: Item o prefab nulo.");
            return false;
        }

        // Instanciamos la planta a partir del prefab
        GameObject newPlant = Instantiate(plantItem.prefab, transform.position, Quaternion.identity);

        // Obtenemos el PlantGrowth de la instancia recién creada
        PlantGrowth plantGrowth = newPlant.GetComponent<PlantGrowth>();
        if (plantGrowth == null)
        {
            Debug.LogWarning("No se encontró PlantGrowth en el prefab de la planta.");
            // Opcional: destruir la instancia si el script no existe
            Destroy(newPlant);
            return false;
        }

        // Asignamos el mud de origen en la instancia
        plantGrowth.originMud = this;

        // Marcamos que ya no se puede plantar hasta que se libere de nuevo
        SetCanPlant(false);

        Debug.Log($"Plantaste: {plantItem.itemName}");

        // Disparamos el evento opcional si deseas realizar otras acciones
        OnPlant?.Invoke(plantItem);

        // Si llegaste hasta aquí, la siembra fue exitosa
        return true;
    }

    public void SetCanPlant(bool value)
    {
        canPlant = value;
    }
}
