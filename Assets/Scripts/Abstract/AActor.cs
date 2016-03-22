using UnityEngine;

/// <summary>
/// Abstract Actor, Both Legion And Rogues Extend This Class.
/// </summary>
public class AActor : MonoBehaviour
{
    [Header("Actor Movement Speed")]
    //[SerializeField] protected float rotateSpeed = 0;
    [SerializeField] protected float movementSpeed = 0;

    public int PlayerNumber { get; set; }
    protected int score = 0;
    public string Team { get; protected set; }
    protected static string legionTeamName = "Legion";
    protected static string rogueTeamName = "Rogue";

    /// <summary>
    /// Updates Camera LayerID (To Work With Hiding Spots)
    /// </summary>
    /// <param name="layerID"></param>
    public void UpdateCameraLayermask(int layerID)
    {
		gameObject.layer = 1 << layerID;
    }

    public int GetScore()
    {
        return score;
    }

    public void ModifyScore(int delta)
    {
        score += delta;
    }
}
