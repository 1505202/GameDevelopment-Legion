using UnityEngine;

using System.Collections;
/// <summary>
/// 
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Rogue : AActor
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

	public enum ERogueState 
	{ 
		RogueState,			// Acts As A Rogue 
		AssimilatingState,  // Plays Assimilation Animaition
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

	private Transform targetParent = null;

	// Players Input Controller
    private AController inputController;

	private float movementOffset = 1;

	private bool canSwitchSkills = true;

	private Vector3 cameraOffset = Vector3.zero;

	private void Start()
	{
		GameManager.Instance.RegisterRogueElement(this);

		myRigidBody = GetComponent<Rigidbody>();		
		myTransform = GetComponent<Transform>();
		myChildTransform = myTransform.GetChild(0);

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

		cameraOffset = myChildTransform.localPosition;
	}

	private void Update()
	{
		if( rogueState == ERogueState.RogueState )
		{
			myRigidBody.velocity = inputController.MoveDirection() * movementSpeed * movementOffset;

			if(inputController.MoveDirection() != Vector3.zero)
			{
				Quaternion lookRotation = Quaternion.LookRotation (inputController.MoveDirection());
				myTransform.rotation = Quaternion.Slerp (myTransform.rotation, lookRotation, Time.deltaTime * rotateSpeed);
				myChildTransform.rotation = Quaternion.Euler( 55, 0, 0 );
				myChildTransform.position = myTransform.position + cameraOffset;
			}

			if( rogueSkillsUnlocked > 0 )
			{
				if( inputController.SwitchingPower() && canSwitchSkills )
				{
					SwitchSkill();
					StartCoroutine(SwitchPowerCD(0.5f));
				}

				if( inputController.FiringPower() )
				{
					if(rogueSkillsUnlocked > 0)
					{
						if( rogueSkills[skillIndex].IsReady )
						{
							rogueSkills[skillIndex].UseSkill();
						}
					}
				}
			}
		}
		
		if( rogueState == ERogueState.AssimilatingState )
		{
			myTransform.position = Vector3.Lerp(myTransform.position, targetParent.position, Time.time * 2);
			myTransform.localScale = Vector3.Lerp(myTransform.localScale, Vector3.zero, Time.time * 2);
		}
	}

    /// <summary>
    /// 
    /// </summary>
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
	public void AssimilateRogue(Transform target)
	{
		targetParent = target;
		rogueState = ERogueState.AssimilatingState;
		GetComponent<Collider>().enabled = false;
		myRigidBody.isKinematic = true;
		GameManager.Instance.RemoveRogueElement(this);
		StartCoroutine(AssimilationControl(1));
	}

	/// <summary>
	/// Controls Cooldown Timers
	/// </summary>
	/// <param name="time">Time for the cooldown</param>
	private IEnumerator SwitchPowerCD(float time) // Use Lambda Expresion/Action
	{
		canSwitchSkills = false;
		yield return new WaitForSeconds(time);
		canSwitchSkills = true;
	}

	private IEnumerator AssimilationControl(float time)
	{
		yield return new WaitForSeconds(time);
		rogueState = ERogueState.AssimilatedState;
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
