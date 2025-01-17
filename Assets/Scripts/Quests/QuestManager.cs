using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; } // Singleton para el acceso global
    public List<Quest> activeQuests = new List<Quest>();      // Lista de misiones activas
    public List<Quest> allQuests = new List<Quest>();      // Lista de misiones activas

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        ResetQuests();
    }

    public void AddQuest(Quest quest)
    {
        if (!activeQuests.Contains(quest) && !quest.isAssigned)
        {
            quest.isAssigned = true;
            activeQuests.Add(quest);
            Debug.Log($"Nueva misión añadida: {quest.questName}");
        }
        else
        {
            Debug.LogWarning($"La misión {quest.questName} ya está activa.");
        }
    }

    public void CheckQuestCompletion(Item item, int quantity)
    {
        foreach (var quest in activeQuests)
        {
            if (!quest.isCompleted && quest.requiredItem == item && quantity >= quest.requiredQuantity)
            {
                Debug.Log($"Completaste la misión: {quest.questName}");
                quest.CompleteQuest();
                // Opcional: Recompensar al jugador
                RewardPlayer(quest.rewardAmount);
            }
        }
    }

    private void RewardPlayer(int rewardAmount)
    {
        Debug.Log($"Recompensa recibida: {rewardAmount}");
        // Aquí puedes agregar lógica para otorgar la recompensa al jugador (dinero, experiencia, etc.).
    }

    private void ResetQuests()
    {
        foreach (var quest in allQuests)
        {
            quest.isCompleted = false;
            quest.isAssigned = false;
        }
        Debug.Log("Misiones reiniciadas.");
    }
}
