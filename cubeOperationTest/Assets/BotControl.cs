using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotControl : MonoBehaviour
{
    public bool isRightControl;
    OVRInput.Controller controller;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 move;
        if (isRightControl)
        {
            move = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        }
        else
        {
            move = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        }

        bool rotate;
        if (isRightControl)
        {
            rotate = OVRInput.Get(OVRInput.Button.SecondaryHandTrigger);
        }
        else
        {
            rotate = OVRInput.Get(OVRInput.Button.PrimaryHandTrigger);
        }

        if (rotate)
        {
            transform.Rotate(Vector3.up, move.x * Time.deltaTime * 100f);
            OVRInput.SetControllerVibration(0.5f, 0.7f, controller);
        }
        else
        {
            OVRInput.SetControllerVibration(0, 0, controller);
            if (Mathf.Abs(move.y) >= Mathf.Abs(move.x))
            {
                transform.position += transform.forward * move.y * 2f * Time.deltaTime;
            }
            else
            {
                transform.position += transform.right * move.x * 2f * Time.deltaTime;
            }
        }
    }
}