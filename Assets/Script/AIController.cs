using UnityEngine;
using System.Collections;

public class AIController : Character {

	[SerializeField] private float moveTimer = 0f;
	[SerializeField] private float nextMoveTime = 0f;
	[SerializeField] private float maxMoveTime = 10f;
	[SerializeField] private float minMoveTime = 5f;

	public float wallRayRange = 1f;

	// Use this for initialization
	void Start () {

		if (gameObject.tag == "Legion")
			speed = 8f;
		else if (gameObject.tag == "RoguePlayer" || gameObject.tag == "RogueAI")
			speed = 7f;
		assimilated = gameObject.tag == "Legion";
	}
	
	// Update is called once per frame
	void Update () {

		if (!GameObject.Find ("GameController").GetComponent<SpawnController>().start)
			return;

		moveTimer += Time.deltaTime;

		if (moveTimer >= nextMoveTime || checkWallCollision()) {
			changeMovement();
			moveTimer = 0f;
			nextMoveTime = Random.Range (minMoveTime, maxMoveTime);
		}

		/*if (myCamera != null) {
			myCamera.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, myCamera.transform.position.z);
		}*/

		Debug.DrawRay (transform.position, Vector3.up * wallRayRange, Color.green);
		Debug.DrawRay (transform.position, Vector3.down * wallRayRange, Color.green);
		Debug.DrawRay (transform.position, Vector3.left * wallRayRange, Color.green);
		Debug.DrawRay (transform.position, Vector3.right * wallRayRange, Color.green);

	}

	private void changeMovement(){
		Vector2 direction = getRandomValidMovement ();

		GetComponent<Rigidbody2D> ().velocity = direction * speed;
	}

	private Vector2 getRandomValidMovement(){
		int layerMask = 1 << LayerMask.NameToLayer ("Walls");
		ArrayList directions = new ArrayList ();

		RaycastHit2D upHit = Physics2D.Raycast (transform.position, Vector2.up, wallRayRange, layerMask);
		RaycastHit2D leftHit = Physics2D.Raycast (transform.position, Vector2.left, wallRayRange, layerMask);
		RaycastHit2D rightHit = Physics2D.Raycast (transform.position, Vector2.right, wallRayRange, layerMask);
		RaycastHit2D downHit = Physics2D.Raycast (transform.position, Vector2.down, wallRayRange, layerMask);
	
		if (upHit.collider == null)
			directions.Add (Vector2.up);
		if (leftHit.collider == null)
			directions.Add (Vector2.left);
		if (rightHit.collider == null)
			directions.Add (Vector2.right);
		if (downHit.collider == null)
			directions.Add (Vector2.down);

		//Pick random element from valid directions
		int i = Random.Range (0, directions.Count);
		return (Vector2) directions[i];
	}

	private bool checkWallCollision(){
		int layerMask = 1 << LayerMask.NameToLayer ("Walls");
		Vector2 v = GetComponent<Rigidbody2D> ().velocity;

		RaycastHit2D upHit = Physics2D.Raycast (transform.position, Vector2.up, wallRayRange, layerMask);
		RaycastHit2D leftHit = Physics2D.Raycast (transform.position, Vector2.left, wallRayRange, layerMask);
		RaycastHit2D rightHit = Physics2D.Raycast (transform.position, Vector2.right, wallRayRange, layerMask);
		RaycastHit2D downHit = Physics2D.Raycast (transform.position, Vector2.down, wallRayRange, layerMask);
		
		if (upHit.collider != null && v.y > 0.1f)
			return true;
		else if (leftHit.collider != null && v.x < -0.1f)
			return true;
		else if (rightHit.collider != null && v.x > 0.1f)
			return true;
		else if (downHit.collider != null && v.y < -0.1f)
			return true;

		return false;
	}
}
