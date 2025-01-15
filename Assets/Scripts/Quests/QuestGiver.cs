using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    public Quest quest; // Misión asignada por este NPC

    public void AssignQuest()
    {
        if (quest != null && !quest.isCompleted)
        {
            QuestManager.Instance.AddQuest(quest);
            Debug.Log($"Misión asignada: {quest.questName}");
        }
        else if (quest.isCompleted)
        {
            Debug.Log($"La misión {quest.questName} ya fue completada.");
        }
    }
}
