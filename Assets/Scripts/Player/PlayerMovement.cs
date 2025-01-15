using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;
    private bool isFrozen = false;
    private Vector2 move, mouseLook, joystickLook;
    private Vector3 rotationTarget;
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

        if (isPc)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(mouseLook);
            if (Physics.Raycast(ray, out hit))
            {
                rotationTarget = hit.point;
            }
            MovePlayerWithAim();
        }
        else
        {
            if (joystickLook.x == 0 && joystickLook.y == 0)
            {
                MovePlayer();
            }
            else
            {
                MovePlayerWithAim();
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        inputMovement = context.ReadValue<Vector2>();
    }

    public void OnMouseLook(InputAction.CallbackContext context)
    {
        mouseLook = context.ReadValue<Vector2>();
    }

    public void OnJoystickLook(InputAction.CallbackContext context)
    {
        joystickLook = context.ReadValue<Vector2>();
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

    public void MovePlayerWithAim()
    {
        if (isPc)
        {
            var lookPos = rotationTarget - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);

            Vector3 aimDirection = new Vector3(rotationTarget.x, 0.0f, rotationTarget.z);

            if (aimDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(aimDirection), 0.15f);
            }
        }
        else
        {
            Vector3 aimDirection = new Vector3(joystickLook.x, 0.0f, joystickLook.y);

            if (aimDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(aimDirection), 0.15f);
            }
        }

        Vector3 movement = new Vector3(move.x, 0.0f, move.y);
        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }

}
