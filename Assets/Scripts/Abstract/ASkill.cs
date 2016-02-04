using UnityEngine;
using System.Collections;
/// <summary>
/// Abstract Class For All Skills
/// </summary>
public abstract class ASkill : MonoBehaviour, ISkill
{
	protected bool isReady = true;
	protected float cooldown = 0;
	protected float duration = 0;

	public abstract void UseSkill();

	public bool IsReady
	{
		get{ return isReady; }
	}

	protected IEnumerator SkillCooldown(float time)
	{
		isReady = false;
		yield return new WaitForSeconds(time);
		isReady = true;
	}
}