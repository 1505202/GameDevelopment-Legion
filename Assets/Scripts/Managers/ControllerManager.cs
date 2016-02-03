using UnityEngine;

public class ControllerManager : MonoBehaviour 
{
	private static ControllerManager instance;
	public static ControllerManager Instance
	{
		get
		{
			if(instance == null)
			{
				instance = new GameObject("ControllerManager", typeof(ControllerManager)).GetComponent<ControllerManager>();
			}

			return instance;
		}
	}
	private AController[] controllers = new AController[5];

	private void Start()
	{
		GetComponent<Transform>().parent = GameObject.FindGameObjectWithTag("ManagerHolder").GetComponent<Transform>();
	}

	private void Update () 
	{
		for(int i  = 0; i < controllers.Length; i++)
		{
			if(controllers[i] != null)
			{
				controllers[i].UpdateController();
			}
		}
	}

	public AController NewController( AController controllerInput )
	{
		for(int i = 0; i < controllers.Length; i++)
		{
			if( controllers[i] == null )
			{
				controllers[i] = controllerInput;
				return controllers[i];
			}
		}
		return null;
	}
	public AController ReplaceController( AController currentController, AController newController )
	{
		for(int i = 0; i < controllers.Length; i++)
		{
			if( controllers[i] == currentController )
			{
				controllers[i] = newController;
				return controllers[i];
			}
		}
		return null;
	}
}
