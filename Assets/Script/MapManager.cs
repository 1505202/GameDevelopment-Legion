using UnityEngine;
using System.Collections;

/// <summary>
/// Generates a perimeter ad handles the gameobject matrix map of all the current grid and its size.
/// </summary>
public class MapManager : MonoBehaviour 
{
	public int xDimension = 0;
	public int zDimension = 0;

	public float offset;

	public LayerMask layerMask;

	public bool isDirty = false;

	public static MapManager instance;

	[SerializeField] private GameObject[,] map;

	private void Start () 
	{
		instance = this;

		GeneratePerimeterCubes();
		InitializeMap();

		string[] joysticksConnected = Input.GetJoystickNames();
	
		// PS4 Controller
		if(joysticksConnected[0] == "Wireless Controller")
		{

		}
	}

	private void Update()
	{
		if(isDirty)
		{
			RecalculateMap();
			isDirty = false;
		}
	}

	private void GeneratePerimeterCubes()
	{
		Transform perimeterParent = GetComponent<Transform>().GetChild(0);
		for(int z = 0; z < zDimension; z++)
		{
			if( z == 0 || z == zDimension - 1 )
			{
				for(int x = 0; x < xDimension; x++)
				{
					Transform cube = GameObject.CreatePrimitive(PrimitiveType.Cube).GetComponent<Transform>();
					cube.position = new Vector3( x + offset, 0, z + offset );
					cube.parent = perimeterParent;

					cube.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.black);

					cube.gameObject.layer = LayerMask.NameToLayer("Walls");
				}
			}
			else
			{
				Transform cube = GameObject.CreatePrimitive(PrimitiveType.Cube).GetComponent<Transform>();
				cube.position = new Vector3( 0 + offset, 0, z + offset );
				cube.parent = perimeterParent;
				cube.gameObject.layer = LayerMask.NameToLayer("Walls");

				cube.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.black);



				cube = GameObject.CreatePrimitive(PrimitiveType.Cube).GetComponent<Transform>();
				cube.position = new Vector3( xDimension - 1 + offset, 0, z + offset );
				cube.parent = perimeterParent;
				cube.gameObject.layer = LayerMask.NameToLayer("Walls");

				cube.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.black);

			}
		}
	}
	private void InitializeMap()
	{
		map = new GameObject[xDimension, zDimension];

		for(int z = 0; z < zDimension; z++)
		{
			for(int x = 0; x < xDimension; x++)
			{
				RaycastHit hit;

				if(Physics.Raycast(new Vector3(x + offset, 10, z + offset), Vector3.down, out hit, Mathf.Infinity, layerMask))
				{
					map[x,z] = hit.collider.gameObject;
					hit.collider.gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);

				}
				else
				{
					map[x,z] = null;
				}
			}
		}

	}
	private void RecalculateMap()
	{
		for(int z = 0; z < zDimension; z++)
		{
			for(int x = 0; x < xDimension; x++)
			{
				RaycastHit hit;
				
				if(Physics.Raycast(new Vector3(x + offset, 10, z + offset), Vector3.down, out hit, Mathf.Infinity, layerMask))
				{
					map[x,z] = hit.collider.gameObject;
					hit.collider.gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
				}
				else
				{
					map[x,z] = null;
				}
			}
		}
	}
}
