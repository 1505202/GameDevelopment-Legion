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

	public abstract bool UseSkill();

	public bool IsReady
	{
		get{ return isReady; }
        set { isReady = value; }
	}

	protected IEnumerator SkillCooldown()
	{
		isReady = false;
		yield return new WaitForSeconds(cooldown);
		isReady = true;
	}
}