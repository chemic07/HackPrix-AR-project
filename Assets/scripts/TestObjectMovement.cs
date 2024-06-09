using UnityEngine;

public class SimpleObjectMovement : MonoBehaviour
{
    public GameObject selectedObject;
    private bool isDragging = false;
    private Vector3 offset;

    void Update()
    {
        // Toggle object selection with the J key
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (selectedObject == null)
            {
                // If no object is selected, select the object
                SelectObject();
            }
            else
            {
                // If an object is selected, deselect it
                DeselectObject();
            }
        }

        // If an object is selected and left mouse button is clicked, start dragging
        if (Input.GetMouseButtonDown(0) && selectedObject != null)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject == selectedObject)
                {
                    isDragging = true;
                    offset = selectedObject.transform.position - hit.point;
                }
            }
        }

        // If left mouse button is released, stop dragging
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        // If an object is selected and dragging, move it with mouse
        if (isDragging)
        {
            MoveSelectedObject();
        }
    }

    private void SelectObject()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            selectedObject = hit.transform.gameObject;
            isDragging = true;
            offset = selectedObject.transform.position - hit.point;
        }
    }

    private void DeselectObject()
    {
        selectedObject = null;
        isDragging = false;
    }

    private void MoveSelectedObject()
    {
        if (selectedObject != null)
        {
            // Get the current mouse position in world space
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, selectedObject.transform.position.z));

            // Apply the offset
            mousePosition += offset;

            // Set the object's position to the new mouse position
            selectedObject.transform.position = mousePosition;
        }
    }
}
