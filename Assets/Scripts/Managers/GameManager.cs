using UnityEngine;

using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float maxTime;

    [SerializeField] private GameObject legion;
    [SerializeField] private List<GameObject> rogueElements = new List<GameObject>(4); 

    private float currentTime;

	private int assimilatedRogueCount = 0;
	public void Assimilate(Rogue rogue)
	{
		RemoveRogueElement(rogue);
        for (int i = 0; i < rogueElements.Count; i++)
		{
            rogueElements[i].GetComponent<Rogue>().UpdateRogueSkillCount();
		}
	}
    public int GetBehaviourIndex()
    {
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
    /// Removes A Rogue Element From The List
    /// </summary>
    /// <param name="rogue"> Rogue Element To Remove </param>
    public void RemoveRogueElement(Rogue rogue)
    {
        rogueElements.Remove(rogue.gameObject);
    }
}
