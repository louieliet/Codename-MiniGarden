using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlantGrowth : MonoBehaviour
{

    public GrowthPhase[] growthPhases; // Lista de fases configurables
    public Vector3 growthPerPhase = new Vector3(0, 5, 0); // Cambio en el tamaño por fase

    [SerializeField] private int currentPhaseIndex = 0;
    private Vector3 initialScale;
    private bool isFullyGrown = false;

    public UnityEvent onHarvested; // Evento que se dispara al recolectar la planta

    private void Start()
    {
        initialScale = transform.localScale;
        StartCoroutine(GrowthRoutine());
    }

    private IEnumerator GrowthRoutine()
    {
        while (currentPhaseIndex < growthPhases.Length)
        {
            GrowthPhase currentPhase = growthPhases[currentPhaseIndex];

            currentPhase.onPhaseStart?.Invoke();

            float elapsedTime = 0;
            while (elapsedTime < currentPhase.duration)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.localScale += growthPerPhase;

            currentPhase.onPhaseEnd?.Invoke();
            currentPhaseIndex++;
        }

        isFullyGrown = true;
        Debug.Log("La planta ha alcanzado su crecimiento máximo.");
    }

    public bool TryHarvest(PlayerInventory playerInventory)
    {
        if (isFullyGrown)
        {
            GrowthPhase currentPhase = growthPhases[currentPhaseIndex - 1];
            Debug.Log($"Fase actual: {currentPhase.phaseName}, CanBeHarvested: {currentPhase.canBeHarvested}, HarvestItem: {currentPhase.harvestItem}");

            if (currentPhase.canBeHarvested && currentPhase.harvestItem != null)
            {
                playerInventory.AddToInventory(currentPhase.harvestItem, currentPhase.harvestQuantity);
                onHarvested?.Invoke();
                Destroy(gameObject);
                return true;
            }
        }

        Debug.Log("La planta no está lista para ser recolectada.");
        return false;
    }

}
