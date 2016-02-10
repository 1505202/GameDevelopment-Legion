using UnityEngine;

using System.Collections;

using UnityEngine.Networking;
/// <summary>
/// 
/// </summary>
public class Rogue : AActor, IAssimilatable
{

	[Header("Skill Variables")]
	[Header("Rogue Speed Buff")]
	[SerializeField] private float speedMultiplier = 1;
	[SerializeField] private float speedDuration = 1;
	[SerializeField] private float speedCooldown = 1;
	
	[Header("Rogue Blink")]
	[SerializeField] private float blinkDistance = 1;
	[SerializeField] private float blinkCooldown = 1;
	
	[Header("Rogue Invisibility")]
	[SerializeField] private float invisibilityDuration = 1;
	[SerializeField] private float invisibilityCooldown = 1;

	[SyncVar] private int assimilatedBehaviour = 0;

	public enum ERogueState 
	{ 
		RogueState,			// Acts As A Rogue 
		AssimilatedState    // Plays As Legion
	}
	private ERogueState rogueState = ERogueState.RogueState;

	// Rogue Skills
	private int skillIndex = 0;
    private int rogueSkillsUnlocked = 3;
    private ASkill[] rogueSkills = new ASkill[3];

	// Cached Components
	private Rigidbody myRigidBody = null;
	private Transform myTransform = null;
	private Transform myChildTransform = null;

	private Transform target = null;
	private LineRenderer line = null;
	
	// Players Input Controller
    private AController inputController = null;

	private float movementOffset = 1;

	private bool canSwitchSkills = true;

	private Vector3 cameraOffset = Vector3.zero;
	private Vector3 cameraRotation = Vector3.zero;



	[SyncVar] private Vector3 lineStartPoint = Vector3.zero;
	[SyncVar] private Vector3 lineEndPoint = Vector3.zero;

	private void Start()
	{
		GameManager.Instance.RegisterRogueElement(this);

		myRigidBody = GetComponent<Rigidbody>();		
		myTransform = GetComponent<Transform>();

		inputController = ControllerManager.Instance.NewController( new JInput( 1 ) );

		RogueSpeedUp speedBoost = gameObject.AddComponent<RogueSpeedUp>();
		speedBoost.Initialize(this, speedMultiplier, speedDuration, speedCooldown);
		rogueSkills[0] = speedBoost;

		RogueBlink dash = gameObject.AddComponent<RogueBlink>();
		dash.Initialize(GetComponent<Transform>(), blinkCooldown, blinkDistance);
		rogueSkills[1] = dash;

		RogueStealth stealth = gameObject.AddComponent<RogueStealth>();
		stealth.Initialize(GetComponent<MeshRenderer>(), invisibilityDuration, invisibilityCooldown);
		rogueSkills[2] = stealth;

		if (isLocalPlayer) 
		{
			myChildTransform = myTransform.GetChild (0);
			cameraOffset = myChildTransform.localPosition / 1.5f; // Magic Numbers Are Real
			cameraRotation = myChildTransform.rotation.eulerAngles;
			myChildTransform.gameObject.SetActive(true);
		}
	}
	private void Update()
	{
		// Controls The Sync Var To Switch To An Assimilated Behaviour Change
		if( rogueState != ERogueState.AssimilatedState && assimilatedBehaviour != 0 )
		{
			SwitchActorBehaviour();
			rogueState = ERogueState.AssimilatedState;
		}

		// Returns If It Isnt A Local Player
		if (!isLocalPlayer)
		{
			if(line != null)
			{
				line.SetPosition(0, lineStartPoint);
				line.SetPosition(1, lineEndPoint);
			}
			return;
		}

		switch(assimilatedBehaviour)
		{
		case 0:
			RogueBehaviour();
			return;
		case 1:
			BeamBehaviour();
			return;
		case 2:
			TetherBehaviour();
			return;
		case 3:
			ProbeBehaviour();
			return;
		}

	}

	private void RogueBehaviour()
	{
		/// Rogue Behaviour
		/// Translation and Rotation Handling
		myRigidBody.velocity = inputController.MoveDirection() * movementSpeed * movementOffset;
		
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
		// Skill handling
		if (rogueSkillsUnlocked > 0)
		{
			if (inputController.SwitchingPower() && canSwitchSkills)
			{
				SwitchSkill();
				StartCoroutine(SwitchPowerCD(0.5f));
			}
			
			if (inputController.FiringPower())
			{
				if (rogueSkillsUnlocked > 0)
				{
					if (rogueSkills[skillIndex].IsReady)
					{
						rogueSkills[skillIndex].UseSkill();
					}
				}
			}
		}
	}
	private void BeamBehaviour()
	{
		myTransform.position = target.position;
		
		if( inputController.FiringPower() )
		{
			Vector3 dir = inputController.AimDirection();
			if( dir == Vector3.zero )
			{
				dir = target.forward;
			}
			
			line.enabled = true;
			Ray ray = new Ray(transform.position, dir);
			RaycastHit hit;
			if( Physics.Raycast(ray, out hit, 10) )
			{
				CmdUpdateLineRenderer(transform.position, hit.point);

				line.SetPosition(0, lineStartPoint);
				line.SetPosition(1, lineEndPoint);
				
				Rogue rogue = hit.collider.gameObject.GetComponent<Rogue>();
				if(rogue != null)
				{
					rogue.MovementOffset = 0.2f;
				}
			}
			else
			{
				CmdUpdateLineRenderer(transform.position, ray.GetPoint(10));
				
				line.SetPosition(0, lineStartPoint);
				line.SetPosition(1, lineEndPoint);
			}
		}
		else
		{
			lineStartPoint = Vector3.zero;
			lineEndPoint = Vector3.zero;
			
			line.enabled = false;
		}
	}
	private void TetherBehaviour(){}
	private void ProbeBehaviour(){}


	[Command]
	private void CmdUpdateLineRenderer(Vector3 positionA, Vector3 positionB)
	{
		lineStartPoint = positionA;
		lineEndPoint = positionB;
	}

	[Command]
	public void CmdAssimilate()
	{
		assimilatedBehaviour = GameManager.Instance.AssimilatedRogueCount();
	}

	private void OnCollisionEnter(Collision obj)
	{
		if(obj.gameObject.CompareTag("Legion"))
		{
			CmdAssimilate();
		}
	}


	/// Skill Controlling Functions
	public void UpdateRogueSkillCount()
    {
        rogueSkillsUnlocked++;
    }
	private void SwitchSkill()
	{
		if(++skillIndex >= rogueSkillsUnlocked)
		{
			skillIndex = 0;
		}
	}
	private IEnumerator SwitchPowerCD(float time) // Use Lambda Expresion/Action
	{
		canSwitchSkills = false;
		yield return new WaitForSeconds(time);
		canSwitchSkills = true;
	}

	/// Actor Switching Function
	public void SwitchActorBehaviour()
	{
		if( assimilatedBehaviour == 1 )
		{
			Destroy(GetComponent<MeshFilter>());
			Destroy(GetComponent<MeshRenderer>());
			Destroy(GetComponent<Collider>());
			GetComponent<Rigidbody>().isKinematic = true; // Networking Requires It
			line = gameObject.AddComponent<LineRenderer>();
			target = GameObject.FindGameObjectWithTag("Legion").GetComponent<Transform>();
		}
		else if( assimilatedBehaviour == 2 )
		{
		}
		else if( assimilatedBehaviour == 3 )
		{
		}

		for(int i = 0; i < rogueSkills.Length; i++)
		{
			Destroy(rogueSkills[i]);
		}
	}

	#region Properties

	public ERogueState RogueState
	{
		get { return rogueState; }
		set { rogueState = value; }
	}

	public AController Controller
	{
		get { return inputController; }
		set { inputController = value; }
	}

	public float MovementOffset
	{
		get { return movementOffset; }
		set { movementOffset = value; }
	}

	#endregion 
}
