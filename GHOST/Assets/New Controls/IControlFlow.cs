using System;
using System.Collections.Generic;
using RosSharp.RosBridgeClient;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public abstract class ControlFlow
{
    public SpotInterface spotOne;
    public TMP_Text[] labels;
    public GameObject dummyGripper;

    public enum Button { A, B, X, Y };
    public enum ButtonState { Down, Held, Up };

    public enum Joystick { Left, Right };
    public enum JoystickState { Active, Idle };

    private readonly Dictionary<Button, OVRInput.Button> buttonOvrMapping = new()
    {
        { Button.A, OVRInput.Button.One },
        { Button.B, OVRInput.Button.Two },
        { Button.X, OVRInput.Button.Three },
        { Button.Y, OVRInput.Button.Four }
    };

    private readonly Dictionary<Button, int> labelIndices = new()
    {
        { Button.A, 0 },
        { Button.B, 1 }
    };

    private readonly Dictionary<Tuple<Button, ButtonState>, Action> buttonListeners = new();
    private readonly Dictionary<Button, Func<string>> labelGetters = new();

    public void SuperUpdate()
    {
        Debug.Log(OVRInput.Get(OVRInput.Button.Three));
        foreach (var kvp in buttonListeners)
        {
            var ovrButton = buttonOvrMapping[kvp.Key.Item1];
            var buttonState = kvp.Key.Item2;

            if (
                buttonState == ButtonState.Down && OVRInput.GetDown(ovrButton) ||
                buttonState == ButtonState.Held && OVRInput.Get(ovrButton) ||
                buttonState == ButtonState.Up && OVRInput.GetUp(ovrButton)
                )
                kvp.Value();
        }

        foreach (var kvp in labelGetters)
        {
            var labelIndex = labelIndices[kvp.Key];
            labels[labelIndex].text = kvp.Value();
        }

        Update();
    }  

    public void SetButtonListener(Button button, ButtonState buttonState, Action listener)
    {
        buttonListeners[new(button, buttonState)] = listener;    
    }

    public void SetLabelGetter(Button button, Func<string> getter)
    {
        labelGetters[button] = getter;
    }

    public abstract void Start();

    public abstract void Update();
}

public class SpotInterface
{
    private readonly MoveSpot move;
    private readonly SetGripper gripper;

    public SpotInterface(GameObject rosConnector)
    {
        move = rosConnector.GetComponent<MoveSpot>();
        gripper = rosConnector.GetComponent<SetGripper>();
    }

    public void Drive(Vector2 direction)
    {
        move.drive(direction, 0f, 0f);
    }

    public void SetGripperOpen(bool open)
    {
        if (open)
            gripper.openGripper();
        else
            gripper.closeGripper();
    }
}