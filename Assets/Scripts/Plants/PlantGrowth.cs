using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlantGrowth : MonoBehaviour
{
    public GrowthPhase[] growthPhases; // Lista de fases configurables
    public Vector3 growthPerPhase = new Vector3(0, 5, 0); // Cambio en el tamaño por fase

    private int currentPhaseIndex = 0;
    private Vector3 initialScale;

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

            // Invocar eventos de inicio de fase
            currentPhase.onPhaseStart?.Invoke();

            float elapsedTime = 0;
            while (elapsedTime < currentPhase.duration)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Escalar la planta al final de la fase
            transform.localScale += growthPerPhase;

            // Invocar eventos de fin de fase
            currentPhase.onPhaseEnd?.Invoke();

            currentPhaseIndex++;
        }

        Debug.Log("La planta ha completado todas las fases de crecimiento.");
    }
}
