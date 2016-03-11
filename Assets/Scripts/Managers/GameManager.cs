using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	[SerializeField] private float maxSeconds = 180;
    [SerializeField] private GameObject legion = null;
    [SerializeField] private List<GameObject> legionElements = new List<GameObject>(4);
    [SerializeField] private List<GameObject> rogueElements = new List<GameObject>(4);
    [SerializeField] private Text gameOverText = null;
	[SerializeField] private Text timerText = null;
	[SerializeField] private int timeRemainingWarningThreshold = 10;
	[SerializeField] private GameObject pausePanel = null;
	[SerializeField] private GameObject LobbyPanel = null;

    private static GameManager instance;
	private static bool isResetting = false;
	private bool isGameOver;
	private bool isEndGameWarningPlaying;
	private int assimilatedRogueCount = 0;
	private bool isPaused = false;
	private bool isStarting = true;
	private bool[] readyPlayers = new bool[5];


    public static GameManager Instance { get { return instance; } }
    public float SecondsRemaining { get; private set; }
    public bool IsGameOver { get { return isGameOver; } set { isGameOver = value; } }

	enum States 
	{
		InLobby,
		InGame,
		PauseChanging,
		Paused,
		GameOver,
		ExitingApplication,
	};

    private void Start()
    {
        IsGameOver = false;
		isEndGameWarningPlaying = false;
		isPaused = false;
		pausePanel.SetActive (false);
		isStarting = true;
		
        if (instance == null)
        {
            instance = this;
            GetComponent<Transform>().parent = GameObject.FindGameObjectWithTag("ManagerHolder").GetComponent<Transform>();
            SecondsRemaining = maxSeconds;
			AudioManager.StartLevelMusic();
			EnteringLobby();

			if(isResetting)
			{
				isResetting = false;
				StartGame();
			}
        }
        else
        {
            Destroy(this);
        }
    }

    private void Update()
    {
		States currentState = GetCurrentState();

		switch (currentState) 
		{
		case States.ExitingApplication:
			ExitGame();
			break;
		case States.GameOver:
			return;
		case States.InLobby:
			InLobby();
			break;
		case States.PauseChanging:
			OnPauseChanging();
			break;
		case States.Paused:
			if (Input.GetKeyDown (KeyCode.R)) 
			{
				ResetGame();
			}
			break;
		default:
			ClockTick();    
			timerText.text = SecondsRemaining.ToString("F1"); // display one decimal place
			CheckForVictory ();
			gameOverText.enabled = IsGameOver;
			break;
		}
    }

	private States GetCurrentState()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) 
			return States.ExitingApplication;

		if (IsGameOver)
			return States.GameOver;

		if (Input.GetKeyDown (KeyCode.Space))
			return States.PauseChanging;

		if (isPaused)
			return States.Paused;

		if (isStarting)
			return States.InLobby;

		return States.InGame;
	}

	private void OnPauseChanging()
	{
		if(isPaused)
		{
			UnPause();
		}
		else
		{
			Pause();
		}
	}

	private void EnteringLobby()
	{
		FillInPlayerTable ();
	}

	private void InLobby ()
	{
		if (Input.GetKeyDown (KeyCode.S)) 
		{
			StartGame();
		}

		for(int i=1; i<=5; i++)
		{
			if(Input.GetButtonDown("JReadyButton" + i))
			{
				readyPlayers[i-1] = true;
				GameObject infoPanel = LobbyPanel.transform.FindChild ("PlayerInfo" + i).gameObject;
				infoPanel.transform.FindChild ("IsReadyImage").gameObject.SetActive(true);
			}

			if(readyPlayers.All( x => x ))	// check that all readyPlayers are true
			{
				StartGame();
			}
		}

	}

	private void FillInPlayerTable()
	{
		Color32[] colors = new Color32[5] {Color.green, Color.blue, Color.red, Color.yellow, Color.magenta };
		int colorBaseIndex = Random.Range (0, 4);
		
		for (int i=1; i<=5; i++) {
			GameObject infoPanel = LobbyPanel.transform.FindChild ("PlayerInfo" + i).gameObject;
			Text playerName = infoPanel.transform.FindChild ("PlayerName").GetComponent<Text> ();
			Text playerTeam = infoPanel.transform.FindChild ("PlayerTeam").GetComponent<Text> ();
			Text points = infoPanel.transform.FindChild ("Points").GetComponent<Text> ();
			GameObject playerReadyImage = infoPanel.transform.FindChild ("IsReadyImage").gameObject;
			
			playerName.color = colors [(colorBaseIndex + i) % 5];
			playerTeam.text = string.Empty;
			points.text = string.Empty;
			playerReadyImage.SetActive(false);
			
			readyPlayers[i-1] = false;
		}
	}

	private void StartGame()
	{
		isStarting = false;
		LobbyPanel.SetActive (false);
		Time.timeScale = 1;
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
			AudioManager.StartMenuMusic();
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

		if (SecondsRemaining <= timeRemainingWarningThreshold && !isEndGameWarningPlaying && !IsGameOver) 
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

	private void Pause()
	{
		isPaused = true;
		Time.timeScale = 0;
		pausePanel.SetActive (true);
	}

	private void UnPause()
	{
		isPaused = false;
		Time.timeScale = 1;
		pausePanel.SetActive (false);
		
	}

	private void ResetGame()
	{
		isResetting = true;
		Application.LoadLevel(Application.loadedLevel);
	}

	public void ExitGame()
	{
		Debug.Log ("Getting out of here!");
		Application.Quit ();
	}
}
