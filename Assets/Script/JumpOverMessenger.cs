﻿using UnityEngine;
using System.Collections;

public class JumpOverMessenger : MonoBehaviour 
{
	[SerializeField] private JumpOver target;
	[SerializeField] [Range(-1,1)]private int dir;

	private void OnTriggerEnter(Collider collidedObject)
	{
		if( collidedObject.CompareTag("Legion") )
		{
			target.TriggerBezierTransition( collidedObject.GetComponent<Transform>(), dir );
		}
	}
}
