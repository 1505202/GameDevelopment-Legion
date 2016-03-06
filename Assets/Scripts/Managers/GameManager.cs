using UnityEngine;

using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	[SerializeField] private float maxSeconds = 180;
    [SerializeField] private GameObject legion;
    [SerializeField] private List<GameObject> legionElements = new List<GameObject>(4);
    [SerializeField] private List<GameObject> rogueElements = new List<GameObject>(4);
    [SerializeField] private Text gameOverText;
    [SerializeField] private Text timerText;
	[SerializeField] private int timeRemainingWarningThreshold = 10;
	private bool isGameOver;

	private bool isEndGameWarningPlaying;
	private int assimilatedRogueCount = 0;
    private static GameManager instance;

    public static GameManager Instance { get { return instance; } }
    public float SecondsRemaining { get; private set; }
    public bool IsGameOver { get { return isGameOver; } set { isGameOver = value; } }

    private void Start()
    {
        IsGameOver = false;
		isEndGameWarningPlaying = false;

        if (instance == null)
        {
            instance = this;
            GetComponent<Transform>().parent = GameObject.FindGameObjectWithTag("ManagerHolder").GetComponent<Transform>();
            SecondsRemaining = maxSeconds;
        }
        else
        {
            Destroy(this);
        }
    }
    private void Update()
    {
        ClockTick();    
        gameOverText.enabled = IsGameOver;

		if (IsGameOver)
		{
			return;
		}

		timerText.text = SecondsRemaining.ToString("F1"); // display one decimal place

		CheckForVictory ();
    }

	public void CheckForVictory()
	{
		if (rogueElements.Count <= 0) 
		{
			LegionVictory ();
			IsGameOver = true;
		} 
		else if (SecondsRemaining <= 0) 
		{
			RogueVictory ();
			IsGameOver = true;
		}

		if (IsGameOver) 
		{
			StopEndGameWarning();
			AudioManager.PlayGameOverSound();
			DisablePhysics ();
		}
	}
	
	public void Assimilate(Rogue rogue)
	{
		rogueElements.Remove(rogue.gameObject);
		legionElements.Add(rogue.gameObject);
		
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
        SecondsRemaining -= Time.deltaTime;

		if (SecondsRemaining <= 10 && !isEndGameWarningPlaying && !IsGameOver) 
		{
			PlayEndGameWarning();
		}
    }

    private void RogueVictory()
    {
        Debug.Log("Rogues Win!!!");
        gameOverText.text = "Rogue Victory!";
    }
    private void LegionVictory()
    {
        Debug.Log("Legion Wins!!!");
        gameOverText.text = "Legion Victory!";
    }

    private void DisablePhysics()
    {
        legion.GetComponent<Rigidbody>().velocity = Vector3.zero;
        for (int i = 0; i < rogueElements.Count; i++ )
        {
            rogueElements[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        for (int i = 0; i < legionElements.Count; i++)
        {
            legionElements[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

	private void PlayEndGameWarning()
	{
		AudioManager.PlayGameAlmostOverSound();
		isEndGameWarningPlaying = true;
	}

	private void StopEndGameWarning()
	{
		AudioManager.StopGameAlmostOverSound();
		isEndGameWarningPlaying = false;
	}
}
