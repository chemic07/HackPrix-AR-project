using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cakeslice
{
    public class ToggleOutlineOnSelect : MonoBehaviour
    {
        private bool enabledByOtherScript = false;

        private void Start()
        {
            if (enabledByOtherScript)
            {
                ToggleOutline(true);
            }
            else
            {
                ToggleOutline(false);
            }
        }

        public void EnableOutline()
        {
            enabledByOtherScript = true;
            ToggleOutline(true);
        }

        public void DisableOutline()
        {
            enabledByOtherScript = false;
            ToggleOutline(false);
        }

        public void ToggleOutline(bool enable)
        {
            // Iterate through each child object and recursively toggle outlines
            ToggleOutlineRecursive(transform, enable);
        }

        private void ToggleOutlineRecursive(Transform parent, bool enable)
        {
            // Toggle outline for the current object
            Outline outline = parent.GetComponent<Outline>();
            if (outline != null)
            {
                outline.enabled = enable;
            }

            // Recursively toggle outlines for child objects
            foreach (Transform child in parent)
            {
                ToggleOutlineRecursive(child, enable);
            }
        }
    }
}
