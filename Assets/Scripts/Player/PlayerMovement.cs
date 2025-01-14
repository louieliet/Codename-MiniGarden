using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float gravity = -9.81f;
    public float acceleration = 10f;

    private Rigidbody rb;
    private Vector3 velocity;
    private Vector2 inputMovement;
    private Vector3 currentVelocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void Update()
    {
        // Movimiento del personaje
        Vector3 move = new Vector3(inputMovement.x, 0, inputMovement.y);
        rb.MovePosition(rb.position + move * moveSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        Vector3 targetVelocity = new Vector3(inputMovement.x, 0, inputMovement.y) * moveSpeed;
        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
    }

    // Input del movimiento
    public void OnMove(InputAction.CallbackContext context)
    {
        inputMovement = context.ReadValue<Vector2>();
    }

}
