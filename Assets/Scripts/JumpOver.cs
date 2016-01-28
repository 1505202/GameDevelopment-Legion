using UnityEngine;
using System.Collections;
using System;

public class JumpOver : MonoBehaviour 
{
	[SerializeField] private Transform[] bezierControlPoints;
	[SerializeField] private float duration;

	private float startTime;

	private float dir;
	private bool isTransporting = false;

	private Transform legionTransform;

	private void Update()
	{
		if( isTransporting )
		{
			float timeAt = Mathf.Clamp01(Time.time - startTime);

			if(dir == 1)
				legionTransform.position = GetBezierPoint( bezierControlPoints[0].position, bezierControlPoints[3].position, bezierControlPoints[1].position, bezierControlPoints[2].position, timeAt );
			else if( dir == -1)
				legionTransform.position = GetBezierPoint( bezierControlPoints[3].position, bezierControlPoints[0].position, bezierControlPoints[2].position, bezierControlPoints[1].position, timeAt );

			if( timeAt == 1 )
			{
				isTransporting = false;
				legionTransform.GetComponent<Character>().enabled = true;
			}
		}
	}

	private Vector3 GetBezierPoint( Vector3 start, Vector3 end, Vector3 startTangent, Vector3 endTangent, float time )
	{
		return ( ( Mathf.Pow((1 - time), 3) * start) + ( 3 * Mathf.Pow((1 - time),2) * time * startTangent ) + ( 3 * ( 1 - time ) * Mathf.Pow(time, 2) * endTangent ) + ( Mathf.Pow(time, 3) * end ) );
	}

	public void TriggerBezierTransition(Transform inLegionTransform, int inDir)
	{
		if(!isTransporting)
		{			
			legionTransform = inLegionTransform;
			legionTransform.GetComponent<Character>().enabled = false;
			
			isTransporting = true;
			startTime = Time.time;
			
			dir = inDir;
		}
	}
}
