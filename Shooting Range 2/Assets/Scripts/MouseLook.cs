using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float defaultMouseSensitivity = 200f;
    public float mouseSensitivity = 200f;

    public Transform playerBody;
    public float oldMouseSens;

    float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    public void SetMouseSensitivity(float sens)
    {
        mouseSensitivity = sens;
    }

    public void ScopedSens(float sensMulti)
    {
        oldMouseSens = mouseSensitivity;
        mouseSensitivity = mouseSensitivity * sensMulti;
    }

    public void UnScopedSens()
    {
        mouseSensitivity = oldMouseSens;
    }
}
