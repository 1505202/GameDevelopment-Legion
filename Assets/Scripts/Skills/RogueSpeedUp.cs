using UnityEngine;
using System.Collections;

public class RogueSpeedUp : ASkill 
{
	private Rogue rogue = null;

	private float multiplier = 1;

	public void Initialize(Rogue rogue, float multiplier, float duration, float cooldown)
	{
		this.rogue = rogue;

		this.multiplier = multiplier;
		this.duration = duration;
		this.cooldown = cooldown;
	}

	public override bool UseSkill()
	{
		StartCoroutine( SkillLogic(duration) );
		StartCoroutine( SkillCooldown() );
        return true;
	}

	private IEnumerator SkillLogic(float time)
	{
		rogue.MovementOffset *= multiplier;
		yield return new WaitForSeconds(time);
		rogue.MovementOffset /= multiplier;
	}
}
