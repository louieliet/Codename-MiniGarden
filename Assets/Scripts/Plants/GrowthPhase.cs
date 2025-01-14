using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class GrowthPhase
{
    public string phaseName;
    public float duration; // Duración de la fase
    public UnityEvent onPhaseStart; // Eventos al inicio de la fase
    public UnityEvent onPhaseEnd;   // Eventos al final de la fase
    public bool canBeHarvested;     // Si puede ser recolectado en esta fase
    public Item harvestItem;        // Ítem que se obtiene al recolectar
    public int harvestQuantity = 1; // Cantidad del ítem que se obtiene
}