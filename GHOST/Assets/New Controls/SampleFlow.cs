using System.Collections.Generic;
using UnityEngine;

public class SampleFlow : ControlFlow
{
    private int direction = 0;

    public override void Start()
    {
        SetButtonListener(Button.A, ButtonState.Held, () => {
            spotOne.Drive(new(0f, 1f));
            direction = 1;
            });
        SetButtonListener(Button.B, ButtonState.Held, () => {
            spotOne.Drive(new(0f, -1f));
            direction = -1;
        });
        SetButtonListener(Button.A, ButtonState.Up, () => direction = 0);
        SetButtonListener(Button.B, ButtonState.Up, () => direction = 0);

        SetLabelGetter(Button.A, () => direction == 1 ? "Going forward" : "Forward");
        SetLabelGetter(Button.B, () => direction == -1 ? "Going backward" : "Backward");

        //SetButtonListener(Button.X, ButtonState.Down, () => spotOne.SetGripperOpen(true));
        //SetButtonListener(Button.Y, ButtonState.Down, () => spotOne.SetGripperOpen(false));

        SetButtonListener(Button.X, ButtonState.Down, () => spotOne.SetGripperPos(spotOne.GetGripperPos() + new Vector3(0f, 0.1f, 0.1f)));
        SetButtonListener(Button.Y, ButtonState.Down, () => spotOne.SetGripperPos(spotOne.GetGripperPos() - new Vector3(0f, 0.1f, 0.1f)));

        SetInfoGetter(() =>
        {
            if (direction == 1)
                return "Going forward";
            if (direction == -1)
                return "Going backward";
            return "";
        });
    }

    public override void Update()
    {

    }
}