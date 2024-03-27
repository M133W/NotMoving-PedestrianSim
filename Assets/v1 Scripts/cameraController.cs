using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float rotationSpeed = 2.0f;
    public float maxHorizontalAngle = 90.0f;

    public float horizontalRotation = 0;
    private bool canRotateLeft = false;

    void Start()
    {
        // hide the cursor
        Cursor.visible=false;
        // Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float horizontalRotationInput = Input.GetAxis("Horizontal")* rotationSpeed;

        // if turn right
        if (horizontalRotationInput > 0)
        {
            horizontalRotation += horizontalRotationInput;
            horizontalRotation = Mathf.Clamp(horizontalRotation, 0, maxHorizontalAngle);
            canRotateLeft = true;
        }
        // if turn left
        else if (canRotateLeft && horizontalRotationInput < 0)
        {
            horizontalRotation += horizontalRotationInput;
            horizontalRotation = Mathf.Clamp(horizontalRotation, 0, maxHorizontalAngle);

            // if at the initial view, stop turing left
            if (horizontalRotation == 0)
            {
                canRotateLeft = false;
            }
        }

        // rotate the camera
        transform.localEulerAngles = new Vector3(0, horizontalRotation, 0);
        // Debug.Log("Pixel width :" + Camera.main.pixelWidth + " Pixel height : " + Camera.main.cameraOutput.pixelHeight);
    }
}
