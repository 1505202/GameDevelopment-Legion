using UnityEngine;
/// <summary>
/// Abstract Class For All Skills
/// </summary>
public abstract class ASkill : MonoBehaviour, ISkill
{
	[SerializeField] protected float skillCooldown = 0;
	[SerializeField] protected bool skillAvailable = true;

	public abstract void UseSkill();	
}