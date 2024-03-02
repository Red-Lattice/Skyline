using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Initialization")]
    public GameObject parent;

    [Header("Values")]
    public float XSensitivity = 1f;
    public float YSensitivity = 1f;

    private Vector2 currentLook;
    

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        parent = transform.parent.gameObject;
    }

    public Vector2 getCurrentLook()
    {
        return currentLook;
    }

    void Update()
    {
        RotateMainCamera();
    }

    void FixedUpdate()
    {
        currentLook = Vector2.Lerp(currentLook, currentLook, 0.8f);
    }

    void RotateMainCamera()
    {
        Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        mouseInput.x *= XSensitivity;
        mouseInput.y *= YSensitivity;

        currentLook.x += mouseInput.x;
        currentLook.y = Mathf.Clamp(currentLook.y += mouseInput.y, -90, 90);

        transform.localRotation = Quaternion.AngleAxis(-currentLook.y, Vector3.right);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);
        parent.transform.localRotation = Quaternion.Euler(0, currentLook.x, 0);
    }
}
