using UnityEngine;
/// <summary>
/// Gets All Local Player Input.
/// </summary>
public class JInput : AController
{
    /// <summary>
    /// Unity Input Controller ID
    /// </summary>
    private int controllerID = 0;

	private string moveHorizontal;
	private string moveVertical;
	private string aimHorizontal;
	private string aimVertical;
	private string switchPower;
	private string firePower;
	
    public JInput(int controllerID)
    {
		this.controllerID = controllerID;

		moveHorizontal 	= "JMoveHorizontal" + controllerID;
		moveVertical	= "JMoveVertical" + controllerID;

		aimHorizontal 	= "JAimHorizontal" + controllerID;
		aimVertical 	= "JAimVertical" + controllerID;

		switchPower 	= "JSwitchPower" + controllerID;
		firePower 		= "JFirePower" + controllerID;
    }

    public override void UpdateController()
	{
		if(controllerID != 0)
		{
			moveDirection.x = Input.GetAxis(moveHorizontal);
			moveDirection.z = Input.GetAxis(moveVertical);

			aimDirection.x = Input.GetAxis(aimHorizontal);
			aimDirection.z = Input.GetAxis(aimVertical);

			isSwitchingPower = Input.GetButton(switchPower);
			isFiringPower = Input.GetButton(firePower);
		}
    }

	public int ControllerID 
	{
		get{ return controllerID; }
		set{ controllerID = value; }
	}
}
