using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ComplexDualDrive : MonoBehaviour, IComplexControl
{
    public GameObject robot;
    public Transform arm;

    public void Move(Vector2 input)
    {
        if (robot == null) return;

        Vector3 direction = new Vector3(input.x, 0, input.y);
        direction = Camera.main.transform.TransformDirection(direction);
        direction.y = 0;

        robot.transform.Translate(direction * Time.deltaTime * 2f);
    }

    public void Rotate(float input)
    {
        if (robot == null) return;
        robot.transform.Rotate(Vector3.up * input * Time.deltaTime * 100f);
    }

    public void UseArm(bool activate)
    {
        if (arm == null) return;
        arm.localEulerAngles = activate ? new Vector3(-30, 0, 0) : Vector3.zero;
    }

    void Update()
    {
        Vector2 moveInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        Move(moveInput);

        float rotateInput = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).x;
        Rotate(rotateInput);

        bool armInput = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger);
        UseArm(armInput);
    }
}

