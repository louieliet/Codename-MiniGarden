using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class MudInteraction : MonoBehaviour
{
    [SerializeField] private bool canPlant = true;
    private BoxCollider boxCollider;

    // Evento opcional para acciones adicionales al plantar
    public UnityEvent<Item> OnPlant;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true; // Asegúrate de que el collider sea un trigger
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("El jugador ha entrado en contacto con el barro.");

            // Asignamos este área de barro al jugador para permitirle plantar aquí
            PlayerPlant player = other.GetComponent<PlayerPlant>();
            if (player != null)
            {
                player.currentMud = this;
                // Aquí podrías mostrar en la UI un mensaje indicando "Presiona E para plantar"
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("El jugador ha salido del área de barro.");
            PlayerPlant player = other.GetComponent<PlayerPlant>();
            if (player != null && player.currentMud == this)
            {
                player.currentMud = null;
                // Oculta el mensaje de plantado en la UI si lo habías mostrado
            }
        }
    }

    public void Plant(Item plantItem)
    {
        // Primero, validamos si se puede plantar
        if (!canPlant)
        {
            Debug.LogWarning("No se puede plantar en este momento, esta tierrita está ocupada.");
            return;
        }

        // Validamos el item y su prefab
        if (plantItem == null || plantItem.prefab == null)
        {
            Debug.LogWarning("No se puede plantar: Item o prefab nulo.");
            return;
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
            return;
        }

        // Asignamos el mud de origen en la instancia
        plantGrowth.originMud = this;

        // Marcamos que ya no se puede plantar hasta que se libere de nuevo
        SetCanPlant(false);

        Debug.Log($"Plantaste: {plantItem.itemName}");

        // Disparamos el evento opcional si deseas realizar otras acciones
        OnPlant?.Invoke(plantItem);
    }

    public void SetCanPlant(bool value)
    {
        canPlant = value;
    }
}
