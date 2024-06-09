using UnityEngine;
using UnityEngine.UI;

public class MoveUpDown : MonoBehaviour
{
    public Slider slider; // Reference to the slider UI component
    public float minHeight = 0f; // Minimum height of the object
    public float maxHeight = 5f; // Maximum height of the object
    public float speed = 2f; // Speed of movement

    private float targetHeight; // Target height for smooth movement

    private void Start()
    {
        // Add listener to the slider's value change event
        slider.onValueChanged.AddListener(OnSliderValueChanged);

        // Set initial target height based on slider's initial value
        targetHeight = Mathf.Lerp(minHeight, maxHeight, slider.value);
    }

    private void OnSliderValueChanged(float value)
    {
        // Update target height based on the new slider value
        targetHeight = Mathf.Lerp(minHeight, maxHeight, value);
    }

    private void Update()
    {
        // Smoothly move the object to the target height
        Vector3 targetPosition = new Vector3(transform.position.x, targetHeight, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * speed);
    }
}
