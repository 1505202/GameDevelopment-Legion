using UnityEngine;
using System.Collections;

public class RogueGlitch : ASkill
{
    // Camera References
    private Transform mainCameraTransform = null;

    // TeleportLimits
    private Vector3 lowerLimit = Vector3.zero;
    private Vector3 upperLimit = Vector3.zero;

    // Control Variable
    private bool isGlitching = false;

    private Vector3 initialPosition;

    private Vector3 targetPosition;

    public void Initialize(GameObject mainCameraObject, Vector3 minPosition, Vector3 maxPosition, float duration, float interval, float cooldown, float minLengthPercentile, float maxLengthPercentile)
	{
        this.lowerLimit = minPosition;
        this.upperLimit = maxPosition;
        
        this.duration = duration;
		this.cooldown = cooldown;
        
        mainCameraTransform = mainCameraObject.GetComponent<Transform>();

        initialPosition = mainCameraTransform.position;
	}

	public override bool UseSkill()
	{
		StartCoroutine( SkillLogic(duration) );
		StartCoroutine( SkillCooldown() );
        return true;
	}
	
	public IEnumerator SkillLogic(float time)
	{
        isGlitching = true;
        StartCoroutine(RandomizePosition(duration, 0.1f));
        yield return new WaitForSeconds(duration);
        isGlitching = false;
        mainCameraTransform.position = initialPosition;
    }

    private IEnumerator RandomizePosition(float duration, float interval)
    {
        while(duration > 0)
        {
            targetPosition = new Vector3(Random.Range(lowerLimit.x, upperLimit.x), Random.Range(lowerLimit.y, upperLimit.y), Random.Range(lowerLimit.z, upperLimit.z));
            duration -= interval;
            yield return new WaitForSeconds(interval);
        }
    }

    private void Update()
    {
        if (isGlitching)
        {
            //System.Random r = new Random();
            mainCameraTransform.position = Vector3.Lerp(mainCameraTransform.position, targetPosition, Time.deltaTime * 50);
        }
    }
}