using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Quests/Quest")]
public class Quest : ScriptableObject
{
    public string questName;           // Nombre de la misión
    public string description;         // Descripción de la misión
    public Item requiredItem;          // Ítem requerido para completar la misión
    public int requiredQuantity = 1;   // Cantidad requerida del ítem
    public int rewardAmount = 100;     // Recompensa (por ejemplo, dinero o puntos)
    public bool isCompleted = false;   // Estado de la misión

    public void CompleteQuest()
    {
        isCompleted = true;
        Debug.Log($"Quest completada: {questName}");
    }
}
