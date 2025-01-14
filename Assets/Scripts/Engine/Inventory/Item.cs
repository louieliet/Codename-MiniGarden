using UnityEngine;

public enum ItemType
{
    Tool,
    Plant
}

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName; // Nombre del objeto
    public Sprite icon;     // Icono del objeto para la interfaz
    public GameObject prefab; // Prefab asociado al objeto
    public string description; // Descripción del objeto
    public int maxStackSize = 99; // Tamaño máximo de pila
    public ItemType itemType; // Tipo de objeto
}
