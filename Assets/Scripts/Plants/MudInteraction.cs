using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class MudInteraction : MonoBehaviour
{
    private BoxCollider boxCollider;

    // Evento opcional para acciones adicionales al plantar
    public UnityEvent<Item> OnPlant;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true; // Asegúrate que el collider es un trigger
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
        if (plantItem != null && plantItem.prefab != null && plantItem.prefab.GetComponent<PlantGrowth>() != null)
        {
            // Instancia el prefab de la planta en la posición del barro
            Instantiate(plantItem.prefab, transform.position, Quaternion.identity);
            Debug.Log($"Plantaste: {plantItem.itemName}");
            OnPlant?.Invoke(plantItem);
        }
        else
        {
            Debug.LogWarning("No se puede plantar: Item o prefab nulo.");
        }
    }
}
