using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Draggable : MonoBehaviour
{
    [Header("Movement")]
    public float dragSpeed = 20f;           
    public float pullSpeed = 5f;            
    public float scrollSensitivity = 1f;    

    [Header("Rotation")]
    public float rotationSpeedDegPerSecond = 180f; 

    // Internals
    private Rigidbody rb;
    private Vector3 offset;
    private float zCoord;
    private Vector3 targetPosition;
    private bool dragging = false;
    private bool rotating = false;
    private bool isMouseOver = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnMouseEnter() => isMouseOver = true;
    void OnMouseExit() => isMouseOver = false;

    void OnMouseDown()
    {
        if (Camera.main == null) return;

        zCoord = Camera.main.WorldToScreenPoint(transform.position).z;
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord;
        offset = transform.position - Camera.main.ScreenToWorldPoint(mousePoint);

        if (Input.GetMouseButton(0))
        {
            dragging = true;
            targetPosition = transform.position;
        }

        if (Input.GetMouseButton(1))
        {
            rotating = true;
        }
    }

    void OnMouseUp()
    {
        if (Input.GetMouseButtonUp(0)) dragging = false;
        if (Input.GetMouseButtonUp(1)) rotating = false;

        if (!Input.GetMouseButton(0) && !Input.GetMouseButton(1))
        {
            dragging = false;
            rotating = false;
        }
    }

    void OnMouseDrag()
    {
        if (!dragging || Camera.main == null) return;

        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord;
        Vector3 desired = Camera.main.ScreenToWorldPoint(mousePoint) + offset;

        targetPosition = Vector3.Lerp(targetPosition, desired, Mathf.Clamp01(dragSpeed * Time.deltaTime));
    }

    void Update()
    {
        if (dragging && Camera.main != null)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scroll) > 1e-5f)
            {
                Vector3 camForward = Camera.main.transform.forward;
                targetPosition += camForward * (scroll * scrollSensitivity * pullSpeed);
            }
        }

        if (!rotating && isMouseOver && Input.GetMouseButtonDown(1))
        {
            rotating = true;
        }
        if (rotating && Input.GetMouseButtonUp(1))
        {
            rotating = false;
        }
    }

    void FixedUpdate()
    {
        if (dragging)
        {
            rb.MovePosition(targetPosition);
        }

        if (rotating)
        {
            float angleThisStep = rotationSpeedDegPerSecond * Time.fixedDeltaTime;

            // Rotate around local Z axis (table “up” axis)
            Quaternion deltaRot = Quaternion.AngleAxis(angleThisStep, transform.forward);
            Quaternion newRot = rb.rotation * deltaRot;

            rb.MoveRotation(newRot);
        }
    }
}
