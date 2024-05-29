using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] GameObject camHolder;
    [SerializeField] GameObject oriention;
    float pitch;
    float yaw;
    [SerializeField] GameObject cursor;

    [Header("Stats")]
    [SerializeField] float sensitivity;

    [Header("State")]
    public bool uiMode;

    // Update is called once per frame
    void Update()
    {
        if (uiMode)
        {
            cursor.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            cursor.transform.position = Input.mousePosition;
        }
        else
        {
            cursor.SetActive(false);

            Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

            pitch -= mouseDelta.y * sensitivity;
            pitch = Mathf.Clamp(pitch, -80, 80);

            yaw += mouseDelta.x * sensitivity;

            camHolder.transform.localEulerAngles = Vector3.right * pitch;

            oriention.transform.localEulerAngles = Vector3.up * yaw;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(camHolder.transform.position, camHolder.transform.position + camHolder.transform.forward);
    }
}
