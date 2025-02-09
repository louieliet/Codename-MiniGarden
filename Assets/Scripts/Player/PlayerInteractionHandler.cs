using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractionHandler : MonoBehaviour
{
    [SerializeField] private LayerMask interactionLayerMask; // Capa para objetos interactuables
    [SerializeField] private float interactionRadius = 2f;   // Radio de la esfera de interacción
    public bool debugMode = false;

    // Aquí guardamos la referencia al interactable más cercano
    private IInteractable currentInteractable;

    private void Update()
    {
        // Detectar colisiones dentro de la esfera
        Collider[] hits = Physics.OverlapSphere(transform.position, interactionRadius, interactionLayerMask);

        // Buscar el interactable más cercano
        IInteractable closestInteractable = null;
        float closestDistance = Mathf.Infinity;

        foreach (var hitCollider in hits)
        {
            float distance = Vector3.Distance(transform.position, hitCollider.transform.position);

            // Comprobar si implementa IInteractable
            if (hitCollider.TryGetComponent(out IInteractable possibleInteractable))
            {
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestInteractable = possibleInteractable;
                }
            }
        }

        // Si encontramos un interactable diferente al actual
        if (closestInteractable != currentInteractable)
        {
            // Ocultar advertencia del anterior (si existía)
            currentInteractable?.HideWarning();

            // Actualizar referencia
            currentInteractable = closestInteractable;

            // Mostrar advertencia del nuevo (si existe)
            currentInteractable?.ShowWarning();
        }

        // Si el jugador se aleja demasiado
        if (currentInteractable != null)
        {
            // Accedemos al transform a través de cast a MonoBehaviour
            // porque IInteractable en sí no hereda de MonoBehaviour
            float distance = Vector3.Distance(
                transform.position,
                ((MonoBehaviour)currentInteractable).transform.position
            );

            if (distance > interactionRadius)
            {
                currentInteractable.HideWarning();
                currentInteractable = null;  // Limpiar referencia
            }
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed) return;

        // Realiza un raycast desde la cámara usando la posición del mouse.
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, interactionLayerMask))
        {
            // Comprueba si el objeto golpeado implementa IInteractable
            if (hit.collider.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                // Solo interactúa si el interactable golpeado es el mismo que el currentInteractable (es decir, el que está en rango)
                if (interactable == currentInteractable)
                {
                    currentInteractable.Interact();
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!debugMode) return;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
