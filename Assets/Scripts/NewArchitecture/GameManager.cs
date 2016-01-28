using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// 
/// </summary>
public class GameManager : MonoBehaviour
{
    private float maxTime;
    private float currentTime;
    private int currentPlayers;
    private GameManager instance;
    public List<Rogue> rogues = new List<Rogue>();

    private void Start()
    {

    }

    private void Update()
    {

    }

    /// <summary>
    /// Tests For A Game Over Everyframe
    /// </summary>
    private void IsGameOver()
    {

    }

    /// <summary>
    /// Processes The Countdown Tick
    /// </summary>
    private void ClockTick()
    {
        
    }

    /// <summary>
    /// Registers a Rogue Element To The List
    /// </summary>
    /// <param name="rogue"></param>
    public void AddRogueElement(Rogue rogue)
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
