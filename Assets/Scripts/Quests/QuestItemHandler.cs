using UnityEngine;
using DialogueEditor;

public class QuestItemHandler : MonoBehaviour
{
    [SerializeField] private Quest relatedQuest; // Misión asociada a este objeto
    [SerializeField] private Item requiredItem;  // Ítem necesario para completar la misión
    [SerializeField] private int requiredQuantity = 1; // Cantidad requerida
    public QuestGiver _questGiver;
    public Inventory playerInventory;

    private void Start()
    {
        if (_questGiver == null)
        {
            Debug.LogError("No se ha asignado un QuestGiver para este objeto para:" + gameObject.name);
        }
    }

    public void ShowWarning()
    {
        //Debug.Log("Acércate e interactúa para completar la quest.");
        // Aquí puedes mostrar un indicador visual (como un UI encima del objeto)
    }

    public void HideWarning()
    {
        // Debug.Log("Te alejaste del objeto de quest.");
        // Aquí puedes ocultar el indicador visual
    }

    public void CompleteQuest()
    {
        if (relatedQuest != null && !relatedQuest.isCompleted)
        {
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
        else
        {
            Debug.Log("Esta misión ya fue completada o no está asignada.");
        }
    }
}
