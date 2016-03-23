using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 
/// </summary>
public class Rogue : AActor, IAssimilatable
{
	[Header("Skill Variables")]
    [Space(10)]
	[Header("Rogue Blink")]
    [SerializeField] private float blinkDistance = 1;
    [SerializeField] private float blinkCooldown = 1;

    [Header("Rogue Blink")]
    [SerializeField] private GameObject cloneObject = null;
    [SerializeField] private float cloneDuration = 1;
    [SerializeField] private float cloneCooldown = 1;

    [Header("Rogue Glitch")]
    [SerializeField] private float glitchDuration = 0;
    [SerializeField] private float glitchCooldown = 0;
    [SerializeField] private Vector3 higherLimits = Vector3.zero;
    [SerializeField] private Vector3 lowerLimits = Vector3.zero;

	[Header("Assimilated Skills")]
	[SerializeField] private float tetherMaxDistance = 0;

    [Header("Lighting")]
    [SerializeField] private float maxIntensity = 5;

    [Header("Rogue Sub Mesh MeshRenderer")]
    [SerializeField] private MeshRenderer[] subMeshes;

    // TODO: Ensure these are used or removed

    // [Header("Assimilated Meshes")]
    // [SerializeField] private Mesh tetherMesh = null;
    // [SerializeField] private Mesh cannonMesh = null;
    // [SerializeField] private Mesh trailBlazerMesh = null;
	
	private int assimilatedBehaviour = 0;
	private Vector3 lineStartPoint = Vector3.zero;
	private Vector3 lineEndPoint = Vector3.zero;

	public enum ERogueState 
	{ 
		RogueState,			// Acts As A Rogue 
		AssimilatedState    // Plays As Legion
	}

	private enum BehaviourType
	{
		Rogue = 0,
		Tether,
		Cannonball,
		TrailBlazer
	};

	private ERogueState rogueState = ERogueState.RogueState;

	// Rogue Skills
	private int skillIndex = 0;
    private int rogueSkillsUnlocked = 0;
    private ASkill[] rogueSkills = new ASkill[3];

	// Cached Components
	private Rigidbody myRigidBody = null;
	private Transform myTransform = null;

	private Transform target = null;
	private LineRenderer line = null;
	
	// Players Input Controller
    private AController inputController = null;

	private float movementOffset = 1;
	private bool canSwitchSkills = true;
	private bool hasCollidedWithLegion = false;

    [SerializeField] private Light lightSource;

	// cannonball 
	private bool isPropelled = false;
	private Vector3 propelledDirection = Vector3.zero;
	[SerializeField] private float propelledVelocity = 14;
    [SerializeField] private float stunDuration = 0; 
    bool canMove = true;

    [Header("Cannon Reticle")]
    [SerializeField] private GameObject cannonReticle;

    [Header("Particle Prefabs")]
    [SerializeField] private GameObject blinkParticlePrefab;
    [SerializeField] private GameObject cannonParticlePrefab;

	public GameObject trailBlazerPrefab;
    public Vector3 trailBlazerDropOffset;

    /// <summary>
    /// To Force The Animator To Transition To A Shape On Caught, use The Following
    /// Parameter Name : SwitchToModel
    /// Tiangle = 1
    /// Circle  = 2
    /// Cross   = 3
    /// Square  = 4
    /// 
    /// Note: 0 Reserved So That The Animations Swap Before Being Caught
    /// </summary>
    private Animator animator;
    


    /// <summary>
    /// /////////////////////////////////////
    /// </summary>
    [SerializeField] private LayerMask layerMask;

    private ConfigurableJoint joint;

    private List<Vector3> contactPoints = new List<Vector3>();
    private List<GameObject> intermediatePoints = new List<GameObject>();

    private SoftJointLimit limit = new SoftJointLimit();


	private void Start()
	{
        animator = GetComponent<Animator>();

		myRigidBody = GetComponent<Rigidbody>();
        myTransform = GetComponent<Transform>();

		inputController = ControllerManager.Instance.NewController();

        
        // Temporary Change Until New Skills Are Added
		RogueBlink dash = gameObject.AddComponent<RogueBlink>();
		dash.Initialize(GetComponent<Transform>(), blinkCooldown, blinkDistance, blinkParticlePrefab);

        RogueClone clone = gameObject.AddComponent<RogueClone>();
        clone.Initialize(myTransform, cloneObject, inputController, base.movementSpeed, cloneDuration, cloneCooldown);

        RogueGlitch glitch = gameObject.AddComponent<RogueGlitch>();
        glitch.Initialize(Camera.main.transform, Camera.main.transform.position, lowerLimits, higherLimits, glitchDuration, glitchCooldown);

        rogueSkills[0] = dash;
        rogueSkills[1] = clone;
        rogueSkills[2] = glitch;



	}
	private void Update()
	{
	    if (GameManager.Instance.IsGameOver)
	        return;

        cloneObject.GetComponent<Animator>().SetBool("Start", true);
        animator.SetBool("Start", true);

		if(line != null)
		{
			line.SetPosition(0, lineStartPoint);
			line.SetPosition(1, lineEndPoint);
		}


        HandleGlobalCooldownLight();

		switch((BehaviourType)assimilatedBehaviour)
		{
		case BehaviourType.Rogue:
			RogueBehaviour();
			return;
		case BehaviourType.Tether:
			TetherBehaviour();
			return;
		case BehaviourType.Cannonball:
			CannonballBehaviour();
			return;
		case BehaviourType.TrailBlazer:
            TrailblazerBehaviour();
			return;
		}

	}

	private void RogueBehaviour()
	{
		if (!canMove) 
		{
            myRigidBody.velocity = Vector2.zero;
            return; 
		}

		HandleMoveInput();

		// Skill handling
		if (rogueSkillsUnlocked > 0)
		{
			if (inputController.SwitchingPower() && canSwitchSkills)
			{
				SwitchSkill();
				StartCoroutine(SwitchPowerCD(0.5f));
			}
			
			if (inputController.FiringPower() && rogueSkills[skillIndex].IsReady)
			{
                if (rogueSkills[skillIndex].UseSkill())
                {
                    lightSource.intensity = 0;
                }
			}
		}
	}
	private void TetherBehaviour()
	{
        HandleMoveInput();

        // updating LineRenderer Points
        UpdateEndPoints();
        UpdateLineRenderer();

        UpdateJointLimits();

        CheckRogueCollision();

        if (UnWrap())
        {
            return;
        }

        if (Wrap())
        {
            return;
        }
	}
    private void CannonballBehaviour()
    {
		// If FIRE button is pressed, propel forward.
		if (!isPropelled && inputController.FiringPower ()) 
		{
			isPropelled = true;
			propelledDirection = inputController.MoveDirection().normalized;
			AudioManager.PlayCannonballFireSound();
		}

		if (isPropelled) 
		{
			myRigidBody.velocity = propelledDirection * propelledVelocity;
		}


    }
    private void TrailblazerBehaviour()
    {
        if (inputController.MoveDirection() != Vector3.zero)
        {
            float x = Mathf.Abs(inputController.MoveDirection().x);
            float z = Mathf.Abs(inputController.MoveDirection().z);

            if (x > z)
            {
                myRigidBody.velocity = new Vector3(inputController.MoveDirection().x, 0, 0) * movementSpeed;
            }
            if (z > x)
            {
                myRigidBody.velocity = new Vector3(0, 0, inputController.MoveDirection().z) * movementSpeed;
            }
            Instantiate(trailBlazerPrefab, myTransform.position, Quaternion.identity);
        }
        else
        {
            myRigidBody.velocity = Vector3.zero;
        }
    }

	private void OnCollisionEnter(Collision obj)
	{
        if (gameObject.CompareTag("CannonBall"))
        {
            Instantiate(cannonParticlePrefab, transform.position, Quaternion.Euler(90, 0, 0));
        }

        if (obj.gameObject.CompareTag("Legion") && hasCollidedWithLegion)
            return;

		if(obj.gameObject.CompareTag("Legion") && assimilatedBehaviour == (int)BehaviourType.Rogue)
		{
			hasCollidedWithLegion = true;
			Assimilate();
		}
        
		if (assimilatedBehaviour == (int)BehaviourType.Cannonball) 
		{
            myRigidBody.velocity = Vector3.zero;

			if(obj.gameObject.CompareTag("Rogue") && isPropelled == true)
			{
                StartCoroutine(StunRogue(obj.gameObject.GetComponent<Rogue>()));
                myTransform.position = target.position - target.forward;
            }
			isPropelled = false;
		}
	}
    // Controls Consistant Wall Collisions For CannonBall
    private void OnCollisionStay(Collision obj)
    {
        if (obj.gameObject.CompareTag("Floor"))
        {
            return;
        }

        if (assimilatedBehaviour == (int)BehaviourType.Cannonball)
        {
            myRigidBody.velocity = Vector3.zero;

            if (obj.gameObject.CompareTag("Rogue") && isPropelled == true)
            {
                StartCoroutine ( StunRogue(obj.gameObject.GetComponent<Rogue>()) ) ;
                myTransform.position = target.position - target.forward;
            }
            isPropelled = false;
        }
    }

	private void UpdateLineRendererPoints(Vector3 positionA, Vector3 positionB)
	{
		lineStartPoint = positionA;
		lineEndPoint = positionB;
	}

	public void Assimilate()
	{
		GameManager.Instance.Assimilate(this);
        assimilatedBehaviour = GameManager.Instance.GetBehaviourIndex();
        
        SwitchActorBehaviour();

		AudioManager.PlayAssimilationSound ();
	}
	public void UpdateRogueSkillCount()
    {
        if(rogueSkillsUnlocked < rogueSkills.Length)
            rogueSkillsUnlocked++;
    }
    public void SwitchActorBehaviour()
    {
        // HACK: Debug Code To Force Assimilation, Delete After Testing Phase
        //assimilatedBehaviour = (int)BehaviourType.Cannonball;

		if (assimilatedBehaviour == (int)BehaviourType.Tether)
        {
            target = GameObject.FindGameObjectWithTag("Legion").GetComponent<Transform>();

            myTransform = GetComponent<Transform>();

            contactPoints.Add(myTransform.position);
            contactPoints.Add(target.position);

            line = GetComponent<LineRenderer>();

            joint = gameObject.AddComponent<ConfigurableJoint>();
            joint.connectedBody = target.GetComponent<Rigidbody>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = Vector3.zero;
            joint.xMotion = ConfigurableJointMotion.Limited;
            joint.yMotion = ConfigurableJointMotion.Limited;
            joint.zMotion = ConfigurableJointMotion.Limited;

            limit.limit = tetherMaxDistance;
            joint.linearLimit = limit;

            movementSpeed *= 3.5f;
            animator.SetInteger("SwitchToModel", 3); // Transition Model To Cross

            gameObject.tag = "Untagged";
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
		else if (assimilatedBehaviour == (int)BehaviourType.Cannonball)
        {
            animator.SetInteger("SwitchToModel", 2); // Transition Model To Circle

            /*
             * if cannonReticle is !active
             * then cannonreticle.setActive(true)
             * */
            cannonReticle.SetActive(true);

            gameObject.tag = "CannonBall";
            gameObject.layer = LayerMask.NameToLayer("Default");

            target = GameObject.FindGameObjectWithTag("Legion").GetComponent<Transform>();
        }
        else if (assimilatedBehaviour == (int)BehaviourType.TrailBlazer)
        {
            animator.SetInteger("SwitchToModel", 4); // Transition Model To Square

            gameObject.tag = "Untagged";
            gameObject.layer = LayerMask.NameToLayer("Default");

            movementSpeed *= 2;
        }
        else
        {
            animator.SetInteger("SwitchToModel", 1); // Transition Model To Square
        }


        SetRogueColors( GameManager.Instance.LegionColor );


        for (int i = 0; i < rogueSkills.Length; i++)
        {
            Destroy(rogueSkills[i]);
        }
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

	private IEnumerator StunRogue(Rogue rogue)
	{
        rogue.canMove = false;
		AudioManager.PlayCannonballStunSound ();
        yield return new WaitForSeconds(stunDuration);
        rogue.canMove = true;
	}

	private void HandleMoveInput()
	{
		/// Rogue Behaviour
		/// Translation and Rotation Handling
        myRigidBody.velocity = (inputController.MoveDirection() * movementSpeed * movementOffset) + new Vector3(0, myRigidBody.velocity.y, 0);

        if (inputController.MoveDirection() != Vector3.zero)
        {
            myTransform.rotation = Quaternion.LookRotation(inputController.MoveDirection());
            //myTransform.rotation = Quaternion.Slerp(myTransform.rotation, lookRotation, Time.deltaTime * rotateSpeed);
        }
    }

    private void HandleGlobalCooldownLight()
    {
        lightSource.intensity = Mathf.Lerp(lightSource.intensity, maxIntensity, Time.deltaTime / blinkCooldown);
    }

    #region Tether and wrapping

    private void UpdateEndPoints()
    {
        contactPoints[0] = myTransform.position;
        contactPoints[contactPoints.Count - 1] = target.position;
    }
    private void UpdateLineRenderer()
    {
        line.SetVertexCount(contactPoints.Count);
        for (int i = 0; i < contactPoints.Count; i++)
        {
            line.SetPosition(i, contactPoints[i]);
        }
    }

    private void UpdateJointLimits()
    {
        if (contactPoints.Count > 2)
        {
            // Get Total Rope Distance
            float totalDistanceFromParent = 0;
            for (int i = 0; i < contactPoints.Count - 1; i++)
            {
                totalDistanceFromParent += Vector3.Distance(contactPoints[i], contactPoints[i + 1]);
            }
            //Debug.Log(totalDistanceFromParent);

            if (totalDistanceFromParent > tetherMaxDistance)
            {
                float delta = totalDistanceFromParent - tetherMaxDistance;
                float epsilon = 0.000001f; // floating point values cannot be guarenteed to be exactly 0
                while (delta > epsilon)
                {
                    float closestPointDistance = Vector3.Distance(contactPoints[0], contactPoints[1]);

                    float limitDistance = delta > closestPointDistance ? 0 : (closestPointDistance - delta);

                    limit.limit = limitDistance;
                    joint.linearLimit = limit;

                    if (limitDistance == 0)
                    {
                        contactPoints.RemoveAt(1);
                        if (intermediatePoints.Count > 0)
                            intermediatePoints.RemoveAt(0);

                        if (intermediatePoints.Count > 0)
                        {
                            joint.connectedBody = intermediatePoints[0].GetComponent<Rigidbody>();
                        }
                        else
                        {
                            joint.connectedBody = target.GetComponent<Rigidbody>();
                        }
                    }
                    delta -= closestPointDistance - limitDistance;
                }
            }


        }
        // Reset Connected Body To Closest Point [Can Be Optimised by placing an event sorta system]
        if (intermediatePoints.Count > 0)
        {
            joint.connectedBody = intermediatePoints[0].GetComponent<Rigidbody>();
        }
        else
        {
            joint.connectedBody = target.GetComponent<Rigidbody>();
            limit.limit = tetherMaxDistance;
            joint.linearLimit = limit;
        }
    }

    private bool UnWrap()
    {
        if (contactPoints.Count > 2)
        {
            RaycastHit hit;
            //// UnWrap From Target Side
            if (!Physics.Linecast(contactPoints[contactPoints.Count - 1], contactPoints[contactPoints.Count - 2], out hit, layerMask))
            {
                if (!Physics.Linecast(contactPoints[contactPoints.Count - 1], contactPoints[contactPoints.Count - 3], out hit, layerMask))
                {
                    contactPoints.RemoveAt(contactPoints.Count - 2);

                    GameObject obj = intermediatePoints[intermediatePoints.Count - 1];
                    intermediatePoints.Remove(obj);
                    Destroy(obj);

                    return true;
                }
            }

            // UnWrap From MySide
            if (!Physics.Linecast(contactPoints[0], contactPoints[1], out hit, layerMask))
            {
                if (!Physics.Linecast(contactPoints[0], contactPoints[2], out hit, layerMask)) // Have to see both to be a valid unwrappoint
                {
                    contactPoints.RemoveAt(1);

                    GameObject obj = intermediatePoints[intermediatePoints.Count - 1];
                    intermediatePoints.Remove(obj);
                    Destroy(obj);

                    return true;
                }
            }
        }
        return false;
    }
    private bool Wrap()
    {
        RaycastHit hit;
        // Wrap From Target Side
        if (Physics.Linecast(contactPoints[contactPoints.Count - 1], contactPoints[contactPoints.Count - 2], out hit, layerMask))
        {
            GameObject obj = new GameObject("ContactPoint" + (contactPoints.Count - 1), typeof(Rigidbody));
            Vector3 dirDiff = (hit.point - hit.collider.transform.position).normalized / 10; // So the normalized vector is substantially smaller
            obj.GetComponent<Transform>().position = hit.point + dirDiff;
            obj.GetComponent<Rigidbody>().isKinematic = true;
            contactPoints.Insert(contactPoints.Count - 1, hit.point + dirDiff);
            intermediatePoints.Add(obj);
            return true;
        }

        // Wrap From MySide
        if (Physics.Linecast(contactPoints[0], contactPoints[1], out hit, layerMask))
        {
            GameObject obj = new GameObject("ContactPoint" + (contactPoints.Count - 1), typeof(Rigidbody));
            Vector3 dirDiff = (hit.point - hit.collider.transform.position).normalized / 10;
            obj.GetComponent<Transform>().position = hit.point + dirDiff;
            obj.GetComponent<Rigidbody>().isKinematic = true;
            contactPoints.Insert(1, hit.point + dirDiff);
            intermediatePoints.Add(obj);
            return true;
        }
        return false;
    }

    private void CheckRogueCollision()
    {
        RaycastHit hit;
        for (int i = 0; i < contactPoints.Count - 1; i++)
        {
            if (Physics.Linecast(contactPoints[i], contactPoints[i + 1], out hit, (1 << LayerMask.NameToLayer("Rogue")) ))
            {
                hit.collider.gameObject.GetComponent<Rogue>().Assimilate();
            }
        }
    }

    #endregion

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

    public void SetRogueColors(Color color)
    {
        for (int i = 0; i < subMeshes.Length; i++)
        {
            subMeshes[i].material.color = color;
        }
        cloneObject = Instantiate(cloneObject, Vector3.zero, Quaternion.identity) as GameObject;

        cloneObject.GetComponent<RogueCloneMeshReferences>().UpdateCloneColors(color);
        lightSource.color = color;
    }
}
