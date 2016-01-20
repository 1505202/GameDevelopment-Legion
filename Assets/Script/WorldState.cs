using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldState : MonoBehaviour {

	private List<GameObject> characters = new List<GameObject>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GameObject[] legion = GameObject.FindGameObjectsWithTag ("Legion");
		GameObject[] roguePlayers = GameObject.FindGameObjectsWithTag ("RoguePlayer");
		GameObject[] rogueAI = GameObject.FindGameObjectsWithTag ("RogueAI");

		foreach (GameObject go in legion)
			characters.Add (go);
		foreach (GameObject go in roguePlayers)
			characters.Add (go);
		foreach (GameObject go in rogueAI)
			characters.Add (go);
	}

	private void clearState(){
		characters.Clear ();
	}
}
