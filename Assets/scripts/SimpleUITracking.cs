using UnityEngine;

public class SimpleUITracking : MonoBehaviour
{
    public float moveSpeed = 50f; // Speed of movement
    public float minScale = 1f; // Minimum scale
    public float maxScale = 2f; // Maximum scale
    public float scaleSpeed = 1f; // Speed of scaling
    public float maxOffset = 50f; // Maximum offset from initial position

    private RectTransform rectTransform;
    private Vector2 initialPosition;
    private Vector2 targetPosition;
    private float currentScale;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        initialPosition = rectTransform.anchoredPosition;
        currentScale = minScale;
        SetRandomTargetPosition();
    }

    void Update()
    {
        // Move towards target position
        rectTransform.anchoredPosition = Vector2.MoveTowards(rectTransform.anchoredPosition, targetPosition, moveSpeed * Time.deltaTime);

        // Scale up and down
        currentScale = Mathf.PingPong(Time.time * scaleSpeed, maxScale - minScale) + minScale;
        rectTransform.localScale = new Vector3(currentScale, currentScale, 1f);

        // If reached target position, set a new random target position
        if ((Vector2)rectTransform.anchoredPosition == targetPosition)
        {
            SetRandomTargetPosition();
        }
    }

    // Set a new random target position within the offset range from the initial position
    void SetRandomTargetPosition()
    {
        float offsetX = Random.Range(-maxOffset, maxOffset);
        float offsetY = Random.Range(-maxOffset, maxOffset);
        targetPosition = initialPosition + new Vector2(offsetX, offsetY);
    }
}
