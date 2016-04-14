using UnityEngine;
/// <summary>
/// Interface To Update The ActorController
/// </summary>
public interface IControllerInput
{
    Vector3 MoveDirection();
    Vector3 AimDirection();

    bool FiringPower();

    bool GetButton(ControllerInputKey key);
}
