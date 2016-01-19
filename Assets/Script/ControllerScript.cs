using UnityEngine;
using System.Collections;

public class ControllerScript : MonoBehaviour, CameraInterface {

	[SerializeField] private float speed = 1;
	[SerializeField] private KeyCode up;
	[SerializeField] private KeyCode down;
	[SerializeField] private KeyCode left;
	[SerializeField] private KeyCode right;

	public ArrayList cameras = new ArrayList ();
	public Camera startCamera;
	bool assimilated = false;

	private Rigidbody2D body;

	// Use this for initialization
	void Start () {
		cameras.Add (startCamera);
	}
	
	// Update is called once per frame
	void Update () {

		//Process input
		Vector2 v = new Vector2 (0, 0);
		if (Input.GetKey (right)) {
			v = new Vector2 (v.x + speed, v.y);
		}
		if (Input.GetKey (up)) {
			v = new Vector2 (v.x, v.y + speed);
		}
		if (Input.GetKey (left)) {
			v = new Vector2 (v.x - speed, v.y);
		}
		if (Input.GetKey (down)) {
			v = new Vector2 (v.x, v.y - speed);
		}

		GetComponent<Rigidbody2D> ().velocity = v;

		if ((gameObject.tag == "RoguePlayer") && !(v.x == 0 && v.y == 0))
			GameObject.Find ("GameScript").GetComponent<SpawnController> ().start = true;

		if (startCamera != null) {
			if(!assimilated || gameObject.tag == "Legion") startCamera.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, startCamera.transform.position.z);
//			else {
//				startCamera.transform.position = GameObject.Find ("LegionCamera").GetComponent<Camera>().transform.position;
//			}

		foreach(Camera c in cameras){
				if(c != null && gameObject.tag == "Legion")
					c.transform.position = GameObject.Find ("LegionCamera").transform.position;
		}
		}
	}

	void OnCollisionEnter2D(Collision2D coll){
		//Set color of rogue to legion
		if ((coll.gameObject.tag == "RoguePlayer" || coll.gameObject.tag == "RogueAI") && gameObject.tag == "Legion") {
			//coll.gameObject.GetComponent<Renderer> ().material = gameObject.GetComponent<Renderer> ().material;
			gameObject.transform.localScale = new Vector3 (gameObject.transform.localScale.x+0.11f, 1.0f, gameObject.transform.localScale.z+0.11f);
			if(coll.gameObject.tag == "RogueAI"){
				cameras.Add (coll.gameObject.GetComponent<AIController>().getCamera ());
			} else if(coll.gameObject.tag == "RoguePlayer"){
				cameras.Add (coll.gameObject.GetComponent<ControllerScript>().getCamera ());
			}
			assimilated = true;

			Destroy (coll.gameObject);

		}

		Debug.Log ("[" + gameObject.tag + "]" + "Collision with: " + coll.gameObject.tag);
	}

	public Camera getCamera(){
		return startCamera;
	}

}
