using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RadarSystem : MonoBehaviour
{
	// Core components required by Radar
	public List<RadarBlip> _RadarBlips = new List<RadarBlip> ();
	public Material Blip;
	public ParticleRenderMode RenderMode;
	public GameObject CenterPoint;
	public float RadarScale;
	public LayerMask BlipLayer;
		
	// Used to find layer in from Layermask choice
	private int bit;
	
	// Does Blip use opacity over distance from center?
	public bool FadeOverDistance, drawLineHeights, OnPlane;
	
	//Particleemitter & renderer
	private ParticleEmitter _pe;
	private ParticleRenderer _pr;
	public Particle[] _rblips = new Particle[0];
	private int _frameCount = 0;
		
	// Use this for initialization
	public void Awake ()
	{
		
		if (CenterPoint == null)
			Debug.LogError ("No Center Point defined in Inspector");
		
		// Create Particle Emitter in Script
		_pe = (ParticleEmitter)gameObject.AddComponent ("EllipsoidParticleEmitter");
		_pe.emit = false;
		_pe.useWorldSpace = false;
		
		_pr = gameObject.AddComponent<ParticleRenderer> ();
		_pr.sharedMaterial = Blip;
		_pr.particleRenderMode = RenderMode;
		_pr.lengthScale = 0.0f;
		_pr.velocityScale = 1.0f;

		enabled = true;
		
		// Find our Layer based on the LayerMask from the Inspector
		int mask = (int)BlipLayer;	

		if (!Mathf.IsPowerOfTwo (mask))
			Debug.LogError ("Please assign only one Layer to RadarSystem, your height lines will not be visible");
				
		bit = 0;
		while (((mask = mask>>1) & 0x1) == 0) {
			bit++;
		}
		
		bit += 1;

		if (OnPlane) {
			drawLineHeights = false;
		}
	}
	
	/// <summary>
	/// Starts system.
	/// </summary>
	/// <param name='_center'>
	/// Supply player or GO that will form center point.
	/// </param>
	public void StartMe (GameObject _center)
	{
		CenterPoint = _center;
	}

	/// <summary>
	/// Adds the radar blip.
	/// </summary>
	/// <param name="go"> GameObject to Track</param>
	/// <param name="clr"> Color to pass to particle</param>
	public void AddRadarBlip (GameObject go, Color clr, float size)
	{
		// Create a Blip for each item on screen
		RadarBlip rb = new RadarBlip ()
		{
			Target = go,
			BlipColor = clr,
			BlipSize = size,
			BlipOpacity = 1.0f,
			BlipHeight = new GameObject ("Line")
		};
		
		if (drawLineHeights) {
			// Assign a line renderer for each RadarBlip to draw heights
			LineRenderer line = rb.BlipHeight.AddComponent<LineRenderer> ();
			rb.BlipHeight.layer = bit;
			rb.BlipHeight.transform.parent = this.transform;
			
			line.material = new Material (Shader.Find ("Mobile/Particles/Additive"));
			line.SetColors (Color.white, Color.white);
			line.SetWidth (0.02F, 0.02F);
			line.SetVertexCount (2);
			line.useWorldSpace = false;

			rb.BlipHeight.name = "Line " + go.name;
		}
		
		_RadarBlips.Add (rb);
	}
	
	/// <summary>
	/// Removes the radar blip.
	/// </summary>
	/// <param name='go'>
	/// Used to remove a blip in game - pass the GO and it will remove the corresponding blip
	/// </param>
	public void RemoveRadarBlip (GameObject go)
	{
		// Clear toDestroy
		int toDestroy = 0;
		// Set total amount of Blips to cycle through
		int max = _RadarBlips.Count;
		for (int i = 0; i < max; i++) {
			// Find Blip in List and set toDestroy
			if (go == _RadarBlips [i].Target)
				toDestroy = i;
		}
		//Remove RadarBlip from List & Linerenderer from scene
		Destroy (_RadarBlips [toDestroy].BlipHeight.gameObject);
		_RadarBlips.RemoveAt (toDestroy);
	}
	
	/// <summary>
	/// Cleans up function used when you need to clear all Blips in scene
	/// </summary>
	public void CleanUp ()
	{
		// Cycle through entire RadarBlip list
		for (int i = 0; i < _RadarBlips.Count; i++) {
			Destroy (_RadarBlips [i].BlipHeight.gameObject);
		}
		// Clear particles array
		_pe.ClearParticles ();
		// Clear RadarBlip List
		_RadarBlips.Clear ();
	}
	
	/// <summary>
	/// Update all entries in the RadarBlip list.
	/// </summary>
	void Update ()
	{
		// Test to see if we have a player
		if (CenterPoint != null) {
			
			// Resize array to latest Blip Count
			if (_rblips.Length != _RadarBlips.Count)
				System.Array.Resize (ref _rblips, _RadarBlips.Count);
				
			// Get Total Blips to update
			int max = _RadarBlips.Count;
			
			// Loop through all the Blips updating there position in relation to the Player ( Or GO chosen as Center point )
			for (int i = 0; i < max; i++) {
				
				// Only update distance check every 20 cycles - This is optimisation for mobile usage
//				if (_frameCount-- < 0) {
//					_frameCount = 20;
				float distance = (_RadarBlips [i].Target.transform.position - CenterPoint.transform.position).magnitude;
					
				// Specify RadarBlip opacity based on distance to center point
				_RadarBlips [i].BlipOpacity = Mathf.InverseLerp ((RadarScale * 3), 0, distance);
//				}
				
				// Set all elements of individual Radar Blip as we loop through
				if (!OnPlane) {
					_rblips [i].position = (_RadarBlips [i].Target.transform.position - CenterPoint.transform.position) / RadarScale;
				} else {
					_rblips [i].position = new Vector3 (_RadarBlips [i].Target.transform.position.x - CenterPoint.transform.position.x, 0, _RadarBlips [i].Target.transform.position.z - CenterPoint.transform.position.z) / RadarScale;
				}
				if (!FadeOverDistance) {
					// Specifies Blip Color - With no Opacity, set in inspector
					_rblips [i].color = new Color (_RadarBlips [i].BlipColor.r, _RadarBlips [i].BlipColor.g, _RadarBlips [i].BlipColor.b); 
				} else {
					// Specifies Blip Color - With Opacity
					_rblips [i].color = new Color (_RadarBlips [i].BlipColor.r, _RadarBlips [i].BlipColor.g, _RadarBlips [i].BlipColor.b, _RadarBlips [i].BlipOpacity);
				}
				
				_rblips [i].startEnergy = 3.0f;
				_rblips [i].energy = 3.0f;
				_rblips [i].velocity = Vector3.zero;
				_rblips [i].size = _RadarBlips [i].BlipSize;
						
				// Set rotation of Blip based on Player ( Or GO chosen as Center point )
				_RadarBlips [i].BlipHeight.transform.rotation = Quaternion.Inverse (CenterPoint.transform.rotation);
				
				// Test whether lineheights are to be drawn?
				if (drawLineHeights) {
					// Cache line renderer of this object
					LineRenderer line = _RadarBlips [i].BlipHeight.GetComponent<LineRenderer> ();
							
					// To keep Blips aligned to Center Point's horizontal axis & provide height above/below - Cast a plane from center point and get distance to plane
					Plane mPlane = new Plane (CenterPoint.transform.up, CenterPoint.transform.position);
					float length = mPlane.GetDistanceToPoint (_RadarBlips [i].Target.transform.position) / RadarScale;
			
					
					// Set line renderers position and height based on particle it follows
					// Color & Opacity of line is determined RadarBlip's Color
					line.SetPosition (0, _rblips [i].position);
					line.SetPosition (1, line.transform.InverseTransformPoint (line.transform.TransformPoint (_rblips [i].position) - length * Vector3.up));
					line.SetColors (_rblips [i].color, _rblips [i].color);
				}
			}
			
			// Test to check if Blips exists
			if (_rblips != null)
				
				// Otherwise pass Blips to Particle emitter & display
				_pe.particles = _rblips;
				
			_pe.transform.rotation = Quaternion.Inverse (CenterPoint.transform.rotation);
				
		}
	}
}


/// <summary>
/// Radar Blip GameObject to Track & specify: Size, Color & Opacity ( Height is used by internal methods.
/// </summary>
public class RadarBlip
{
	public GameObject Target;
	public Color BlipColor;
	public float BlipSize;
	public float BlipOpacity;
	public GameObject BlipHeight;
}