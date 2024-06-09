using UnityEngine;
using UnityEngine.UI;

public class RotateOnDrag : MonoBehaviour
{
    public GameObject uiElement; // Reference to the UI element you want to hide/show
    private Vector3 dragStartPosition;
    private float rotationSpeed = 0.3f;
     private bool isRotating = false;

    void Update()
    {
        // Check if the mouse button is being held down
        if (Input.GetMouseButtonDown(0))
        {
            // Store the initial mouse position
            dragStartPosition = Input.mousePosition;
            isRotating = true; // Start rotating
            if (uiElement != null)
                uiElement.SetActive(false); // Hide the UI element
        }
        // Check if the mouse button is held down and moving
        else if (Input.GetMouseButton(0))
        {
            // Calculate the distance moved horizontally
            float deltaX = Input.mousePosition.x - dragStartPosition.x;

            // Rotate the object based on the horizontal movement
            transform.Rotate(Vector3.up, -deltaX * rotationSpeed, Space.Self);

            // Update the drag start position for the next frame
            dragStartPosition = Input.mousePosition;
        }
        // Check if the mouse button is released
        else if (Input.GetMouseButtonUp(0))
        {
            isRotating = false; // Stop rotating
            if (uiElement != null)
                uiElement.SetActive(true); // Show the UI element
        }
    }
}
