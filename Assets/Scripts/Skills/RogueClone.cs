using UnityEngine;
using System.Collections;

public class RogueClone : ASkill 
{
    private GameObject targetObject = null;

    private Rigidbody targetRigidbody = null;

    private AController inputController = null;

    private Transform rogueTransform = null;

    private float movementSpeed = 0;

    private bool isCloneActive = false;

    public void Initialize(Transform rogueTransform, GameObject targetObj, AController inputController, float movementSpeed, float duration, float cooldown)
	{
        this.duration = duration;
        this.cooldown = cooldown;

        this.targetObject = targetObj;

        this.targetRigidbody = targetObj.GetComponent<Rigidbody>();

        this.rogueTransform = rogueTransform;

        this.inputController = inputController;

        this.movementSpeed = movementSpeed;
	}

	public override bool UseSkill()
	{
        StartCoroutine(SkillLogic(duration));
        StartCoroutine(SkillCooldown());
        return true;
	}

    public void Update()
    {
        if (isCloneActive)
        {
            targetRigidbody.velocity = inputController.AimDirection() * movementSpeed;
        }
    }

    private void OnCollisionEnter(Collision obj)
    {
        if (obj.collider.CompareTag("Legion"))
        {
            this.isReady = true;
            targetObject.SetActive(false);
        }
    }

    public IEnumerator SkillLogic(float time)
    {
        isCloneActive = true;
        targetObject.GetComponent<Transform>().position = rogueTransform.position;
        targetObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        isCloneActive = false;
        targetObject.SetActive(false);
    }
}
