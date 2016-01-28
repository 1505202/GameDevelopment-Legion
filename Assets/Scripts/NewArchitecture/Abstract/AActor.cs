using UnityEngine;

/// <summary>
/// Abstract Actor, Both Legion And Rogues Extend This Class.
/// </summary>
public class AActor : MonoBehaviour
{
    protected float rotateSpeed = 0;
    protected float movementSpeed = 0;

    /// <summary>
    /// Updates Camera LayerID (To Work With Hiding Spots)
    /// </summary>
    /// <param name="layerID"></param>
    public void UpdateCameraLayermask(int layerID)
    {

    }
}
