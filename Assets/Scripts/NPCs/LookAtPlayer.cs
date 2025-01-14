using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    [Tooltip("Referencia al transform del jugador que se desea seguir con la mirada.")]
    public Transform player;

    [Tooltip("Velocidad de suavizado al girar la mirada.")]
    public float rotationSpeed = 5f;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;

        if (player == null)
        {
            Debug.LogWarning("No se ha encontrado un jugador con la etiqueta 'Player' para que el NPC lo siga.");
        }
    }

    private void Update()
    {
        if (player == null) return;

        // Dirección desde el NPC hacia el jugador
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0;
        // Opcional: Si deseas que el NPC incline la cabeza para mirar hacia arriba/abajo, 
        // elimina la línea anterior que anula la componente Y.

        if (directionToPlayer == Vector3.zero) return;

        // Calcular la rotación deseada para mirar al jugador
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

        // Suavizar la rotación para una transición más natural
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
