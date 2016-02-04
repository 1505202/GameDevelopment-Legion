using UnityEngine;
using System.Collections;

public class RogueStealth : ASkill
{
	private MeshRenderer meshRenderer;

	public void Initialize(MeshRenderer meshRenderer, float duration, float cooldown)
	{
		this.meshRenderer = meshRenderer;
		this.duration = duration;
		this.cooldown = cooldown;
	}

	public override void UseSkill()
	{
		StartCoroutine( SkillLogic(duration) );
		StartCoroutine( SkillCooldown(cooldown) );
	}
	
	public IEnumerator SkillLogic(float time)
	{
		meshRenderer.enabled = false;
		yield return new WaitForSeconds(duration);
		meshRenderer.enabled = true;
	}
}
