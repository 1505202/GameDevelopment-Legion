using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Rogue : AActor, IAssimilatable
{
    [Header("Skill Variables")]

    //[Header("Example Skill")]
    //[SerializeField] private float skillVariable = null;
    [SerializeField] private GameObject mBlinkParticleObject = null;

    [SerializeField] private float skillResource;

    [Header("Lighting")]
    [SerializeField] private float maxIntensity = 5;
    [SerializeField] private Light lightSource = null;

    private int assimilatedBehaviour;

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

    #endregion

    public enum ERogueState
    {
        RogueState,         // Acts As A Rogue 
        AssimilatedState    // Plays As Legion
    }

    private enum BehaviourType
    {
        Rogue = 0,
        Assimilated
    };

    private ERogueState rogueState = ERogueState.RogueState;

    // Rogue Skills
    private ASkill[] rogueSkills = new ASkill[3];

    // Cached Components
    private Rigidbody myRigidBody;
    private Transform myTransform;

    // Players Input Controller
    private AController inputController;

    private bool hasCollidedWithLegion;
    bool canMove = true;

    // Use this for initialization
    void Start () {
        myRigidBody = GetComponent<Rigidbody>();
        myTransform = GetComponent<Transform>();
        inputController = ControllerManager.Instance.NewController();
        score = 0;
        Team = rogueTeamName;

        // INITIALIZE SKILLS

        //rogueSkills = new RogueBlink[3];
        for (int i = 0; i < rogueSkills.Length; i++)
        {
            RogueBlink tempRogueSkill = gameObject.AddComponent<RogueBlink>();
            tempRogueSkill.Initialize(GetComponent<Transform>(), 5.0f, 1.0f, mBlinkParticleObject);


            rogueSkills[i] = tempRogueSkill;
        }

        lightSource.intensity = maxIntensity;
    }
	
	// Update is called once per frame
	void Update () {

        if (GameManager.Instance.IsGameOver || GameManager.Instance.IsInLobby())
            return;

        // Skill & Movement handling according to current behaviour
        switch ((BehaviourType)assimilatedBehaviour)
        {
            case BehaviourType.Rogue:
                RogueBehaviour();
                return;
            case BehaviourType.Assimilated:
                AssimilatedBehaviour();
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
        for (int i = 0; i < rogueSkills.Length; i++)
        {
            //Check if button for skill is pressed and skill is ready to fire
            if (inputController.GetButton((ControllerInputKey)i) && rogueSkills[i].IsReady)
            {
                //Use skill
                if (rogueSkills[i].UseSkill())
                {
                    //ReduceResource
                    skillResource = 0;
                }
            }
        }
    }

    private void AssimilatedBehaviour()
    {
        if (!canMove)
        {
            myRigidBody.velocity = Vector2.zero;
            return;
        }

       HandleMoveInput();

        // Skill handling
        for (int i = 0; i < rogueSkills.Length; i++)
        {
            //Check if button for skill is pressed and skill is ready to fire
            if (inputController.GetButton((ControllerInputKey)i) && rogueSkills[i].IsReady)
            {
                //Use skill
                if (rogueSkills[i].UseSkill())
                {
                    //ReduceResource
                    skillResource = 0;
                }
            }
        }
    }



    private void OnCollisionEnter(Collision obj)
    {
        if (obj.gameObject.CompareTag("Legion") && assimilatedBehaviour == (int)BehaviourType.Rogue)
        {
            hasCollidedWithLegion = true;
            Assimilate();
        }
    }

    public void Assimilate()
    {
        GameManager.Instance.Assimilate(this);
        assimilatedBehaviour = GameManager.Instance.GetBehaviourIndex();

        SwitchActorBehaviour();

        if (GameManager.Instance.audioEnabled) AudioManager.PlayAssimilationSound();
    }

    public void SwitchActorBehaviour()
    {
        SetRogueColors(GameManager.Instance.LegionColor);

        for (int i = 0; i < rogueSkills.Length; i++)
        {
            Destroy(rogueSkills[i]);
        }
    }

    private void HandleMoveInput()
    {
        /// Rogue Behaviour
        /// Translation and Rotation Handling
        /// TODO: THIS LINE CAUSES FIRST ROGUE TO BE STATIC, SECOND ROGUE TO HAVE NO FRICTION
        myRigidBody.velocity = (inputController.MoveDirection() * movementSpeed) + new Vector3(0, myRigidBody.velocity.y, 0);

        if (inputController.MoveDirection() != Vector3.zero)
        {
            myTransform.rotation = Quaternion.LookRotation(inputController.MoveDirection());
        }
    }

    public void SetRogueColors(Color color)
    {
        //Set the color of all the rogue's meshes
        Renderer[] children = GetComponentsInChildren<Renderer>();
        foreach(Renderer rend in children)
        {
            rend.material.color = color;
        }

        //Set color of the light source
        lightSource.color = color;
    }
}
