using UnityEngine;

using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float maxSeconds = 180;
    [SerializeField] private GameObject legion;
    [SerializeField] private List<GameObject> rogueElements = new List<GameObject>(4);
    [SerializeField] private Text gameOverText;
    [SerializeField] private Text timerText;

	private int assimilatedRogueCount = 0;
    private static GameManager instance;


    public static GameManager Instance
    {
        get { return instance; }
    }

    public float SecondsRemaining { get; private set; }

    public bool IsGameOver { get; set; }

    private void Start()
    {
        IsGameOver = false;
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
        if (IsGameOver)
            return;

        ClockTick();
        gameOverText.enabled = IsGameOver;
        timerText.text = SecondsRemaining.ToString("F1"); // display one decimal place


        if (IsGameOver)
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
        SecondsRemaining -= Time.deltaTime;
        if (SecondsRemaining <= 0)
        {
            IsGameOver = true;
        }
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
