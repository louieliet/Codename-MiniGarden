using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractionHandler : MonoBehaviour
{
    [SerializeField] private LayerMask interactionLayerMask; // Capa para objetos interactuables
    [SerializeField] private float interactionRadius = 2f;   // Radio de la esfera de interacción
    public bool debugMode = false;

    private ConversationStarter currentConversationStarter;

    private void Update()
    {
        // Detectar colisiones dentro de la esfera
        Collider[] hits = Physics.OverlapSphere(transform.position, interactionRadius, interactionLayerMask);

        // Encontrar el objeto interactuable más cercano
        ConversationStarter closestConversationStarter = null;
        float closestDistance = Mathf.Infinity;

        foreach (var hitCollider in hits)
        {
            if (hitCollider.TryGetComponent(out ConversationStarter conversationStarter))
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestConversationStarter = conversationStarter;
                }
            }
        }

        // Comprobar si el NPC más cercano cambia
        if (closestConversationStarter != currentConversationStarter)
        {
            if (currentConversationStarter != null)
            {
                currentConversationStarter.HideWarning(); // Ocultar advertencia del NPC anterior
                currentConversationStarter.StopConversation(); // Cerrar conversación si está activa
            }

            currentConversationStarter = closestConversationStarter;

            if (currentConversationStarter != null)
            {
                currentConversationStarter.ShowWarning(); // Mostrar advertencia del NPC más cercano
            }
        }

        // Verificar si el jugador se aleja demasiado del NPC actual
        if (currentConversationStarter != null)
        {
            float distance = Vector3.Distance(transform.position, currentConversationStarter.transform.position);
            if (distance > interactionRadius)
            {
                currentConversationStarter.HideWarning();  // Ocultar advertencia al salir del rango
                currentConversationStarter.StopConversation(); // Cerrar conversación si está activa
                currentConversationStarter = null;         // Limpiar la referencia actual
            }
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed) return;

        // Iniciar conversación con el NPC más cercano si está en rango
        if (currentConversationStarter != null)
        {
            currentConversationStarter.StartConversation();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!debugMode) return;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
