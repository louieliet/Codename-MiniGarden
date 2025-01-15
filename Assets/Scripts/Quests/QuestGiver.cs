using UnityEngine;
using DialogueEditor;
using System.Collections.Generic;
public class QuestGiver : MonoBehaviour
{
    public Quest quest;
    public List<NPCConversation> conversations;
    [SerializeField] int conversationIndex = 0;
    [SerializeField] private ConversationStarter conversationStarter;

    private void Start()
    {
        conversationStarter = GetComponent<ConversationStarter>();
        conversationStarter.SetConversation(conversations[conversationIndex]);
    }

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

    public void ChangeConversation()
    {
        conversationIndex++;
        conversationStarter.SetConversation(conversations[conversationIndex]);
    }

    public void FinalConversation()
    {
        conversationStarter.SetConversation(conversations[conversations.Count - 1]);
    }
}
