using UnityEngine;
using System.Collections;

public class AIController// : Character 
{
//
//	[SerializeField] private float moveTimer = 0f;
//	[SerializeField] private float nextMoveTime = 0f;
//	[SerializeField] private float maxMoveTime = 5f;
//	[SerializeField] private float minMoveTime = 2.5f;
//
//	public float wallRayRange = 1f;
//
//	private float moveVariance = 0.2f;
//
//	private float legionDangerRange = 5f;
//	private bool danger = false;
//
//	// Use this for initialization
//	void Start () {
//
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
//		if (!GameObject.Find ("GameController").GetComponent<SpawnController>().start)
//			return;
//
//		moveTimer += Time.deltaTime;
//
//		if (moveTimer >= nextMoveTime || checkWallCollision()) {
//			changeMovement();
//			moveTimer = 0f;
//			nextMoveTime = Random.Range (minMoveTime, maxMoveTime);
//		}
//
//		//Change rotation according to velocity
//		Vector3 direction = GetComponent<Rigidbody> ().velocity.normalized;
//
//		Vector3 camOriginPos = myCamera.transform.position;
//		Quaternion camOriginRot = myCamera.transform.rotation;
//		
//		if(!(direction.x==0 && direction.z==0)){
//			Quaternion lookRotation = Quaternion.LookRotation (direction.normalized);
//			transform.rotation = Quaternion.Slerp (transform.rotation, lookRotation, 0.5f);
//			myCamera.transform.position = camOriginPos;
//			myCamera.transform.rotation = camOriginRot;
//		}
//
//		/*if (myCamera != null) {
//			myCamera.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, myCamera.transform.position.z);
//		}*/
//
//		Debug.DrawRay (transform.position, Vector3.forward * wallRayRange, Color.red);
//		Debug.DrawRay (transform.position, Vector3.back * wallRayRange, Color.green);
//		Debug.DrawRay (transform.position, Vector3.left * wallRayRange, Color.green);
//		Debug.DrawRay (transform.position, Vector3.right * wallRayRange, Color.green);
//		Debug.Log ("updateai");
//
//	}
//
//	private void changeMovement(){
//		Vector3 direction = getRandomValidMovement ();
//
//		GetComponent<Rigidbody> ().velocity = direction * speed;
//	}
//
//	private Vector3 getRandomValidMovement(){
//		int layerMask = 1 << LayerMask.NameToLayer ("Walls");
//		ArrayList directions = new ArrayList ();
//
//		bool forwardHit = Physics.Raycast (transform.position, Vector3.forward, wallRayRange*3, layerMask);
//		bool leftHit = Physics.Raycast (transform.position, Vector3.left, wallRayRange*3, layerMask);
//		bool rightHit = Physics.Raycast (transform.position, Vector3.right, wallRayRange*3, layerMask);
//		bool backHit = Physics.Raycast (transform.position, Vector3.back, wallRayRange*3, layerMask);
//
//		//Check for position of legion
//		GameObject legion = GameObject.Find ("Legion");
//		float legionDistance = Vector3.Distance (this.transform.position, legion.transform.position);
//		danger = legionDistance <= legionDangerRange;
//		Vector3 legionDirection = legion.transform.position - this.transform.position;
//		legionDirection.Normalize ();
//	
//		if (!forwardHit && (legionDirection.z <= 0 || (Mathf.Pow(legionDirection.z,2) <= Mathf.Pow (legionDirection.x,2)) && !danger))
//			directions.Add (Vector3.forward + (Vector3.right * Random.Range (-moveVariance, moveVariance)));
//		if (!leftHit && (legionDirection.x >= 0 || (Mathf.Pow(legionDirection.x,2) <= Mathf.Pow (legionDirection.z,2)) && !danger))
//			directions.Add (Vector3.left + (Vector3.forward * Random.Range (-moveVariance, moveVariance)));
//		if (!rightHit && (legionDirection.x <= 0 || (Mathf.Pow(legionDirection.x,2) <= Mathf.Pow (legionDirection.z,2)) && !danger))
//			directions.Add (Vector3.right + (Vector3.forward * Random.Range (-moveVariance, moveVariance)));
//		if (!backHit && (legionDirection.z >= 0 || (Mathf.Pow(legionDirection.z,2) <= Mathf.Pow (legionDirection.x,2)) && !danger))
//			directions.Add (Vector3.back + (Vector3.right * Random.Range (-moveVariance, moveVariance)));
//
//		//Pick random element from valid directions
//		int i = Random.Range (0, directions.Count);
//		if (directions.Count == 0)	//If there are no possible safe directions to move
//			return new Vector3 (0, 0, 0);
//		else
//			return (Vector3) directions[i];
//	}
//
//	private bool checkWallCollision(){
//		int layerMask = 1 << LayerMask.NameToLayer ("Walls");
//		Vector3 v = GetComponent<Rigidbody> ().velocity;
//
//		bool forwardHit = Physics.Raycast (transform.position, Vector3.forward, wallRayRange*1.1f, layerMask);
//		bool leftHit = Physics.Raycast (transform.position, Vector3.left, wallRayRange, layerMask);
//		bool rightHit = Physics.Raycast (transform.position, Vector3.right, wallRayRange, layerMask);
//		bool backHit = Physics.Raycast (transform.position, Vector3.back, wallRayRange, layerMask);
//		
//		if (forwardHit && v.z > 0.1f)
//			return true;
//		else if (leftHit && v.x < -0.1f)
//			return true;
//		else if (rightHit && v.x > 0.1f)
//			return true;
//		else if (backHit && v.z < -0.1f)
//			return true;
//
//		return false;
//	}
}
