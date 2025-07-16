using UnityEngine;

public class TankDriveMapping : MonoBehaviour, SimpleControlMapping
{
    public Transform robot1;
    public Transform robot2;
    private Transform activeRobot;

    public float moveSpeed = 1.5f;
    public float turnSpeed = 90f;

    private bool isActive = false;
    private bool toggleCooldown = false;

    void Start()
    {
        activeRobot = robot1;
    }

    public void Enable()
    {
        isActive = true;
    }

    public void Disable()
    {
        isActive = false;
    }

    void Update()
    {
        if (isActive)
            ProcessInput();
    }

    public void ProcessInput()
    {
        // Handle robot swap on X button (left controller)
        if (OVRInput.GetDown(OVRInput.Button.Three)) // X button
        {
            activeRobot = (activeRobot == robot1) ? robot2 : robot1;
            Debug.Log("Switched active robot to: " + activeRobot.name);
        }

        // Left controller movement (translation)
        Vector2 leftThumbstick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        Vector3 move = new Vector3(leftThumbstick.x, 0, leftThumbstick.y) * moveSpeed * Time.deltaTime;
        activeRobot.Translate(move, Space.Self);

        // Right controller movement (rotation)
        Vector2 rightThumbstick = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        float turn = rightThumbstick.x * turnSpeed * Time.deltaTime;
        activeRobot.Rotate(Vector3.up, turn);
    }
}
