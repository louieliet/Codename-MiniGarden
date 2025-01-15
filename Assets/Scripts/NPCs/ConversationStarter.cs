using UnityEngine;
using DialogueEditor;

public class ConversationStarter : MonoBehaviour
{
    public NPCConversation npcConversation;
    [SerializeField] private GameObject warningObject;

    private void Start()
    {
        warningObject.SetActive(false);
    }

    public void StartConversation()
    {
        ConversationManager.Instance.StartConversation(npcConversation);
    }

    public void StopConversation()
    {
        ConversationManager.Instance.EndConversation();
    }

    public void ShowWarning()
    {
        warningObject.SetActive(true);
    }

    public void HideWarning()
    {
        warningObject.SetActive(false);
    }

    public void SetConversation(NPCConversation conversation)
    {
        npcConversation = conversation;
    }
}
