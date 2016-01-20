using UnityEngine;
using System.Collections;

public abstract class Character : MonoBehaviour, ICamera {

	[SerializeField] protected Camera myCamera;
	protected float speed;
	protected bool assimilated;

	public Camera getCamera(){
		return myCamera;
	}

	public void setCamera(Camera c){
		myCamera = c;
	}
}