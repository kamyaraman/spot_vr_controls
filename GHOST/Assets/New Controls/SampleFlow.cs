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
                    spot.Rotate(direction.x);
                    spot.SetHeight(spot.GetHeight() + direction.y);
                }
                else
                {
                    spot.Drive(direction);
                }
            });

        SetButtonListener(Button.AOrX, ButtonState.Down,
            () => Transition(new SampleArmFlow()));

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
    }

    public override void Update()
    {

    }
}

public class SampleArmFlow : ControlFlow
{
    public override void Start()
    {
        SetHandListener(pos => spot.SetGripperPos(pos));

        SetButtonListener(Button.Trigger, ButtonState.Down,
            () => spot.SetGripperOpen(!spot.GetGripperOpen()));

        SetButtonListener(Button.AOrX, ButtonState.Down,
            () => Transition(new SampleDriveFlow()));

        SetLabelGetter(Button.Trigger,
            () =>
            {
                if (spot.GetGripperOpen())
                    return "Close Claw";
                else
                    return "Open Claw";
            });
        SetLabelGetter(Button.AOrX, () => "Move Spot");
    }

    public override void Update()
    {
        
    }
}