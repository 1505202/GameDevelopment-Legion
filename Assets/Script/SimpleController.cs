using UnityEngine;
using System.Collections;
/// <summary>
/// Simple Character Controller
/// This system works with the principle of layer masks
/// Disables layers and activates layers to control blocks, turning them on and off accordingly
/// Note: If a block already exists in a invisible state, when reshot it reactivates.
/// </summary>
public class SimpleController : MonoBehaviour 
{
	public bool enableDebugControls;

	public float speed;

	private float axisX;
	private float axisY;

	private Rigidbody myRigidbody;

	private float aimAxisX;
	private float aimAxisY;

	private void Start () 
	{
		myRigidbody = GetComponent<Rigidbody>();
	}
	private void Update () 
	{
		// Validates Input [Debug = KMInput (Keyboard Mouse Input), or JSInput (Joystick Input)]
		if(enableDebugControls)
		{
			GetKMInput();
		}
		else
		{
			GetJSInput();
		}

		// Sets Player Velocity
		myRigidbody.velocity = new Vector3(axisX * speed, myRigidbody.velocity.y, axisY * speed);

/* NOTE: DISABLED UNTIL FURTHER NOTICE DO NOT DELETE
		// "Destroy" Object
		if(Input.GetButtonDown("KMDestroy") || Input.GetButtonDown("JDestroy"))
		{
			Vector3 dir = new Vector3(aimAxisX, 0, aimAxisY).normalized;

			RaycastHit hit;
			LayerMask layerMask = (1 << LayerMask.NameToLayer("MapGrid"));

			Debug.DrawRay(transform.position, dir * 100, Color.black);

			if(Physics.Raycast(transform.position, dir, out hit, 100, layerMask))
			{
				GameObject obj = hit.collider.gameObject;
				obj.GetComponent<MeshRenderer>().enabled = false;
				obj.layer = LayerMask.NameToLayer("IgnorePlayerCollision");
			}
		}

		// Create Object (Also Activates First Active One)
		if(Input.GetButtonDown("KMCreate") || Input.GetButtonDown("JCreate"))
		{
			Vector3 dir = new Vector3(aimAxisX, 0, aimAxisY).normalized;
			
			RaycastHit hit;
			LayerMask layerMask = (1 << LayerMask.NameToLayer("IgnorePlayerCollision")) | (1 << LayerMask.NameToLayer("MapGrid")) | (1 << LayerMask.NameToLayer("Walls"));
			
			Debug.DrawRay(transform.position, dir * 100, Color.black);
			
			if(Physics.Raycast(transform.position, dir, out hit, 100, layerMask))
			{
				// Make Sure Its In Range (Stops Player being Stuck In A Cube)
				if( Vector3.Distance(transform.position, hit.point) > 1 )
				{
					GameObject obj = hit.collider.gameObject;

					// Checks if hit an active or inactive block, if its inactive then it activates it, otherwise it adds a new one and sets
					// the MapManager to isDirty = true;
					if(obj.layer == LayerMask.NameToLayer("IgnorePlayerCollision"))
					{
						obj.GetComponent<MeshRenderer>().enabled = true;
						obj.layer = LayerMask.NameToLayer("MapGrid");
					}
					else if(obj.layer == LayerMask.NameToLayer("MapGrid") || obj.layer == LayerMask.NameToLayer("Walls")) 
					{
						Transform cube = GameObject.CreatePrimitive(PrimitiveType.Cube).GetComponent<Transform>();

						Transform objT = obj.GetComponent<Transform>();

						float dot = Vector3.Dot( Vector3.forward, (hit.point - objT.position).normalized );


						// On The Sides
						if( dot > 0.5f || dot < -0.5f )
						{
							if(dot > 0)
								cube.position = objT.position + new Vector3( 0, 0,  1);
							else 
								cube.position = objT.position + new Vector3( 0, 0, -1);
						}
						else // Fornt Or Back 
						{
							dot = Vector3.Dot( Vector3.right, (hit.point - objT.position).normalized ); // maybe could do without this one?

							if(dot > 0)
								cube.position = objT.position + new Vector3( 1, 0, 0);
							else 
								cube.position = objT.position + new Vector3(-1, 0, 0);
						}
						cube.gameObject.layer = LayerMask.NameToLayer("MapGrid");
					}
				}

				MapManager.instance.isDirty = true;
			}
		}
*/
	}

	private void GetKMInput()
	{
		axisX = Input.GetAxisRaw("KMMoveHorizontal");
		axisY = Input.GetAxisRaw("KMMoveVertical");

		aimAxisX = Input.mousePosition.x - (Screen.width / 2);
		aimAxisY = Input.mousePosition.y - (Screen.height / 2);
	}

	private void GetJSInput()
	{
		axisX = Input.GetAxisRaw("JMoveHorizontal");
		axisY = Input.GetAxisRaw("JMoveVertical");

		aimAxisX = Input.GetAxisRaw("JAimHorizontal");
		aimAxisY = Input.GetAxisRaw("JAimVertical");
	}
}
