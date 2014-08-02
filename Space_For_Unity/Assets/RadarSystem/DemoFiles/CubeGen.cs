using UnityEngine;
using System.Collections;

public class CubeGen : MonoBehaviour {
	
	
	public bool cubesOn;
	
	// Used to provide Inspector Variable in Editor
	public RadarSystem _RadarSystem;
	
	
	public GameObject _cubes;
	public int _numCubes;
	public int _cubesSpawnRadius;
	
	public GameObject Parent;
		
	void Start ()
	{
				
		if (cubesOn) {
			for (int i = 1; i < _numCubes; i++) {
				GameObject cuboid = Instantiate (_cubes) as GameObject;
				cuboid.transform.position = Random.insideUnitSphere * _cubesSpawnRadius;
				cuboid.transform.eulerAngles = new Vector3 (Random.Range (0, 90), Random.Range (0, 90), Random.Range (0, 90));
				cuboid.transform.parent = Parent.transform;
				
				// Attach this object to the Radar list - Specifies Object, Color and Size
				_RadarSystem.AddRadarBlip (cuboid, Color.red, 0.5f);
				
				
			}
		}		
	}
}