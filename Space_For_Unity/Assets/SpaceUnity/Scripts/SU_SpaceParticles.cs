// Space Particles C# Script (version: 1.02)
// SPACE UNITY - Space Scene Construction Kit
// http://www.spaceunity.com
// (c) 2013 Stefan Persson

// DESCRIPTION:
// Script spawns particles in a sphere around its parent.
// The particles live for an infinite period of time but they will be relocated when they
// are beyond "range".

// This script requires the prefab "SpaceUnity\Prefabs\CameraEffects\SpaceParticles" which has a
// particle system that this script depends on.

// INSTRUCTIONS:
// Drag the SpaceParticles/SpaceFog prefab to your scene and make it a child of the camera (e.g. Main Camera)

// PARAMETERS:
//   particleScale	(Scale of particles - used to scale the size of particles of the Particle System)
//   range			(Range of the sphere that the particles live within)
//   maxParticles	(Number of particles, modify this to suit your visual and performance needs)
//   distanceSpawn  (Distance % (0.0-1.0) of range where new particles should spawn)
//   fadeParticles	(Whether particles should alpha fade in/out when close to out of range)
//   distanceFade   (Start fading from % (0.0-1.0) of range (should be lower than distanceSpawn))

// HINTS:
// You can also modify the particle system of the prefab to modify particle texture, size, speed, colors, etc.

// Version History
// 1.02 - Prefixed with SU_SpaceParticles to avoid naming conflicts.
// 1.01 - Initial Release.

using UnityEngine;
using System.Collections;

public class SU_SpaceParticles : MonoBehaviour {	
	// Maximum number of particles in the sphere (configure to your needs for look and performance)
	public int maxParticles = 1000;
	// Range of particle sphere (when particles are beyond this range from its 
	// parent they will respawn (relocate) to within range at distanceSpawn of range.
	public float range = 200.0f;
	// Distance percentile of range to relocate/spawn particles to
	public float distanceSpawn = 0.95f;	
	// Minimum size of particles
	public float minParticleSize = 0.5f;
	// Maximum size of particles
	public float maxParticleSize = 1.0f;	
	// Multiplier of size
	public float sizeMultiplier = 1.0f;	
	// Minimum drift/movement speed of particles
	public float minParticleDriftSpeed = 0.0f;
	// Maximum drift/movement speed of particles
	public float maxParticleDriftSpeed = 1.0f;
	// Multiplier of driftSpeed
	public float driftSpeedMultiplier = 1.0f;	
	// Fade particles in/out of range (usually not necessary for small particles)
	public bool fadeParticles = true;
	// Distance percentile of range to start fading particles (should be lower than distanceSpawn)
	public float distanceFade = 0.5f;

	// Private variables
	private float _distanceToSpawn;
	private float _distanceToFade;
	private ParticleSystem _cacheParticleSystem;
	private Transform _cacheTransform;
	
	void Start () {
		// Cache transform and particle system to improve performance
		_cacheTransform = transform;
		_cacheParticleSystem = particleSystem;
		// Calculate the actual spawn and fade distances
		_distanceToSpawn = range * distanceSpawn;
		_distanceToFade = range * distanceFade;
		
		// Scale particles
		if (_cacheParticleSystem == null) {
			// Throw an error if the object of this script has no particle system
			Debug.LogError("This script must be attached to a GameObject with a particle system. It is strongly recommended " +
							"that you use the SpaceParticles or SpaceFog prefab which have suitable particle systems)");
		}
		
		// Spawn all new particles within a sphere in range of the object
		for (int i=0; i < maxParticles; i ++) {
			ParticleSystem.Particle _newParticle = new ParticleSystem.Particle();					
			_newParticle.position = _cacheTransform.position + (Random.insideUnitSphere * _distanceToSpawn);
			_newParticle.lifetime = Mathf.Infinity;
			Vector3 _velocity = new Vector3(
				Random.Range(minParticleDriftSpeed, maxParticleDriftSpeed) * driftSpeedMultiplier, 
				Random.Range(minParticleDriftSpeed, maxParticleDriftSpeed) * driftSpeedMultiplier, 
				Random.Range(minParticleDriftSpeed, maxParticleDriftSpeed) * driftSpeedMultiplier);			
			_newParticle.velocity = _velocity;						
			_newParticle.size = Random.Range(minParticleSize, maxParticleSize) * sizeMultiplier;								
			particleSystem.Emit(1);
			
		}			
	}
	
	void Update () {
		int _numParticles = particleSystem.particleCount;		
		// Get the particles from the particle system
		ParticleSystem.Particle[] _particles = new ParticleSystem.Particle[_numParticles];
		particleSystem.GetParticles(_particles);
		
		// Iterate through the particles and relocation (spawn) and fading
		for (int i = 0; i < _particles.Length; i++) {			
			// Calcualte distance to particle from transform position
			float _distance = Vector3.Distance(_particles[i].position, _cacheTransform.position);			
			
			// If distance is greater than range...
			if (_distance > range) {						
				// reposition (respawn) particle according to spawn distance
				_particles[i].position = Random.onUnitSphere * _distanceToSpawn + _cacheTransform.position;								
				// Re-calculate distance to particle for fading
				_distance = Vector3.Distance(_particles[i].position, _cacheTransform.position);				
				// Set a new velocity of the particle
				Vector3 _velocity = new Vector3(
					Random.Range(minParticleDriftSpeed, maxParticleDriftSpeed) * driftSpeedMultiplier, 
					Random.Range(minParticleDriftSpeed, maxParticleDriftSpeed) * driftSpeedMultiplier, 
					Random.Range(minParticleDriftSpeed, maxParticleDriftSpeed) * driftSpeedMultiplier);			
				_particles[i].velocity = _velocity;
				// Set a new size of the particle
				_particles[i].size = Random.Range(minParticleSize, maxParticleSize) * sizeMultiplier;						
			}
			
			// If particle fading is enabled...
			if (fadeParticles) {
				// Get the original color of the particle
				Color _col = _particles[i].color;				
				if (_distance > _distanceToFade) {		
					// Fade alpha value of particle between fading distance and spawnin distance
					_particles[i].color = new Color(_col.r, _col.g, _col.b, Mathf.Clamp01(1.0f - ((_distance - _distanceToFade) / (_distanceToSpawn - _distanceToFade))));						
				} else {
					// Particle is within range so ensure it has full alpha value
					_particles[i].color = new Color(_col.r, _col.g, _col.b, 1.0f);						
				}
			}
       	}        
		
		// Set the particles according to above modifications
    	particleSystem.SetParticles(_particles, _numParticles);    	
	}
}
