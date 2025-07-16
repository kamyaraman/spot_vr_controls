using UnityEngine;

public class ToggleMenu : MonoBehaviour
{
    public Canvas mappingCanvas; // Assign your ControlSelectorCanvas here
    private bool isVisible = false;

    void Start()
    {
        if (mappingCanvas != null)
            mappingCanvas.enabled = false; // Hide initially
    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Start))
        {
            isVisible = !isVisible;
            mappingCanvas.enabled = isVisible;
        }
    }
}
