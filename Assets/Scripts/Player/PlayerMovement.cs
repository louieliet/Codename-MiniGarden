using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float gravity = -9.81f;

    private Rigidbody rb;
    private Vector3 velocity;
    private Vector2 inputMovement;
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Pequeña fuerza hacia abajo para mantener al jugador pegado al suelo
        }

        // Movimiento del personaje
        Vector3 move = new Vector3(inputMovement.x, 0, inputMovement.y);
        rb.MovePosition(rb.position + move * moveSpeed * Time.deltaTime);
    }

    // Input del movimiento
    public void OnMove(InputAction.CallbackContext context)
    {
        inputMovement = context.ReadValue<Vector2>();
    }

}
