using UnityEngine;
using System.Collections.Generic;

public class RopeSystem : MonoBehaviour 
{
    [SerializeField] private Transform target;
    [SerializeField] private LayerMask layerMask;

    [SerializeField] private float maxRopeDistance;

    private ConfigurableJoint joint;

    private Transform myTransform;

    private LineRenderer line;
    private List<Vector3> contactPoints = new List<Vector3>();
    private List<GameObject> intermediatePoints = new List<GameObject>();

    private SoftJointLimit limit = new SoftJointLimit();

    public float velocitymultiplier;

	private void Start () 
    {
        myTransform = GetComponent<Transform>();
        joint = GetComponent<ConfigurableJoint>();

        contactPoints.Add(target.position);
        contactPoints.Add(myTransform.position);

        line = GetComponent<LineRenderer>();

        limit.limit = maxRopeDistance;
        joint.linearLimit = limit;
	}
	private void Update () 
    {
        GetComponent<Rigidbody>().velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")) * velocitymultiplier;
        target.GetComponent<Rigidbody>().velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")) * velocitymultiplier;

        // updating LineRenderer Points
        UpdateEndPoints();
        UpdateLineRenderer();

        // Updating Joint
        UpdateJointLimits();

        if (UnWrap())
        {
            return;
        }

        if (Wrap())
        {
            return;
        }
	}

    private void UpdateEndPoints()
    {
        contactPoints[0] = myTransform.position;
        contactPoints[contactPoints.Count - 1] = target.position;
    }
    private void UpdateLineRenderer()
    {
        line.SetVertexCount(contactPoints.Count);
        for (int i = 0; i < contactPoints.Count; i++)
        {
            line.SetPosition(i, contactPoints[i]);
        }
    }

    private void UpdateJointLimits()
    {
        // Get Total Rope Distance
        float totalDistanceFromParent = 0;
        for (int i = 0; i < contactPoints.Count-1; i++)
        {
            totalDistanceFromParent += Vector3.Distance(contactPoints[i], contactPoints[i+1]);
        }
        Debug.Log(totalDistanceFromParent);

        if (totalDistanceFromParent > maxRopeDistance)
        {
            float delta = totalDistanceFromParent - maxRopeDistance;
            float epsilon = 0.000001f; // floating point values cannot be guarenteed to be exactly 0
            while (delta > epsilon)
            {
                float closestPointDistance = Vector3.Distance(contactPoints[0], contactPoints[1]);

                float limitDistance = delta > closestPointDistance ? 0 : (closestPointDistance - delta);

                limit.limit = limitDistance;
                joint.linearLimit = limit;

                if (limitDistance == 0)
                {
                    contactPoints.RemoveAt(1);
                    intermediatePoints.RemoveAt(0);

                    if (intermediatePoints.Count > 0)
                    {
                        joint.connectedBody = intermediatePoints[0].GetComponent<Rigidbody>();
                    }
                    else
                    {
                        joint.connectedBody = target.GetComponent<Rigidbody>();
                    }
                }
                delta -= closestPointDistance - limitDistance;
            }
        }

        // Reset Connected Body To Closest Point [Can Be Optimised by placing an event sorta system]
        if (intermediatePoints.Count > 0)
        {
            joint.connectedBody = intermediatePoints[0].GetComponent<Rigidbody>();
        }
        else
        {
            joint.connectedBody = target.GetComponent<Rigidbody>();
            limit.limit = 10;
            joint.linearLimit = limit;
        }
    }

    private bool UnWrap()
    {
        if(contactPoints.Count > 2)
        {
            RaycastHit hit;
            //// UnWrap From Target Side
            if (!Physics.Linecast(contactPoints[contactPoints.Count - 1], contactPoints[contactPoints.Count - 2], out hit, layerMask))
            {
                if (!Physics.Linecast(contactPoints[contactPoints.Count - 1], contactPoints[contactPoints.Count - 3], out hit, layerMask))
                {
                    contactPoints.RemoveAt(contactPoints.Count - 2);

                    GameObject obj = intermediatePoints[intermediatePoints.Count - 1];
                    intermediatePoints.Remove(obj);
                    Destroy(obj);

                    return true;
                }
            }

            // UnWrap From MySide
            if (!Physics.Linecast(contactPoints[0], contactPoints[1], out hit, layerMask))
            {
                if (!Physics.Linecast(contactPoints[0], contactPoints[2], out hit, layerMask)) // Have to see both to be a valid unwrappoint
                {
                    contactPoints.RemoveAt(1);

                    GameObject obj = intermediatePoints[intermediatePoints.Count - 1];
                    intermediatePoints.Remove(obj);
                    Destroy(obj);

                    return true;
                }
            }
        }
        return false;
    }
    private bool Wrap()
    {
        RaycastHit hit;
        // Wrap From Target Side
        if (Physics.Linecast(contactPoints[contactPoints.Count - 1], contactPoints[contactPoints.Count - 2], out hit, layerMask))
        {
            GameObject obj = new GameObject("ContactPoint" + (contactPoints.Count - 1), typeof(Rigidbody));
            Vector3 dirDiff = (hit.point - hit.collider.transform.position).normalized / 10; // So the normalized vector is substantially smaller
            obj.GetComponent<Transform>().position = hit.point + dirDiff;
            obj.GetComponent<Rigidbody>().isKinematic = true;
            contactPoints.Insert(contactPoints.Count - 1, hit.point + dirDiff);
            intermediatePoints.Add(obj);
            return true;
        }

        // Wrap From MySide
        if (Physics.Linecast(contactPoints[0], contactPoints[1], out hit, layerMask))
        {
            GameObject obj = new GameObject("ContactPoint" + (contactPoints.Count - 1), typeof(Rigidbody));
            Vector3 dirDiff = (hit.point - hit.collider.transform.position).normalized / 10;
            obj.GetComponent<Transform>().position = hit.point + dirDiff;
            obj.GetComponent<Rigidbody>().isKinematic = true;
            contactPoints.Insert(1, hit.point + dirDiff);
            intermediatePoints.Add(obj);
            return true;
        }
        return false;
    }
}
