using UnityEngine;
using UnityEngine.InputSystem;

public class triggerMove : MonoBehaviour
{
    public GameObject[] cubes;
    private int currentCubeIndex = 0;

    private float switchCooldown = 0.5f;
    private float lastSwitchTime = -1f;

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Three) && Time.time - lastSwitchTime > switchCooldown) // X button
        {
            currentCubeIndex = (currentCubeIndex + 1) % cubes.Length;
            lastSwitchTime = Time.time;
        }

        Vector2 move = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);     // Left joystick
        Vector2 rotate = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick); // Right joystick

        GameObject current = cubes[currentCubeIndex];

        // Move forward/backward
        Vector3 moveDirection = (Vector3.forward * move.y + Vector3.right * move.x);
        current.transform.Translate(moveDirection * Time.deltaTime * 2f, Space.Self);

        // Rotate around Y-axis


        current.transform.Rotate(Vector3.up, rotate.x * Time.deltaTime * 100f);

        
    }
}