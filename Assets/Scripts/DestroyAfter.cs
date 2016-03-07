using UnityEngine;
using System.Collections;

public class DestroyAfter : MonoBehaviour 
{
    [SerializeField] private float timeToDie = 0.0f;
    private float startTime;

    private void Start()
    {
        startTime = Time.time;
    }

    private void Update () 
    {
	    if(Time.time > startTime + timeToDie)
        {
            Destroy(this.gameObject);
        }
	}
}
