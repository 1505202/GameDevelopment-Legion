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
    private string firePower;
    private string buttonCrossLabel;
    private string buttonSquareLabel;
    private string buttonCircleLabel;
	
    public JInput(int controllerID)
    {
		this.controllerID = controllerID;

		moveHorizontal 	= "JMoveHorizontal" + controllerID;
		moveVertical	= "JMoveVertical" + controllerID;

		aimHorizontal 	= "JAimHorizontal" + controllerID;
		aimVertical 	= "JAimVertical" + controllerID;

        buttonCrossLabel = "JButtonCross" + controllerID;
        buttonSquareLabel = "JButtonSquare" + controllerID;
        buttonCircleLabel = "JButtonCircle" + controllerID;

        firePower = "JFirePower" + controllerID;
    }

    public override void UpdateController()
	{
		if(controllerID != 0)
		{
			this.moveDirection.x = Input.GetAxis(moveHorizontal);
            this.moveDirection.z = Input.GetAxis(moveVertical);

            this.aimDirection.x = Input.GetAxis(aimHorizontal);
            this.aimDirection.z = Input.GetAxis(aimVertical);

            this.buttonSquare = Input.GetButton(buttonSquareLabel);
            this.buttonCross  = Input.GetButton(buttonCrossLabel);
            this.buttonCircle = Input.GetButton(buttonCircleLabel);

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
