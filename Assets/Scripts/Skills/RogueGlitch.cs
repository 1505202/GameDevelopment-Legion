using UnityEngine;
using System.Collections;

public class RogueGlitch : ASkill
{
    private bool canTeleport;

    Transform targetTransform;

    private Vector3 startPosition;

    private Vector3 minPosition;
    private Vector3 maxPosition;

	public void Initialize(Transform targetTransform, Vector3 initialPositon, Vector3 minPosition, Vector3 maxPosition, float duration, float cooldown)
	{
        this.targetTransform = targetTransform;

        this.startPosition = initialPositon;

        this.minPosition = minPosition;
        this.maxPosition = maxPosition;
        
        this.duration = duration;
		this.cooldown = cooldown;
	}

    public void Update()
    {
        if (canTeleport)
        {
            targetTransform.position = new Vector3(Random.Range(minPosition.x, maxPosition.x), Random.Range(minPosition.y, maxPosition.y), Random.Range(minPosition.z, maxPosition.z));
        }
    }

	public override bool UseSkill()
	{
		StartCoroutine( SkillLogic(duration) );
		StartCoroutine( SkillCooldown() );
        return true;
	}
	
	public IEnumerator SkillLogic(float time)
	{
        canTeleport = true;
        yield return new WaitForSeconds(duration);
        canTeleport = false;
        targetTransform.position = startPosition;
    }
}
