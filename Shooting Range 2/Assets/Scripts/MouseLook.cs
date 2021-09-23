using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    // Default mouse sensitivity is 200
    float mouseSensitivity = 200f;

    // Playerbody to rotate, and oldmousesens for scoped use
    [SerializeField]
    Transform playerBody;
    float oldMouseSens;

    // Rotation limiting float
    float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
    void Update()
    {

        // Mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Using a float to limit the looking angle up and down
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Set the rotations
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    // Set Mouse Sensitivity
    public void SetMouseSensitivity(float sens)
    {
        mouseSensitivity = sens;
    }
    // Save the current mouse sens, then change it for scoped use
    public void ScopedSens(float sensMulti)
    {
        oldMouseSens = mouseSensitivity;
        mouseSensitivity = mouseSensitivity * sensMulti;
    }
    // Set the mouse sense back to the value before scoped view
    public void UnScopedSens()
    {
        mouseSensitivity = oldMouseSens;
    }
    // Returns the current mouse sensitivity
    public float GetMouseSensitivity()
    {
        return mouseSensitivity;
    }
}
