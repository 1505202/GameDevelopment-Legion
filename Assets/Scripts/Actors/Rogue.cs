using UnityEngine;

using System.Collections;
/// <summary>
/// 
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Rogue : AActor
{
	public enum ERogueState 
	{ 
		RogueState,			// Acts As A Rogue 
		AssimilatingState,  // Plays Assimilation Animaition
		AssimilatedState    // Plays As Legion
	}
	private ERogueState rogueState = ERogueState.RogueState;

	// Rogue Skills
    private int rogueSkillsUnlocked = 3;
    private ASkill[] rogueSkills = new ASkill[3];
	private int skillIndex = 0;

	// Assimilated Skill
    private ASkill legionSkill = null;

	// Cached Components
	private Rigidbody myRigidBody = null;
	private Transform myTransform = null;
	private Transform myChildTransform = null;

	private Transform targetParent = null;

	// Players Input Controller
    private AController inputController;


	private bool canSwitchSkills = true;

	private void Start()
	{
		GameManager.Instance.RegisterRogueElement(this);

		myRigidBody = GetComponent<Rigidbody>();		
		myTransform = GetComponent<Transform>();
		myChildTransform = myTransform.GetChild(0);

		inputController = ControllerManager.Instance.NewController( new JInput( 1 ) );

		RogueStealth stealth = gameObject.AddComponent<RogueStealth>();
		stealth.Initialize(GetComponent<MeshRenderer>(), 3, 8);
		rogueSkills[0] = stealth;
	}

	private void Update()
	{
		if( rogueState == ERogueState.RogueState )
		{

			myRigidBody.velocity = inputController.MoveDirection() * movementSpeed;

			if(inputController.MoveDirection() != Vector3.zero)
			{
				Quaternion lookRotation = Quaternion.LookRotation (inputController.MoveDirection());
				myTransform.rotation = Quaternion.Slerp (myTransform.rotation, lookRotation, Time.deltaTime * rotateSpeed);
				myChildTransform.rotation = Quaternion.Euler( 90, 0, 0 );
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
						if( rogueSkills[0].IsReady )
						{
							rogueSkills[0].UseSkill();
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

		if( rogueState == ERogueState.AssimilatedState )
		{
			if(inputController.FiringPower())
			{
				legionSkill.UseSkill();
			}
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

	public ASkill LegionSkill
	{
		get{ return legionSkill; }
		set{ legionSkill = value; }
	}

	public AController Controller
	{
		get { return inputController; }
		set { inputController = value; }
	}

	#endregion 
}
