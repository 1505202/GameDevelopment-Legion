using UnityEngine;
/// <summary>
/// Abstract Controller, Both AI and JInput Extend This Class
/// Commented Until Designer Is Happy With Button Layouts
/// </summary>
public enum ControllerInputKey
{
    Cross = 0,
    Circle = 1,
    Square = 2
}


public abstract class AController : IControllerInput, IUpdateController
{
    protected Vector3 moveDirection = Vector3.zero;
	protected Vector3 aimDirection = Vector3.zero;

    protected bool buttonSquare = false;
    protected bool buttonCross = false;
    protected bool buttonCircle = false;

    protected bool isFiringPower = false;
	
	public Vector3 MoveDirection()
	{
		return moveDirection;
	}
	public Vector3 AimDirection()
	{
		return aimDirection;
	}

    public bool GetButton(ControllerInputKey input)
    {
        if (input == ControllerInputKey.Cross)
        {
            return buttonCross;
        }

        if (input == ControllerInputKey.Circle)
        {
            return buttonCircle;
        }

        if (input == ControllerInputKey.Square)
        {
            return buttonSquare;
        }

        Debug.LogError("Bad Input Key");
        return false;
    }

    public bool FiringPower()
    {
        return isFiringPower;
    }

	public abstract void UpdateController();
}
