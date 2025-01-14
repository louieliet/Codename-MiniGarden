using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    [Range(0, 1)] public float smoothTime = 0.3f;
    public Vector3 offset;
    private Vector3 velocity = Vector3.zero;
    public float zoomSpeed = 2.0f;
    [SerializeField] private Vector2 zZoom;
    [SerializeField] private Vector2 yZoom;

    private void Update()
    {
        if (target == null) { Debug.Log("Target is null"); return; }

        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        HandleZoom();
    }

    private void HandleZoom()
    {
        float scrollValue = Mouse.current.scroll.ReadValue().y;
        offset.y = Mathf.Clamp(offset.y - scrollValue * zoomSpeed * Time.deltaTime, yZoom.x, yZoom.y);
        offset.z = Mathf.Clamp(offset.z + scrollValue * zoomSpeed * Time.deltaTime, -zZoom.y, -zZoom.x);
    }
}