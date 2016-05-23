using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Rogue_New : AActor, IAssimilatable
{
    [Header("Skill Variables")]

    //[Header("Example Skill")]
    //[SerializeField] private float skillVariable = null;

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

        AudioManager.PlayAssimilationSound();
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
        myRigidBody.velocity = (inputController.MoveDirection() * movementSpeed) + new Vector3(0, myRigidBody.velocity.y, 0);

        if (inputController.MoveDirection() != Vector3.zero)
        {
            myTransform.rotation = Quaternion.LookRotation(inputController.MoveDirection());
        }
    }

    public void SetRogueColors(Color color)
    {
        GetComponent<Renderer>().material.color = color;
        lightSource.color = color;
    }
}
