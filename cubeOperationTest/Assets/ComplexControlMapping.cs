
using UnityEngine;

public interface IMove
{
    void Move(Vector2 input);
}

public interface IRotate
{
    void Rotate(float input);
}

public interface IArm
{
    void UseArm(bool activate);
} 

public interface IComplexControl : IMove, IRotate, IArm
{
    // This interface combines all the control interfaces.
    // It can be used to implement complex control systems that require movement, rotation, and arm usage.
}