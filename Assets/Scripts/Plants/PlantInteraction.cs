using UnityEngine;

public class PlantInteraction : MonoBehaviour
{
    public PlayerInventory playerInventory; // Referencia al inventario del jugador

    private void Start()
    {
        playerInventory = GameObject.FindObjectOfType<PlayerInventory>();
    }

    private void OnMouseDown()
    {
        PlantGrowth plantGrowth = GetComponent<PlantGrowth>();
        if (plantGrowth != null)
        {
            if (plantGrowth.TryHarvest(playerInventory))
            {
                Debug.Log("Planta recolectada y añadida al inventario.");
            }
            else
            {
                Debug.Log("La planta no está lista para ser recolectada.");
            }
        }
        else
        {
            Debug.LogWarning("Este objeto no tiene un script de crecimiento.");
        }
    }
}
