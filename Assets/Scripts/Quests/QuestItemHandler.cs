using UnityEngine;
using DialogueEditor;

public class QuestItemHandler : MonoBehaviour, IInteractable
{
    [SerializeField] private Quest relatedQuest; // Misión asociada a este objeto
    [SerializeField] private Item requiredItem;  // Ítem necesario para completar la misión
    [SerializeField] private int requiredQuantity = 1; // Cantidad requerida
    [SerializeField] private GameObject warningObject; // Objeto visual para mostrar advertencia
    public QuestGiver _questGiver;
    public Inventory playerInventory;

    private void Start()
    {
        playerInventory = FindObjectOfType<PlayerInventory>().GetInventory();

        if (playerInventory == null)
        {
            Debug.LogError("No se ha encontrado el inventario del jugador.");
        }

        if (_questGiver == null)
        {
            Debug.LogError("No se ha asignado un QuestGiver para este objeto para:" + gameObject.name);
        }

    }

    public void ShowWarning()
    {
        // Debug.Log("Te acercaste al objeto de quest.");
        warningObject.SetActive(true);

    }

    public void HideWarning()
    {
        // Debug.Log("Te alejaste del objeto de quest.");
        warningObject.SetActive(false);
    }

    public void Interact()
    {
        CompleteQuest();
    }

    public void CompleteQuest()
    {

        if (relatedQuest == null || relatedQuest.isCompleted || !relatedQuest.isAssigned)
        {
            Debug.Log("Esta misión ya fue completada o no está asignada.");
            return;
        }


        if (playerInventory.RemoveItem(requiredItem, requiredQuantity))
        {
            Debug.Log($"Has completado la misión: {relatedQuest.questName}");
            _questGiver.FinalConversation();
            relatedQuest.CompleteQuest();
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("No tienes los ítems necesarios para completar la misión.");
        }

    }
}
