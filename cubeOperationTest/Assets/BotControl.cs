using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class BotControl : MonoBehaviour
{
    public bool isRightControl;

    public Transform arm, jaw, cameraRig;
    public Transform controllerAnchor;

    private Vector3? oldCameraPos;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        OVRInput.Controller controller = isRightControl ? OVRInput.Controller.RTouch : OVRInput.Controller.LTouch; 

        Vector2 action;
        if (isRightControl)
        {
            action = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        }
        else
        {
            action = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        }

        bool doRotation;
        if (isRightControl)
        {
            doRotation = OVRInput.Get(OVRInput.Button.SecondaryThumbstick);
        }
        else
        {
            doRotation = OVRInput.Get(OVRInput.Button.PrimaryThumbstick);
        }

        if (doRotation)
        {
            transform.Rotate(Vector3.up, action.x * Time.deltaTime * 100f);
            OVRInput.SetControllerVibration(0.5f, 0.7f, controller);
        }
        else
        {
            OVRInput.SetControllerVibration(0, 0, controller);
            if (Mathf.Abs(action.y) >= Mathf.Abs(action.x))
            {
                transform.position += 2f * action.y * Time.deltaTime * transform.forward;
            }
            else
            {
                transform.position += 2f * action.x * Time.deltaTime * transform.right;
            }
        }

        bool moveArm;
        if (isRightControl)
        {
            moveArm = OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger);
        }
        else
        {
            moveArm = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger);
        }
        if (moveArm)
        {
            arm.rotation = Quaternion.LookRotation(controllerAnchor.position - Camera.main.transform.position);
            if (oldCameraPos == null)
            {
                oldCameraPos = cameraRig.position;
            }
            cameraRig.position = transform.position;
        }
        else if (oldCameraPos.HasValue)
        {
            cameraRig.position = oldCameraPos.Value;
            oldCameraPos = null;
        }

        float grip;
        if (isRightControl)
        {
            grip = OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger);
        }
        else
        {
            grip = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger);
        }
        jaw.localEulerAngles = new Vector3(-90f - 90f * grip, 0f, 90f);
    }
}