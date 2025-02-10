using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlantGrowth : MonoBehaviour
{
    [Header("Configuración de crecimiento")]
    public GrowthPhase[] growthPhases; // Lista de fases configurables
    public Vector3 growthPerPhase = new Vector3(0, 5, 0); // Incremento de escala por fase
    public MudInteraction originMud; // Referencia al barro de origen

    [Header("Eventos")]
    public UnityEvent onHarvested; // Evento disparado al cosechar

    // Variables internas
    private Vector3 initialScale;
    private bool isFullyGrown = false;
    private bool isDead = false;
    private bool hasBeenWatered = false; // Bandera que indica si se ha regado en la fase actual

    private void Start()
    {
        initialScale = transform.localScale;
        StartCoroutine(GrowthRoutine());
    }

    private IEnumerator GrowthRoutine()
    {
        // Iteramos por cada fase
        for (int i = 0; i < growthPhases.Length; i++)
        {
            GrowthPhase currentPhase = growthPhases[i];
            currentPhase.onPhaseStart?.Invoke();

            // Si es la fase final, no se requiere riego y se marca como completamente crecida.
            if (i == growthPhases.Length - 1)
            {
                transform.localScale += growthPerPhase;
                currentPhase.onPhaseEnd?.Invoke();
                isFullyGrown = true;
                Debug.Log("La planta ha alcanzado su crecimiento máximo.");
                yield break;
            }
            else
            {
                // Reiniciamos la bandera de riego para la fase actual
                hasBeenWatered = false;
                float timer = 0f;

                // Esperamos hasta que se riegue o se agote el tiempo de la fase
                while (timer < currentPhase.duration && !hasBeenWatered)
                {
                    timer += Time.deltaTime;
                    yield return null;
                }

                // Si no se regó a tiempo, la planta muere
                if (!hasBeenWatered)
                {
                    Die();
                    yield break;
                }

                // Si se regó a tiempo, avanzamos a la siguiente fase
                transform.localScale += growthPerPhase;
                currentPhase.onPhaseEnd?.Invoke();
            }
        }
    }

    /// <summary>
    /// Método que se llama desde el sistema de riego (por ejemplo, al interactuar con la planta).
    /// </summary>
    public void WaterPlant()
    {
        if (isDead || isFullyGrown)
            return;

        hasBeenWatered = true;
        Debug.Log("La planta ha sido regada.");
    }

    /// <summary>
    /// Maneja la muerte de la planta por falta de riego.
    /// </summary>
    private void Die()
    {
        isDead = true;
        Debug.Log("La planta ha muerto por falta de agua.");
        Destroy(gameObject);
        if (originMud != null)
        {
            originMud.SetCanPlant(true);
        }
    }

    /// <summary>
    /// Intenta cosechar la planta. Solo se puede cosechar si está completamente crecida.
    /// </summary>
    /// <returns>True si se cosechó correctamente; false en caso contrario.</returns>
    public bool TryHarvest()
    {
        if (!isFullyGrown)
        {
            Debug.Log("La planta no está completamente crecida.");
            return false;
        }

        // Usamos directamente la fase final
        GrowthPhase finalPhase = growthPhases[growthPhases.Length - 1];
        Debug.Log($"Fase final: {finalPhase.phaseName}, CanBeHarvested: {finalPhase.canBeHarvested}, HarvestItem: {finalPhase.harvestItem}");

        // Verificamos que la fase final permita la cosecha y que se haya configurado el ítem a cosechar
        if (finalPhase.canBeHarvested && finalPhase.harvestItem != null && originMud != null)
        {
            // Liberamos el barro para permitir que se plante en él nuevamente
            originMud.SetCanPlant(true);

            // Agregamos el ítem de cosecha al inventario del jugador
            PlayerInventory.Instance.AddToInventory(finalPhase.harvestItem, finalPhase.harvestQuantity);
            onHarvested?.Invoke();
            Destroy(gameObject);
            return true;
        }

        return false;
    }
}
