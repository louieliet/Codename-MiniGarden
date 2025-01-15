using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractionHandler : MonoBehaviour
{
    [SerializeField] private LayerMask interactionLayerMask; // Capa para objetos interactuables
    [SerializeField] private float interactionRadius = 2f;   // Radio de la esfera de interacción
    public bool debugMode = false;

    private ConversationStarter currentConversationStarter;
    private QuestItemHandler currentQuestItemHandler;

    private void Update()
    {
        // Detectar colisiones dentro de la esfera
        Collider[] hits = Physics.OverlapSphere(transform.position, interactionRadius, interactionLayerMask);

        // Buscar el NPC o el objeto interactuable más cercano
        ConversationStarter closestConversationStarter = null;
        QuestItemHandler closestQuestItemHandler = null;
        float closestDistance = Mathf.Infinity;

        foreach (var hitCollider in hits)
        {
            float distance = Vector3.Distance(transform.position, hitCollider.transform.position);

            // Comprobar si es un NPC
            if (hitCollider.TryGetComponent(out ConversationStarter conversationStarter))
            {
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestConversationStarter = conversationStarter;
                }
            }

            // Comprobar si es un objeto de quest
            if (hitCollider.TryGetComponent(out QuestItemHandler questItemHandler))
            {
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestQuestItemHandler = questItemHandler;
                }
            }
        }

        // Manejar el NPC más cercano
        if (closestConversationStarter != currentConversationStarter)
        {
            currentConversationStarter?.HideWarning(); // Ocultar advertencia del NPC anterior
            currentConversationStarter?.StopConversation(); // Detener la conversación si está activa
            currentConversationStarter = closestConversationStarter;
            currentConversationStarter?.ShowWarning(); // Mostrar advertencia del NPC más cercano
        }

        // Manejar el objeto de quest más cercano
        if (closestQuestItemHandler != currentQuestItemHandler)
        {
            currentQuestItemHandler?.HideWarning();
            currentQuestItemHandler = closestQuestItemHandler;
            currentQuestItemHandler?.ShowWarning();
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

        // Interactuar con el NPC más cercano
        if (currentConversationStarter != null)
        {
            currentConversationStarter.StartConversation();
        }
        // Interactuar con el objeto de quest más cercano
        else if (currentQuestItemHandler != null)
        {
            currentQuestItemHandler.CompleteQuest();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!debugMode) return;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
