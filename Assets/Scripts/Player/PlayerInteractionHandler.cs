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

        // Mostrar u ocultar la advertencia en función del NPC más cercano
        if (closestConversationStarter != currentConversationStarter)
        {
            currentConversationStarter?.HideWarning();
            currentConversationStarter = closestConversationStarter;
            currentConversationStarter?.ShowWarning();
        }

        // Check if the player is too far from the current conversation starter
        if (currentConversationStarter != null)
        {
            float distance = Vector3.Distance(transform.position, currentConversationStarter.transform.position);
            if (distance > interactionRadius)
            {
                currentConversationStarter.StopConversation();
                currentConversationStarter = null;
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
