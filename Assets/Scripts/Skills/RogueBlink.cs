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

	public override bool UseSkill()
	{
        if (isReady)
        {
            if (SkillLogic())
            {
				//Play sound effect
				AudioManager.PlayBlinkSound();

                StartCoroutine(SkillCooldown());
                return true;
            }
        }
        return false;
	}
	
	private bool SkillLogic()
	{
        Ray ray = new Ray((targetTransform.position + targetTransform.forward * blinkMultiplier) + new Vector3(0, 5, 0), Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 12))
        {
            // Hits Floor
            if (hit.collider.gameObject.CompareTag("Floor"))
            {
                targetTransform.position += targetTransform.forward * blinkMultiplier;
                return true;
            }
            else
            {
                ray.origin = targetTransform.position;
                ray.direction = targetTransform.forward;

                if (Physics.Raycast(ray, out hit, 12))
                {
                    targetTransform.position = hit.point - (transform.forward / 4);
                    return true;
                }
            }
        }
        return false;
	}
}
