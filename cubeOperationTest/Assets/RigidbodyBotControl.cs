using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

[RequireComponent(typeof(Rigidbody))]
public class RigidbodyBotControl : MonoBehaviour
{
    public bool isRightControl;

    private Rigidbody rb;
    private Vector2 move;
    private bool rotate;
    private OVRInput.Controller controller;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        controller = isRightControl ? OVRInput.Controller.RTouch : OVRInput.Controller.LTouch;
    }

    void Update()
    {
        // Get input
        move = isRightControl ?
            OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick) :
            OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

        rotate = isRightControl ?
            OVRInput.Get(OVRInput.Button.SecondaryHandTrigger) :
            OVRInput.Get(OVRInput.Button.PrimaryHandTrigger);

        if (rotate)
        {
            transform.Rotate(Vector3.up, move.x * Time.deltaTime * 100f);
            OVRInput.SetControllerVibration(0.5f, 0.7f, controller);
        }
        else
        {
            OVRInput.SetControllerVibration(0, 0, controller);
        }
    }

    void FixedUpdate()
    {
        if (rotate) return; // Don't move while rotating

        Vector3 movement = Vector3.zero;

        if (Mathf.Abs(move.y) >= Mathf.Abs(move.x))
        {
            movement = transform.forward * move.y;
        }
        else
        {
            movement = transform.right * move.x;
        }

        rb.MovePosition(rb.position + movement * 2f * Time.fixedDeltaTime);
    }
}
