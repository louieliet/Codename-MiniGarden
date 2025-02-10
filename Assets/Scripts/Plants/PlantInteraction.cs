using UnityEngine;

// Se asume que IInteractable está definido en otra parte del proyecto.
public class PlantInteraction : MonoBehaviour, IInteractable
{
    /// <summary>
    /// Al interactuar, se verifica el ítem activo en el inventario:
    /// - Si es el Water Can, se intenta regar la planta.
    /// - En otro caso, se intenta cosechar la planta.
    /// </summary>
    public void Interact()
    {
        PlantGrowth plantGrowth = GetComponent<PlantGrowth>();
        if (plantGrowth == null)
        {
            Debug.LogWarning("Este objeto no tiene un script de crecimiento.");
            return;
        }

        var activeSlot = PlayerInventory.Instance.GetActiveSlot();
        if (activeSlot != null && activeSlot.item != null &&
            (activeSlot.item.itemName.Equals("WaterCan") || activeSlot.item.itemName.Equals("Water Can")))
        {
            Debug.Log("Intentando regar la planta...");
            plantGrowth.WaterPlant();
        }
        else
        {
            Debug.Log("Intentando recolectar la planta...");
            plantGrowth.TryHarvest();
        }
    }

    public void ShowWarning()
    {
        // Aquí puedes activar la UI o un aviso visual (por ejemplo, "Pulsa E para interactuar")
    }

    public void HideWarning()
    {
        // Aquí puedes desactivar la UI o el aviso visual.
    }
}
