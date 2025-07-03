using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class CameraManager : MonoBehaviour
{
    public Transform xrRig;                  // Your XR Origin or OVRCameraRig
    public Transform targetCameraTransform;  // The camera to teleport to
    public bool useParenting = true;         // If true, parent the rig to follow
    private bool isTeleported = false;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Transform originalParent;


    private void Start()
    {
        xrRig.position = Vector3.zero; 
        xrRig.rotation = Quaternion.identity;
        isTeleported = false; 
    }
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            ToggleTeleport();
        }

        // If using scripted follow mode
        if (isTeleported && !useParenting)
        {
            xrRig.position = targetCameraTransform.position;
            xrRig.rotation = targetCameraTransform.rotation;
        }
    }

    void ToggleTeleport()
    {
        if (!isTeleported)
        {
            // Save original state
            originalPosition = xrRig.position;
            originalRotation = xrRig.rotation;
            originalParent = xrRig.parent;

            // Move XR rig to target
            xrRig.position = targetCameraTransform.position;
            xrRig.rotation = targetCameraTransform.rotation;

            if (useParenting)
                xrRig.SetParent(targetCameraTransform);

            isTeleported = true;
        }
        else
        {
            // Restore original state
            if (useParenting)
                xrRig.SetParent(originalParent);

            xrRig.position = originalPosition;
            xrRig.rotation = originalRotation;

            isTeleported = false;
        }
    }
}

