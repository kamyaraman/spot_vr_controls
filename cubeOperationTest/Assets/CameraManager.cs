using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class CameraManager : MonoBehaviour
{
    public GameObject xrRig;              // Your XR Rig GameObject (e.g., XR Origin or OVRCameraRig)
    public Camera objectCamera;           // The single camera on your object

    private bool isObjectView = false;

    void Start()
    {
        xrRig.SetActive(true);
        objectCamera.enabled = false;

        // Ensure only one AudioListener is active
        if (objectCamera.TryGetComponent<AudioListener>(out var audioListener))
        {
            audioListener.enabled = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            isObjectView = !isObjectView;

            xrRig.SetActive(!isObjectView);
            objectCamera.enabled = isObjectView;

            // AudioListener toggle
            if (objectCamera.TryGetComponent<AudioListener>(out var audioListener))
            {
                audioListener.enabled = isObjectView;
            }
        }
    }
}
