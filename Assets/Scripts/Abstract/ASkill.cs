using UnityEngine;
/// <summary>
/// Abstract Class For All Skills
/// </summary>
public abstract class ASkill : MonoBehaviour, ISkill
{
	[SerializeField] protected bool isReady = true;

	public abstract void UseSkill();

	public bool IsReady
	{
		get{ return isReady; }
	}
}