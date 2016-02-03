using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// 
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] private float maxTime;
    private float currentTime;
	private bool hasGameStarted = false; // TODO: Network Manager Will turn this to true when game is ready to Start
	private List<Rogue> rogues = new List<Rogue>();

	private static GameManager instance;
	public static GameManager Instance
	{
		get { return instance; }
	}

    private void Start()
    {
		if(instance == null)
		{
			instance = this;
			GetComponent<Transform>().parent = GameObject.FindGameObjectWithTag("ManagerHolder").GetComponent<Transform>();
			currentTime = maxTime;
		}
		else
		{
			Destroy (this);
		}
    }

    private void Update()
    {
		if(hasGameStarted)
		{
			ClockTick();
			if( IsGameOver() == 1 ) // Legion Won
			{
				Debug.Log("Legion Won");
			}
			else if( IsGameOver() == 2 ) // Rogues Won
			{
				Debug.Log("Rogues Win");
			}
		}
    }

	public void StartGame()
	{
		hasGameStarted = true;
	}

    /// <summary>
    /// Tests For A Game Over Everyframe
    /// </summary>
    private int IsGameOver()
    {
		if( rogues.Count == 0 )
		{
			return 1;
		}

		if( currentTime <= 0 )
		{
			return 2;
		}

		return 0;
    }

    /// <summary>
    /// Processes The Countdown Tick
    /// </summary>
    private void ClockTick()
    {
		currentTime -= Time.deltaTime;
    }

    /// <summary>
    /// Registers a Rogue Element To The List
    /// </summary>
    /// <param name="rogue"></param>
    public void RegisterRogueElement(Rogue rogue)
    {
        rogues.Add(rogue);
    }

    /// <summary>
    /// Removes A Rogue Element From The List
    /// </summary>
    /// <param name="rogue"> Rogue Element To Remove </param>
    public void RemoveRogueElement(Rogue rogue)
    {
        rogues.Remove(rogue);
    }

    /// <summary>
    /// Finds The Rogue Element In The List
    /// </summary>
    /// <param name="rogue"> Rogue To Find In The List </param>
    public  void FindRogueElement(Rogue rogue)
    {
        
    }

    /// <summary>
    /// Gets A Specific Rogue Element
    /// </summary>
    /// <param name="index"> Get Rogue Element At Index </param>
    /// <returns> A Specific Rogue Element </returns>
    public Rogue GetRogueElement(int index)
    {
        return rogues[index];
    }

    /// <summary>
    /// Removes A Rogue Element At An Index
    /// </summary>
    /// <param name="index"> Index Where To Remove </param>
    /// <returns> Rogue That Was Removed </returns>
    public Rogue RemoveRogueElementAt(int index)
    {
        Rogue rogue = rogues[index];
        rogues.Remove(rogue);
        return rogue;
    }
}
