using UnityEngine;
using System.Collections;

public class Cover : MonoBehaviour 
{
	//private GameObject hiddenCover = null;
	//private GameObject shownCover = null;


	private void Start()
	{

		//hiddenCover = myTransform.GetChild(0).gameObject;
		//shownCover = myTransform.GetChild(1).gameObject;
	}

	private void OnTriggerEnter(Collider triggeredObject)
	{
		//triggeredObject.gameObject.GetComponent<Character>().getCamera(); // change layer

		Camera c = Camera.main;
		c.cullingMask = c.cullingMask & ~(1 << LayerMask.NameToLayer("Cover"));
	}

	private void OnTriggerExit(Collider triggeredObject)
	{
		//triggeredObject.gameObject.GetComponent<Character>().getCamera();
		Camera c = Camera.main;
		c.cullingMask = c.cullingMask | (1 << LayerMask.NameToLayer("Cover"));
	}
}
