using UnityEngine;
/// <summary>
/// Abstract Controller, Both AI and JInput Extend This Class
/// </summary>
public class AController : MonoBehaviour
{
    protected Vector3 direction;
    public Vector3 Direction
    {
        get
        {
            return direction;
        }
        set
        {
            direction = value;
        }
    }
}
