using UnityEngine;
using System.Collections;

public class RogueBlink : ASkill 
{
	private Transform targetTransform;
	private float blinkMultiplier;
    private GameObject particlePrefab;

	public void Initialize(Transform targetTransform, float cooldown, float blinkMultiplier, GameObject particles)
	{
		this.targetTransform = targetTransform;

		this.cooldown = cooldown;
		this.blinkMultiplier = blinkMultiplier;

        this.particlePrefab = particles;
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
        } return false;
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
                CreateParticleSystemAt(targetTransform.position);
                targetTransform.position += targetTransform.forward * blinkMultiplier;
                CreateParticleSystemAt(targetTransform.position);
            }
            else
            {
                ray.origin = targetTransform.position;
                ray.direction = targetTransform.forward;

                if (Physics.Raycast(ray, out hit, 12))
                {
                    targetTransform.position = hit.point - (transform.forward / 4);
                }
            }
            return true;
        }
        return false;
	}

    private void CreateParticleSystemAt(Vector3 position)
    {
        GameObject tempParticles = Instantiate(particlePrefab, position, Quaternion.identity ) as GameObject;

        //tempParticles.GetComponent<ParticleSystem>().Emit(1);
    }
}
