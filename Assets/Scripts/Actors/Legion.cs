using UnityEngine;
using UnityEngine.Networking;

using System.Collections.Generic;
/// <summary>
/// Legion Actor
/// </summary>
[NetworkSettings(channel=1, sendInterval=0)]
public class Legion : AActor
{
//	[Header("Assimilatee Skills")]
//	[Header("Slow Beam")]
//
//	[Header("Tethered Probe")]
//
//	[Header("Satellite Probe")]
//	[SerializeField] private float 

	public enum ELegionState 
	{ 
		Controllable,   // Controllable
		Traversing  	// Traversing Terrain (The Jumps)
	}
	private ELegionState legionState = ELegionState.Controllable;
	
    [SerializeField] private AController inputController;

	// ComponentCaching
	private Rigidbody myRigidbody = null;
	private Transform myTransform = null;
	private Transform myChildTransform = null;

	// ForceCameraControls
	private Vector3 cameraRotation = Vector3.zero;
	private Vector3 cameraOffset = Vector3.zero;

	private void Start()
	{
		myRigidbody = GetComponent<Rigidbody>();
		// Setup new the assimilatedLegionSkillsArray
		myTransform = GetComponent<Transform>();

		// Add Legion Camera
		inputController = ControllerManager.Instance.NewController(new JInput( 1 ));

		if (isLocalPlayer) 
		{
			myChildTransform = myTransform.GetChild (0);
			cameraRotation = myChildTransform.rotation.eulerAngles;
			cameraOffset = myChildTransform.localPosition;
			myChildTransform.position = myTransform.position + cameraOffset;
			myChildTransform.gameObject.SetActive(true);
		}
	}

	private void Update()
	{
	    if (!isLocalPlayer)
	    {
	        return;
	    }

	    if (legionState == ELegionState.Controllable)
	    {
			myRigidbody.velocity = inputController.MoveDirection() * movementSpeed;

	        if (inputController.MoveDirection() != Vector3.zero)
	        {
	            Quaternion lookRotation = Quaternion.LookRotation(inputController.MoveDirection());
	            myTransform.rotation = Quaternion.Slerp(
	                myTransform.rotation,
	                lookRotation,
	                Time.deltaTime * rotateSpeed);

				if (isLocalPlayer) 
				{
	            	myChildTransform.rotation = Quaternion.Euler(cameraRotation);
	            	myChildTransform.position = myTransform.position + cameraOffset;
				}
	        }
	        return;
	    }

	    if (legionState == ELegionState.Traversing)
	    {

	        return;
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

	public ELegionState LegionState
	{
		get { return legionState; }
		set { legionState = value; }
	}

	public AController Controller
	{
		get { return inputController; }
		set { inputController = value; }
	}

	#endregion
}
