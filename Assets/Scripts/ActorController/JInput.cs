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
			this.moveDirection.x = Input.GetAxis(moveHorizontal);
            this.moveDirection.z = Input.GetAxis(moveVertical);

            this.aimDirection.x = Input.GetAxis(aimHorizontal);
            this.aimDirection.z = Input.GetAxis(aimVertical);

            this.isSwitchingPower = Input.GetButton(switchPower);
            this.isFiringPower = Input.GetButton(firePower);
		}
    }

	public int ControllerID 
	{
		get{ return controllerID; }
		set{ controllerID = value; }
	}

    public override string ToString()
    {
        return base.ToString() + " - " + controllerID;
    }
}
