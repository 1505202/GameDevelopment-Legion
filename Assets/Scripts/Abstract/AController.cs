using UnityEngine;
/// <summary>
/// Abstract Controller, Both AI and JInput Extend This Class
/// </summary>
public abstract class AController : IControllerInput, IUpdateController
{
    protected Vector3 moveDirection = Vector3.zero;
	protected Vector3 aimDirection = Vector3.zero;

	protected bool isFiringPower = false;
	protected bool isSwitchingPower = false;
	
	public Vector3 MoveDirection()
	{
		return moveDirection;
	}
	public Vector3 AimDirection()
	{
		return aimDirection;
	}
	
	public bool SwitchingPower()
	{
		return isSwitchingPower;
	}
	public bool FiringPower()
	{
		return isFiringPower;
	}

	public abstract void UpdateController();
}
