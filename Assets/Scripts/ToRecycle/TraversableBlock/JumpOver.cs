using UnityEngine;
using System.Collections.Generic;
using System;

public class JumpOver : MonoBehaviour 
{
	[SerializeField] private Transform[] bezierControlPoints = null;

    List<TransformingPlayer> transforms = new List<TransformingPlayer>();

    class TransformingPlayer
    {
        public AActor controller;
        public Transform playerTransform = null;
        public float startTime = 0;
        public int dir = 0;

        public TransformingPlayer(AActor controller, Transform playerTransform, float startTime, int dir)
        {
            this.controller = controller;
            this.playerTransform = playerTransform;
            this.startTime = startTime;
            this.dir = dir;
        }
    }

	private void Update()
    {
        int i = 0;
        while (i < transforms.Count) 
        {
            TransformingPlayer target = transforms[i];

            float timeAt = Mathf.Clamp01(Time.time - target.startTime);

            if (target.dir == 1)
                target.playerTransform.position = GetBezierPoint(bezierControlPoints[0].position, bezierControlPoints[3].position, bezierControlPoints[1].position, bezierControlPoints[2].position, timeAt);
            else if (target.dir == -1)
                target.playerTransform.position = GetBezierPoint(bezierControlPoints[3].position, bezierControlPoints[0].position, bezierControlPoints[2].position, bezierControlPoints[1].position, timeAt);

            if (timeAt == 1)
            {
                if (target.controller != null) // Clone Has No Actor
                    target.controller.enabled = true;
                transforms.RemoveAt(i);
            }
            else i++;
        }
    }

	private Vector3 GetBezierPoint( Vector3 start, Vector3 end, Vector3 startTangent, Vector3 endTangent, float time )
	{
		return ( ( Mathf.Pow((1 - time), 3) * start) + ( 3 * Mathf.Pow((1 - time),2) * time * startTangent ) + ( 3 * ( 1 - time ) * Mathf.Pow(time, 2) * endTangent ) + ( Mathf.Pow(time, 3) * end ) );
	}

	public void TriggerBezierTransition(Transform inLegionTransform, int inDir)
	{
        for (int i = 0; i < transforms.Count; i++)
        {
            if(transforms[i].playerTransform == inLegionTransform)
                return;
        }

        TransformingPlayer player = new TransformingPlayer(inLegionTransform.GetComponent<AActor>(), inLegionTransform, Time.time, inDir);

        transforms.Add(player);

        if(player.controller != null)
            player.controller.enabled = false;
    }
}
