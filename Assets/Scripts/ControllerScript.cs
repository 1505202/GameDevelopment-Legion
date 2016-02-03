using UnityEngine;
using System.Collections;

/*
Used to specify behaviour for player controlled characters like Legion and RoguePlayer
 */
public class ControllerScript : Character {

	[SerializeField] private KeyCode up;
	[SerializeField] private KeyCode down;
	[SerializeField] private KeyCode left;
	[SerializeField] private KeyCode right;

	public ArrayList cameras = new ArrayList ();

	private Rigidbody body;

	// Use this for initialization
	void Start () {
		cameras.Add (myCamera);
		if (gameObject.tag == "Legion")
			speed = 8f;
		else if (gameObject.tag == "RoguePlayer" || gameObject.tag == "RogueAI")
			speed = 7f;
		assimilated = gameObject.tag == "Legion";
	}
	
	// Update is called once per frame
	void Update () {

		//Process input
		Vector3 v = new Vector3 (0, 0, 0);
		if (Input.GetKey (right)) {
			v = new Vector3 (v.x + speed, v.y, v.z);
		}
		if (Input.GetKey (up)) {
			v = new Vector3 (v.x, v.y, v.z + speed);
		}
		if (Input.GetKey (left)) {
			v = new Vector3 (v.x - speed, v.y, v.z);
		}
		if (Input.GetKey (down)) {
			v = new Vector3 (v.x, v.y, v.z - speed);
		}

		Vector3 camOriginPos = myCamera.transform.position;
		Quaternion camOriginRot = myCamera.transform.rotation;

		if(!(v.x==0 && v.z==0)){
			Quaternion lookRotation = Quaternion.LookRotation (v.normalized);
			transform.rotation = Quaternion.Slerp (transform.rotation, lookRotation, 0.5f);
			myCamera.transform.position = camOriginPos;
			myCamera.transform.rotation = camOriginRot;
		}

		GetComponent<Rigidbody> ().velocity = v;

		if ((gameObject.tag == "RoguePlayer") && !(v.x == 0 && v.z == 0))
			GameObject.Find ("GameController").GetComponent<SpawnController> ().start = true;
		 
	}

	void OnCollisionEnter(Collision coll){
		Debug.Log ("Collision");
		//Set color of rogue to legion
		if ((coll.gameObject.tag == "RoguePlayer" || coll.gameObject.tag == "RogueAI") && gameObject.tag == "Legion") {
			Debug.Log ("Detection");
			//Increase size if this is legion
			gameObject.transform.localScale = new Vector3 (gameObject.transform.localScale.x+0.11f, 1.0f, gameObject.transform.localScale.z+0.11f);
			//Add camera to list of cameras following this character
			Camera collCamera = coll.gameObject.GetComponent<Character>().getCamera ();
			//If collision character has a camera, add it as a child of this object
			if(collCamera != null) {
				collCamera.transform.parent = getCamera ().transform;
				cameras.Add (collCamera);
			}
				

			assimilated = true;

			Destroy (coll.gameObject);

		}

		Debug.Log ("[" + gameObject.tag + "]" + "Collision with: " + coll.gameObject.tag);
	}
	
}
