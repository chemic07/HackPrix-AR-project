using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using cakeslice;

public class FurniturePlacementManager : MonoBehaviour
{
    public GameObject SpawnableFurniture;
    public ARRaycastManager raycastManager;
    public ARPlaneManager planeManager;
    public float movementScaleFactor = 1.0f;
    private List<ARRaycastHit> raycastHits = new List<ARRaycastHit>();
    private bool hasPlacedObject = false; // Flag to track if an object has been placed
    private List<GameObject> placedObjects = new List<GameObject>(); // List to store placed objects
    private GameObject selectedObject; // Reference to the selected object
    private Coroutine floatingCoroutine; // Coroutine for floating animation
    private ToggleOutlineOnSelect outlineScript; // Reference to the ToggleOutlineOnSelect script
    public Slider yAxisSlider; // Reference to the UI Slider for Y-axis movement


    private void Update()
    {
        if (!hasPlacedObject && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            bool collision = raycastManager.Raycast(Input.GetTouch(0).position, raycastHits, TrackableType.PlaneWithinPolygon);

            if (collision && !IsButtonPressed())
            {
                GameObject spawnedObject = Instantiate(SpawnableFurniture);
                spawnedObject.transform.position = raycastHits[0].pose.position;
                spawnedObject.transform.rotation = raycastHits[0].pose.rotation;

                // Start scaling animation coroutine
                StartCoroutine(LerpObjectScale(Vector3.zero, Vector3.one, 0.5f, spawnedObject));

                placedObjects.Add(spawnedObject); // Add the spawned object to the list
                hasPlacedObject = true; // Set the flag to true

                // Enable outline for the spawned object
                EnableOutlineForSelectedObject(spawnedObject);
            }

            // Deactivate plane visualization
            foreach (var plane in planeManager.trackables)
            {
                plane.gameObject.SetActive(false);
            }
            planeManager.enabled = false;
        }

        // Handle object rotation with two-finger touch
        if (selectedObject != null && Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            // Adjust rotation speed according to your preference
            float rotationSpeed = 0.1f;

            // Rotate the selected object around its up axis
            selectedObject.transform.Rotate(Vector3.up, deltaMagnitudeDiff * rotationSpeed);
        }


        // Handle object selection and deselection
        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                // Raycast to check if an object is touched
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    GameObject touchedObject = hit.transform.gameObject;
                    if (placedObjects.Contains(touchedObject))
                    {
                        SelectObject(touchedObject);
                    }
                    else
                    {
                        DeselectObject();
                    }
                }
                else
                {
                    DeselectObject();
                }
            }
        }

        // Move the selected object based on touch input
        if (selectedObject != null && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                MoveSelectedObject(touch);
            }
        }
    }

    private bool IsButtonPressed()
    {
        return EventSystem.current.currentSelectedGameObject?.GetComponent<Button>() != null;
    }

    // Add a private variable to store the last selected object
    private GameObject lastSelectedObject;

    // Modify the SelectObject method to store the last selected object
    public void SelectObject(GameObject obj)
    {
        lastSelectedObject = obj;
        // Deselect any previously selected object
        DeselectObject();
        selectedObject = obj;

        // Enable outline for the selected object
        EnableOutlineForSelectedObject(selectedObject);

        // delete this code to move to base


    }
    private void Start()
    {
        // Add listener to the Slider's OnValueChanged event
        yAxisSlider.onValueChanged.AddListener(OnYAxisSliderChanged);
    }

    // Declare a Vector3 variable to store the initial position
    private Vector3 initialPosition;

    public void OnYAxisSliderChanged(float value)
    {
        if (lastSelectedObject != null)
        {
            // Check if this is the first time the object is selected
            if (initialPosition == Vector3.zero)
            {
                // Store the initial position of the object
                initialPosition = lastSelectedObject.transform.position;
            }

            // Calculate the new position based on the initial position and the Slider's value
            Vector3 newPosition = initialPosition;
            newPosition.y += value * movementScaleFactor;
            // Add or subtract the Slider's value to move up or down

            // Update the Y position of the last selected object
            lastSelectedObject.transform.position = newPosition;
        }
    }






    // Modify the DeselectObject method to only clear the selected object
    private void DeselectObject()
    {
        if (selectedObject != null)
        {
            // Disable outline for the deselected object
            DisableOutlineForSelectedObject(selectedObject);
            selectedObject = null;
        }
    }

    // Modify the DeleteSelectedObject method to delete the last selected object
    public void DeleteSelectedObject()
    {
        if (lastSelectedObject != null && placedObjects.Contains(lastSelectedObject))
        {
            placedObjects.Remove(lastSelectedObject);
            Destroy(lastSelectedObject);
            lastSelectedObject = null; // Clear the last selected object
        }
    }


    public void SwitchFurniture(GameObject furniture)
    {
        SpawnableFurniture = furniture;
        hasPlacedObject = false; // Reset the flag when switching furniture
    }

    // LerpObjectScale coroutine for scaling animation
    private IEnumerator LerpObjectScale(Vector3 a, Vector3 b, float time, GameObject lerpObject)
    {
        float i = 0.0f;
        float rate = (1.0f / time);
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            lerpObject.transform.localScale = Vector3.Lerp(a, b, i);
            yield return null;
        }
    }

    private void EnableOutlineForSelectedObject(GameObject obj)
    {
        outlineScript = obj.GetComponent<ToggleOutlineOnSelect>();
        if (outlineScript != null)
        {
            outlineScript.ToggleOutline(true);
        }
    }

    private void DisableOutlineForSelectedObject(GameObject obj)
    {
        outlineScript = obj.GetComponent<ToggleOutlineOnSelect>();
        if (outlineScript != null)
        {
            outlineScript.ToggleOutline(false);
        }
    }

    private void MoveSelectedObject(Touch touch)
    {
        if (selectedObject != null)
        {
            // Convert touch movement to object translation in world space
            Vector3 forward = Camera.main.transform.forward;
            forward.y = 0f; // Ensure movement is only on the x-z plane
            Vector3 right = Camera.main.transform.right;

            // Calculate translation based on touch delta position and speed modifier
            Vector3 translation = (forward * touch.deltaPosition.y + right * touch.deltaPosition.x) * 0.0009f;

            // Apply translation to the selected object's position
            selectedObject.transform.Translate(translation, Space.World);

            // Update the initial position whenever the object's position changes
            initialPosition = selectedObject.transform.position;
        }
    }

}
