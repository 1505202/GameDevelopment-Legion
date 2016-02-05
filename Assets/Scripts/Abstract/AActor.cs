using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Abstract Actor, Both Legion And Rogues Extend This Class.
/// </summary>
public class AActor : NetworkBehaviour
{
    [SerializeField] protected float rotateSpeed = 0;
    [SerializeField] protected float movementSpeed = 0;

    /// <summary>
    /// Updates Camera LayerID (To Work With Hiding Spots)
    /// </summary>
    /// <param name="layerID"></param>
    public void UpdateCameraLayermask(int layerID)
    {
		gameObject.layer = (1 << layerID);
    }
}
