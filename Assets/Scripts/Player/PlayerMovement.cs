using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;
    private bool isFrozen = false;
    public bool isPc;

    private Rigidbody rb;
    private Vector2 inputMovement;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public void Update()
    {
        if (isFrozen) return;

        MovePlayer();

    }

    public void OnMove(InputAction.CallbackContext context)
    {
        inputMovement = context.ReadValue<Vector2>();
    }

    public void ToggleFreezeMovement()
    {
        isFrozen = !isFrozen;
    }

    private void MovePlayer()
    {
        Vector3 movement = new Vector3(inputMovement.x, 0, inputMovement.y);

        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);
        }

        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }

}
