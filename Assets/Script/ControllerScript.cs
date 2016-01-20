using UnityEngine;
using System.Collections;

/*
Used to specify behaviour for player controlled characters like Legion and RoguePlayer
 */
public class ControllerScript : Character {
//
//	[SerializeField] private KeyCode up;
//	[SerializeField] private KeyCode down;
//	[SerializeField] private KeyCode left;
//	[SerializeField] private KeyCode right;
//
//	public ArrayList cameras = new ArrayList ();
//
//	private Rigidbody2D body;
//
//	// Use this for initialization
//	void Start () {
//		cameras.Add (myCamera);
//		if (gameObject.tag == "Legion")
//			speed = 8f;
//		else if (gameObject.tag == "RoguePlayer" || gameObject.tag == "RogueAI")
//			speed = 7f;
//		assimilated = gameObject.tag == "Legion";
//	}
//	
//	// Update is called once per frame
//	void Update () {
//
//		//Process input
//		Vector2 v = new Vector2 (0, 0);
//		if (Input.GetKey (right)) {
//			v = new Vector2 (v.x + speed, v.y);
//		}
//		if (Input.GetKey (up)) {
//			v = new Vector2 (v.x, v.y + speed);
//		}
//		if (Input.GetKey (left)) {
//			v = new Vector2 (v.x - speed, v.y);
//		}
//		if (Input.GetKey (down)) {
//			v = new Vector2 (v.x, v.y - speed);
//		}
//
//		GetComponent<Rigidbody2D> ().velocity = v;
//
//		if ((gameObject.tag == "RoguePlayer") && !(v.x == 0 && v.y == 0))
//			GameObject.Find ("GameController").GetComponent<SpawnController> ().start = true;
//
//		//if (myCamera != null) {
//		//	if(!assimilated || gameObject.tag == "Legion") myCamera.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, myCamera.transform.position.z);
////			else {
////				myCamera.transform.position = GameObject.Find ("LegionCamera").GetComponent<Camera>().transform.position;
////			}
//
//		/*foreach(Camera c in cameras){
//				if(c != null && gameObject.tag == "Legion")
//					c.transform.position = GameObject.Find ("LegionCamera").transform.position;
//		}
//		}*/
//	}
//
//	void OnCollisionEnter2D(Collision2D coll){
//		//Set color of rogue to legion
//		if ((coll.gameObject.tag == "RoguePlayer" || coll.gameObject.tag == "RogueAI") && gameObject.tag == "Legion") {
//			//Increase size if this is legion
//			gameObject.transform.localScale = new Vector3 (gameObject.transform.localScale.x+0.11f, 1.0f, gameObject.transform.localScale.z+0.11f);
//			//Add camera to list of cameras following this character
//			Camera collCamera = coll.gameObject.GetComponent<Character>().getCamera ();
//			//If collision character has a camera, add it as a child of this object
//			if(collCamera != null) {
//				collCamera.transform.parent = getCamera ().transform;
//				cameras.Add (collCamera);
//			}
//				
//
//			assimilated = true;
//
//			Destroy (coll.gameObject);
//
//		}
//
//		Debug.Log ("[" + gameObject.tag + "]" + "Collision with: " + coll.gameObject.tag);
//	}
	
}
