using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections;

public class ARTrackingPlaneIndicator : MonoBehaviour
{
    public GameObject indicatorUI;
    public float delayBeforeHiding = 10f; // Delay before hiding the UI indicator

    private ARPlaneManager planeManager;
    private bool indicatorHidden = false;

    private void Start()
    {
        // Get a reference to the ARPlaneManager
        planeManager = FindObjectOfType<ARPlaneManager>();

        // Subscribe to the ARPlaneManager's event
        planeManager.planesChanged += OnPlanesChanged;
    }

    private void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        if (!indicatorHidden)
        {
            foreach (var plane in args.added)
            {
                // Once a plane is detected, start a coroutine to hide the indicator after delay
                StartCoroutine(HideIndicatorAfterDelay());
                return;
            }
        }
    }

    private IEnumerator HideIndicatorAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeHiding);

        if (indicatorUI != null)
        {
            indicatorUI.SetActive(false);
            indicatorHidden = true;
        }
    }
}
