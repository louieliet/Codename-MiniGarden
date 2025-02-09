using UnityEngine;
using DialogueEditor;
public class ConversationStarter : MonoBehaviour, IInteractable
{
    public NPCConversation npcConversation;
    [SerializeField] private GameObject warningObject;

    private void Start()
    {
        warningObject.SetActive(false);
    }

    // Esta es la función que implementa IInteractable
    public void Interact()
    {
        StartConversation();
    }

    public void StartConversation()
    {
        GameManager.Instance.TogglePlayerFreeze();
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
