using System;
using System.Collections.Generic;
using System.Linq;
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
    private SkinnedMeshRenderer handRenderer;
    private TMP_Text[] labels;
    private TMP_Text infoText;
    private Action<ControlFlow> managerTransition;

    private Dictionary<Tuple<Button, ButtonState>, Action> buttonListeners;
    private Dictionary<JoystickState, Action<Vector2>> joystickListeners;
    private Action<Transform> handListener;
    private Func<Color> handColorGetter;
    private Dictionary<Button, Func<string>> labelGetters;

    public List<Tuple<string, DateTime>> infoTextLines = new();

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
        SkinnedMeshRenderer handRenderer,
        TMP_Text[] labels,
        TMP_Text infoText,
        List<Tuple<string, DateTime>> infoTextLines,
        Action<ControlFlow> managerTransition
        )
    {
        this.spot = spot;

        this.buttonOvrMapping = buttonOvrMapping;
        this.joystickOvr = joystickOvr;
        this.handAnchor = handAnchor;
        this.handRenderer = handRenderer;
        this.labels = labels;
        this.infoText = infoText;
        this.managerTransition = managerTransition;

        buttonListeners = new();
        joystickListeners = new();
        handListener = null;
        handColorGetter = null;
        labelGetters = new();

        this.infoTextLines = infoTextLines;
        this.infoTextLines.Add(new("Entered " + GetName(), DateTime.Now.AddSeconds(3)));

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
            handListener(handAnchor.transform);

        if (handColorGetter != null)
            handRenderer.material.color = handColorGetter();
        else
            handRenderer.material.color = Color.white;

        for (int i = 0; i < labelOrder.Length; i++)
        {
            var button = labelOrder[i];
            if (labelGetters.ContainsKey(button))
                labels[i].text = labelGetters[button]();
            else
                labels[i].text = "";
        }

        infoTextLines = infoTextLines.Where(l => l.Item2 > DateTime.Now).ToList();
        infoText.text = string.Join("\n", infoTextLines.Select(l => l.Item1));

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

    public void SetHandListener(Action<Transform> listener)
    {
        handListener = listener;
    }

    public void SetHandColorGetter(Func<Color> getter)
    {
        handColorGetter = getter;
    }

    public void SetLabelGetter(Button button, Func<string> getter)
    {
        labelGetters[button] = getter;
    }

    public void AddInfoLine(string text, TimeSpan duration)
    {
        infoTextLines.Add(new(text, DateTime.Now + duration));
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

    public abstract string GetName();
}

public class SpotInterface
{
    private readonly MoveSpot move;
    private readonly SetGripper gripper;

    private readonly GameObject dummyGripper, gripperView;

    private bool isGripperOpen = false;
    private float height = 0f;
    private int gripperUsers = 0;

    public SpotInterface(GameObject rosConnector, GameObject dummyGripper, GameObject gripperView)
    {
        move = rosConnector.GetComponent<MoveSpot>();
        gripper = rosConnector.GetComponent<SetGripper>();

        this.dummyGripper = dummyGripper;
        this.gripperView = gripperView;

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

    public void SetUsingGripper(bool isUsing)
    {
        if (isUsing)
            gripperUsers++;
        else
            gripperUsers--;

        gripperView.SetActive(gripperUsers > 0);
    }

    public bool IsGripperInUse()
    {
        return gripperUsers > 0;
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
        height = Mathf.Clamp(height, -1f, 1f);
        move.drive(new(0f, 0f), 0f, height);
        this.height = height;
    }

    public Vector3 GetGripperPos()
    {
        return dummyGripper.transform.position;
    }

    public void SetGripperPos(Transform tf)
    {
        dummyGripper.transform.SetPositionAndRotation(tf.position, tf.rotation);
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