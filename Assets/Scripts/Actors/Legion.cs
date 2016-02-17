using UnityEngine;
using UnityEngine.Networking;

using System.Collections.Generic;
/// <summary>
/// Legion Actor
/// </summary>
[NetworkSettings(channel=1, sendInterval=0)]
public class Legion : AActor
{	
    [SerializeField] private AController inputController;

	// ComponentCaching
	private Rigidbody myRigidbody = null;
	private Transform myTransform = null;

	private void Start()
	{
        // Component Caching
		myRigidbody = GetComponent<Rigidbody>();
		myTransform = GetComponent<Transform>();

		// Add Legion Camera
		inputController = ControllerManager.Instance.NewController(new JInput( 1 ));
	}

	private void Update()
	{
	    if (!isLocalPlayer)
	    {
	        return;
	    }

		myRigidbody.velocity = inputController.MoveDirection() * movementSpeed;

	    if (inputController.MoveDirection() != Vector3.zero)
	    {
	        Quaternion lookRotation = Quaternion.LookRotation(inputController.MoveDirection());
	        myTransform.rotation = Quaternion.Slerp( myTransform.rotation, lookRotation, Time.deltaTime * rotateSpeed);
	    }
	}

	/// <summary>
	/// Updates the camera layers.
	/// TODO: When Local Split Sceen Gets Implemented make sure to incorporate the update of all local players
	/// </summary>
	private void UpdateCameraLayers()
	{

	}

	#region Properties

	public AController Controller
	{
		get { return inputController; }
		set { inputController = value; }
	}

	#endregion
}
