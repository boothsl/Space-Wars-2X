using UnityEngine;
using System.Collections;

public class SlowRotate : MonoBehaviour {

	Transform myTransform;
	
	public enum spinDirection
		{
		round,
		diagonal
		}
	
	public enum axis
	{
		x,
		y,
		z
	}
	
	public axis myAxis;
	
	public spinDirection mySpin;
	
	public float mySpeed;
	
	Vector3 spin;
	
	// Use this for initialization
	void Start () {
		myTransform = transform;
		
		switch (mySpin)
		{
		case spinDirection.round:
			if ( myAxis == axis.x )
			spin = new Vector3 ( 90, 0, 0 );
			else if ( myAxis == axis.y)
			spin = new Vector3 ( 0, 90, 0 );
			else if (myAxis == axis.z)
			spin = new Vector3 ( 0, 0, 90 );	
			
			break;
		case spinDirection.diagonal:
			spin = new Vector3 ( 0, 45, 90);
			break;
		}
		
	}
	
	// Update is called once per frame
	void Update () {
	    myTransform.Rotate( (spin * Time.deltaTime / 60)*mySpeed);
	}
}