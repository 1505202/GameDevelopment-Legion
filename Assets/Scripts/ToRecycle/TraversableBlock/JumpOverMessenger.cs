using UnityEngine;
using System.Collections;

public class JumpOverMessenger : MonoBehaviour 
{
	[SerializeField] private JumpOver target = null;
	[SerializeField] [Range(-1,1)]private int dir = 0;

	private void OnTriggerEnter(Collider collidedObject)
	{
        if( !collidedObject.CompareTag("Tether")  )
        {
			target.TriggerBezierTransition( collidedObject.GetComponent<Transform>(), dir );
        }
	}
}
