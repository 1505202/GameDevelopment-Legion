using UnityEngine;

using System.Collections.Generic;
/// <summary>
/// 
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] private float maxTime;

    [SerializeField] private GameObject legion;
    [SerializeField] private List<GameObject> rogueElements = new List<GameObject>(4); 

    private float currentTime;
	private List<Rogue> rogues = new List<Rogue>();



	private int assimilatedRogueCount = 0;
	public int AssimilatedRogueCount(Rogue rogue)
	{
		RemoveRogueElement(rogue);
		for(int i = 0; i < rogues.Count; i++)
		{
			rogues[i].RpcUpdateRogueSkillCount();
		}
		return ++assimilatedRogueCount;
	}

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
