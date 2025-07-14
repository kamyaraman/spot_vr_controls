using System;
using UnityEngine;

public class SampleDriveFlow : ControlFlow
{
    public override void Start()
    {
        ComputedVar<bool> doRotateAndYMove = new(
            () => GetButton(Button.Trigger, ButtonState.Held));

        SetJoystickListener(JoystickState.Active,
            direction =>
            {
                if (doRotateAndYMove.Eval())
                {
                    if (Mathf.Abs(direction.x) >= Mathf.Abs(direction.y))
                        spot.Rotate(direction.x);
                    else
                        spot.SetHeight(spot.GetHeight() + direction.y);
                }
                else
                {
                    spot.Drive(direction);
                }
            });

        SetButtonListener(Button.AOrX, ButtonState.Down,
            () => {
                if (!spot.IsGripperInUse())
                    Transition(new SampleArmFlow());
                else
                    AddInfoLine("Error: Arm in Use!", TimeSpan.FromSeconds(3));
            });

        SetLabelGetter(Button.Joystick,
            () =>
            {
                if (doRotateAndYMove.Eval())
                    return "Rotate and Y-Move";
                else
                    return "Drive";
            });
        SetLabelGetter(Button.Trigger,
            () =>
            {
                if (doRotateAndYMove.Eval())
                    return "";
                else
                    return "Rotate and Y-Move";
            });
        SetLabelGetter(Button.AOrX, () => "Move Arm");

        SetHandColorGetter(() => Color.purple);
    }

    public override void Update()
    {

    }

    public override string GetName()
    {
        return "Drive Mode";
    }
}

public class SampleArmFlow : ControlFlow
{
    public override void Start()
    {
        spot.SetUsingGripper(true);

        SetHandListener(pos => spot.SetGripperPos(pos));

        SetButtonListener(Button.Trigger, ButtonState.Down,
            () => spot.SetGripperOpen(!spot.GetGripperOpen()));

        SetButtonListener(Button.AOrX, ButtonState.Down,
            () => {
                spot.SetUsingGripper(false);
                Transition(new SampleDriveFlow());
            });

        SetLabelGetter(Button.Trigger,
            () =>
            {
                if (spot.GetGripperOpen())
                    return "Close Claw";
                else
                    return "Open Claw";
            });
        SetLabelGetter(Button.AOrX, () => "Move Spot");

        SetHandColorGetter(() => Color.yellow);
    }

    public override void Update()
    {
        
    }

    public override string GetName()
    {
        return "Arm Mode";
    }
}