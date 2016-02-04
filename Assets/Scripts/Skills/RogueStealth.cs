using UnityEngine;
using System.Collections;

public class RogueStealth : ASkill, ISkill
{
	private float duration = 0;
	private float cooldown = 0;

	private MeshRenderer meshRenderer;

	public void Initialize(MeshRenderer meshRenderer, float duration, float cooldown)
	{
		this.meshRenderer = meshRenderer;
		this.duration = duration;
		this.cooldown = cooldown;
	}

	public override void UseSkill()
	{
		StartCoroutine( TimeToUse(duration) );
		StartCoroutine( Cooldown(cooldown) );
	}
	
	public IEnumerator TimeToUse(float time)
	{
		meshRenderer.enabled = false;
		yield return new WaitForSeconds(duration);
		meshRenderer.enabled = true;
	}

	private IEnumerator Cooldown(float time)
	{
		isReady = false;
		yield return new WaitForSeconds(time);
		isReady = true;
	}
}
