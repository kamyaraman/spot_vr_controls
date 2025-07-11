using System;
using System.Collections.Generic;
using RosSharp.RosBridgeClient;
using TMPro;
using UnityEngine;

public abstract class ControlFlow
{
    public enum Button { AOrX, BOrY, Joystick, Trigger, Grip };
    public enum ButtonState { Down, Held, Up };

    public enum JoystickState { Active, Idle };

    public SpotInterface spot;

    private Dictionary<Button, OVRInput.Button> buttonOvrMapping;
    private OVRInput.Axis2D joystickOvr;
    private GameObject handAnchor;
    private TMP_Text[] labels;
    private Action<ControlFlow> managerTransition;

    private Dictionary<Tuple<Button, ButtonState>, Action> buttonListeners;
    private Dictionary<JoystickState, Action<Vector2>> joystickListeners;
    private Action<Vector3> handListener;
    private Dictionary<Button, Func<string>> labelGetters;

    private readonly Button[] labelOrder =
    {
        Button.AOrX,
        Button.BOrY,
        Button.Joystick,
        Button.Trigger
    };

    public void SuperStart(
        SpotInterface spot,
        Dictionary<Button, OVRInput.Button> buttonOvrMapping,
        OVRInput.Axis2D joystickOvr,
        GameObject handAnchor,
        TMP_Text[] labels,
        Action<ControlFlow> managerTransition
        )
    {
        this.spot = spot;

        this.buttonOvrMapping = buttonOvrMapping;
        this.joystickOvr = joystickOvr;
        this.handAnchor = handAnchor;
        this.labels = labels;
        this.managerTransition = managerTransition;

        buttonListeners = new();
        joystickListeners = new();
        handListener = null;
        labelGetters = new();

        Start();
    }

    public void SuperUpdate()
    {
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

        var joystickPos = OVRInput.Get(joystickOvr);
        var joystickState = joystickPos.magnitude > 0.1f ? JoystickState.Active : JoystickState.Idle;
        if (joystickListeners.ContainsKey(joystickState))
            joystickListeners[joystickState](joystickPos);

        if (handListener != null)
            handListener(handAnchor.transform.position);

        for (int i = 0; i < labelOrder.Length; i++)
        {
            var button = labelOrder[i];
            if (labelGetters.ContainsKey(button))
                labels[i].text = labelGetters[button]();
            else
                labels[i].text = "";
        }

        Update();
    }  

    public void SetButtonListener(Button button, ButtonState buttonState, Action listener)
    {
        buttonListeners[new(button, buttonState)] = listener;    
    }

    public void SetJoystickListener(JoystickState joystickState, Action<Vector2> listener)
    {
        joystickListeners[joystickState] = listener;
    }

    public void SetHandListener(Action<Vector3> listener)
    {
        handListener = listener;
    }

    public void SetLabelGetter(Button button, Func<string> getter)
    {
        labelGetters[button] = getter;
    }

    public bool GetButton(Button button, ButtonState state)
    {
        var ovrButton = buttonOvrMapping[button];
        return state == ButtonState.Down && OVRInput.GetDown(ovrButton) ||
            state == ButtonState.Held && OVRInput.Get(ovrButton) ||
            state == ButtonState.Up && OVRInput.GetUp(ovrButton);
    }

    public void Transition(ControlFlow otherFlow)
    {
        managerTransition(otherFlow);
    }

    public abstract void Start();

    public abstract void Update();
}

public class SpotInterface
{
    private readonly MoveSpot move;
    private readonly SetGripper gripper;

    private readonly GameObject dummyGripper;

    private bool isGripperOpen = false;
    private float height = 0f;

    public SpotInterface(GameObject rosConnector, GameObject dummyGripper)
    {
        move = rosConnector.GetComponent<MoveSpot>();
        gripper = rosConnector.GetComponent<SetGripper>();

        this.dummyGripper = dummyGripper;

        SetGripperOpen(false);
        SetHeight(0f);
    }

    public void Drive(Vector2 direction)
    {
        move.drive(direction, 0f, 0f);
    }

    public void Rotate(float direction)
    {
        move.drive(new(0f, 0f), direction, 0f);
    }

    public bool GetGripperOpen()
    {
        return isGripperOpen;
    }

    public void SetGripperOpen(bool isGripperOpen)
    {
        if (isGripperOpen)
            gripper.openGripper();
        else
            gripper.closeGripper();
        this.isGripperOpen = isGripperOpen;
    }

    public float GetHeight()
    {
        return height;
    }

    public void SetHeight(float height)
    {
        move.drive(new(0f, 0f), 0f, height);
        this.height = height;
    }

    public Vector3 GetGripperPos()
    {
        return dummyGripper.transform.position;
    }

    public void SetGripperPos(Vector3 pos)
    {
        dummyGripper.transform.position = pos;
    }
}

public class ComputedVar<T>
{
    private readonly Func<T> getter;

    public ComputedVar(Func<T> getter)
    {
        this.getter = getter;
    }

    public T Eval()
    {
        return getter();
    }
}