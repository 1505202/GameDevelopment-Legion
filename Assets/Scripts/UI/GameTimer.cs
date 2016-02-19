using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameTimer : MonoBehaviour
{
   [SerializeField] private Text timerText;

	void Update ()
	{
	  timerText.text = GameManager.Instance.SecondsRemaining.ToString();
	}
}
