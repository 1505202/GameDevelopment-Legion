using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Legion Actor
/// </summary>
public class Legion : AActor
{	
    [SerializeField] private AController inputController;

	// ComponentCaching
	private Rigidbody myRigidbody = null;
	private Transform myTransform = null;

	private void Start()
	{
        // Component Caching
		myRigidbody = GetComponent<Rigidbody>();
		myTransform = GetComponent<Transform>();

		// Add Legion Camera
		inputController = ControllerManager.Instance.NewController();
	}
	private void Update()
    {
		myRigidbody.velocity = inputController.MoveDirection() * movementSpeed;

	    if (inputController.MoveDirection() != Vector3.zero)
	    {
	        Quaternion lookRotation = Quaternion.LookRotation(inputController.MoveDirection());
	        myTransform.rotation = Quaternion.Slerp( myTransform.rotation, lookRotation, Time.deltaTime * rotateSpeed);
	    }
	}

	#region Properties

	public AController Controller
	{
		get { return inputController; }
		set { inputController = value; }
	}

	#endregion
}
