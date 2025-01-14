using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class GrowthPhase
{
    public string phaseName;
    public float duration; // Duración de la fase
    public UnityEvent onPhaseStart; // Eventos al inicio de la fase
    public UnityEvent onPhaseEnd;   // Eventos al final de la fase
}