using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.3f;
    public Vector3 offset;
    private Vector3 velocity = Vector3.zero;
    public float zoomSpeed = 2.0f;
    public float zMinZoom = 5.0f;
    public float zMaxZoom = 20.0f;
    public float yMinZoom = 5.0f;
    public float yMaxZoom = 20.0f;

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
        offset.y = Mathf.Clamp(offset.y - scrollValue * zoomSpeed * Time.deltaTime, yMinZoom, yMaxZoom);
        offset.z = Mathf.Clamp(offset.z + scrollValue * zoomSpeed * Time.deltaTime, -zMaxZoom, -zMinZoom);
    }
}