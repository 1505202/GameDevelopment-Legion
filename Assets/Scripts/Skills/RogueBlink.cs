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
		SkillLogic();
		StartCoroutine( SkillCooldown(cooldown) );
	}
	
	private void SkillLogic()
	{
		targetTransform.position += targetTransform.forward * blinkMultiplier;
	}
}
