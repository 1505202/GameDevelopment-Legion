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
                Debug.LogError("Please Place A ControllerManager Into The Scene.");
			}

			return instance;
		}
	}
	private AController[] controllers = new AController[5];

    private int playerCount = 0;

	private void Start()
	{
        if (instance == null)
        {
            instance = this;
        }
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
