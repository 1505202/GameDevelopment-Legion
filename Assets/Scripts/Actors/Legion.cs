using UnityEngine;

using System.Collections.Generic;
/// <summary>
/// Legion Actor
/// </summary>
[RequireComponent(typeof(Rigidbody))]
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
	
	private ASkill[] assimilatedLegionSkills = new ASkill[3];
	private int skillIndex = 0;

	// Implement when local players exist
	// private List<Transform> cameras = new List<Transform>();

	// ComponentCaching
	private Rigidbody myRigidbody = null;
	private Transform myTransform = null;
	private Transform myChildTransform = null;

	private void Start()
	{
		myRigidbody = GetComponent<Rigidbody>();
		myTransform = GetComponent<Transform>();
		myChildTransform = myTransform.GetChild(0);

		// Add Legion Camera
		inputController = ControllerManager.Instance.NewController(new JInput( 1 ));

		// Setup new the assimilatedLegionSkillsArray
	}

	private void Update()
	{
		if( legionState == ELegionState.Controllable )
		{
			myRigidbody.velocity = inputController.MoveDirection() * movementSpeed;

			if(inputController.MoveDirection() != Vector3.zero)
			{
				Quaternion lookRotation = Quaternion.LookRotation (inputController.MoveDirection());
				myTransform.rotation = Quaternion.Slerp (myTransform.rotation, lookRotation, Time.deltaTime * rotateSpeed);
				myChildTransform.rotation = Quaternion.Euler( 90, 0, 0 );
			}
			return;
		}

		if( legionState == ELegionState.Traversing )
		{

			return;
		}
	}

	private void OnCollisionEnter(Collision obj)
	{
		if(obj.gameObject.CompareTag("Rogue"))
		{
			Rogue rogue = obj.gameObject.GetComponent<Rogue>();
			rogue.AssimilateRogue(GetComponent<Transform>());

			if( skillIndex < assimilatedLegionSkills.Length - 1 )
			{
				//rogue.LegionSkill = assimilatedLegionSkills[skillIndex++];
			}
			else
			{
				//rogue.LegionSkill = assimilatedLegionSkills[skillIndex];
			}
			// TODO: Add local rogue camera to list.
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
