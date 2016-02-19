using UnityEngine;

using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int maxSeconds = 180;
    [SerializeField] private GameObject legion;
    [SerializeField] private List<GameObject> rogueElements = new List<GameObject>(4); 

    private float secondsRemaining;
	private int assimilatedRogueCount = 0;
    private static GameManager instance;

    public static GameManager Instance
    {
        get { return instance; }
    }

    public int SecondsRemaining
    {
        get { return (int)secondsRemaining; }
    }

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            GetComponent<Transform>().parent = GameObject.FindGameObjectWithTag("ManagerHolder").GetComponent<Transform>();
            secondsRemaining = maxSeconds;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        ClockTick();

        if (secondsRemaining < 0)
        {
            if (assimilatedRogueCount >= 4)
            {
                LegionVictory();
            }
            else
            {
                RogueVictory();
            }
        }
    }


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

    /// <summary>
    /// Processes The Countdown Tick
    /// </summary>
    private void ClockTick()
    {
        secondsRemaining -= Time.deltaTime;
    }

    /// <summary>
    /// Removes A Rogue Element From The List
    /// </summary>
    /// <param name="rogue"> Rogue Element To Remove </param>
    public void RemoveRogueElement(Rogue rogue)
    {
        rogueElements.Remove(rogue.gameObject);
    }

    private void RogueVictory()  { Debug.Log("Rogues Win!!!");}
    private void LegionVictory() { Debug.Log("Legion Wins!!!"); }
}
