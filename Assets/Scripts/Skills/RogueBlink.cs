using UnityEngine;
using System.Collections;

public class RogueBlink : ASkill 
{
	private Transform targetTransform;
	private float blinkMultiplier;

	public void Initialize(Transform targetTransform, float cooldown, float blinkMultiplier)
	{
		this.targetTransform = targetTransform;

		this.cooldown = cooldown;
		this.blinkMultiplier = blinkMultiplier;
	}

	public override void UseSkill()
	{
        if (SkillLogic())
        {
            StartCoroutine(SkillCooldown(cooldown));
        }
	}
	
	private bool SkillLogic()
	{
        Ray ray = new Ray(targetTransform.forward * blinkMultiplier + new Vector3(0, 10, 0), Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 12))
        {
            if (hit.collider.gameObject.CompareTag("Floor"))
            {
                targetTransform.position += targetTransform.forward * blinkMultiplier;
                return true;
            }
        }
        return false;
	}
}
