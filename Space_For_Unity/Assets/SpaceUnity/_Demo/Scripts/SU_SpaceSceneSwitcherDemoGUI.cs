// Space Scene Switcher Demo GUI C# Script (version: 1.03)
// SPACE UNITY - Space Scene Construction Kit
// http://www.spaceunity.com
// (c) 2013 Stefan Persson

// DESCRIPTION:
// Small demo script to allow space scene switchign with GUI buttons and function keys.

// INSTRUCTIONS:
// Attach this script to a gameobject in the scene.

// Version History
// 1.03 - Changed deprecated Camera.mainCamera to Camera.main
// 1.02 - Prefixed with SU_SpaceSceneSwitcherDemoGUI to avoid naming conflicts.
//        Added camera follow mode, particles, fog, and asteroid toggle.
//        Added documentation.
// 1.01 - Initial Release.

using UnityEngine;
using System.Collections;

public class SU_SpaceSceneSwitcherDemoGUI : MonoBehaviour {
	// Assign the space particles transform in the inspector (drag and drop from e.g. MainCamera/SpaceParticles)
	public Transform spaceParticles;
	// Bool variables particles on/off via GUI (needs old value due to nature of GUI.Toggle)
	private bool _spaceParticles = true;
	private bool _oldSpaceParticles = true;
	// Assign the space fog transform in the inspector (drag and drop from e.g. MainCamera/SpaceFog)
	public Transform spaceFog;
	// Bool variables fog on/off via GUI (needs old value due to nature of GUI.Toggle)
	private bool _spaceFog = true;
	private bool _oldSpaceFog = true;
	// Assign the asteroid transform in the inspector (drag and drop from e.g. Spaceship/Asteroids)
	public Transform spaceAsteroids;	
	// Bool variables asteroids on/off via GUI (needs old value due to nature of GUI.Toggle)
	private bool _spaceAsteroids = true;
	private bool _oldSpaceAsteroids = true;
	
	
	void OnGUI() {
		// Create a GUI button to switch to SpaceScene1
		if (GUI.Button(new Rect(10,10,180,25), "Switch to Scene 1 (F1)")) {
			SU_SpaceSceneSwitcher.Switch("SpaceScene1");		
		}
		// Create a GUI button to switch to SpaceScene2
		if (GUI.Button(new Rect(10,40,180,25), "Switch to Scene 2 (F2)")) {
			SU_SpaceSceneSwitcher.Switch("SpaceScene2");
		}
		
		// Switch Camera Follow mode
		SU_CameraFollow _cameraFollow = Camera.main.GetComponent<SU_CameraFollow>();
		if (GUI.Button(new Rect(10,70,180,25), "Camera " + _cameraFollow.followMode.ToString())) {			
			if (_cameraFollow.followMode == SU_CameraFollow.FollowMode.CHASE) {
				// Set following spectator mode
				_cameraFollow.followMode = SU_CameraFollow.FollowMode.SPECTATOR;
			} else {
				// Set chase view mode
				_cameraFollow.followMode = SU_CameraFollow.FollowMode.CHASE;
			}
		}
		
		// Handle Space Particles on/off
		_spaceParticles = GUI.Toggle(new Rect(10,100,180,25), _spaceParticles, "Space Particles");
		if (_oldSpaceParticles != _spaceParticles) {
			// Use the SU_SpaceSceneSwitcher to set active which takes into consideration compiler specific code
			SU_SpaceSceneSwitcher.SetActive(spaceParticles.gameObject, _spaceParticles);
			// Remember the value, workaround for how GUI.Toggle works when you can't perform the if statement on the actual toggle
			_oldSpaceParticles = _spaceParticles;			
		}
		
		// Handle Space Fog on/off
		_spaceFog = GUI.Toggle(new Rect(10,130,180,25), _spaceFog, "Space Fog");
		if (_oldSpaceFog != _spaceFog) {
			// Use the SU_SpaceSceneSwitcher to set active which takes into consideration compiler specific code
			SU_SpaceSceneSwitcher.SetActive(spaceFog.gameObject, _spaceFog);
			// Remember the value, workaround for how GUI.Toggle works when you can't perform the if statement on the actual toggle
			_oldSpaceFog = _spaceFog;					
		}
		
		// Handle Asteroids on/off - when the asteroid object is disabled, it will detect this and disable any active asteroids in the scene
		_spaceAsteroids = GUI.Toggle(new Rect(10,160,180,25), _spaceAsteroids, "Space Asteroids");
		if (_oldSpaceAsteroids != _spaceAsteroids) {
			// Use the SU_SpaceSceneSwitcher to set active which takes into consideration compiler specific code
			SU_SpaceSceneSwitcher.SetActive(spaceAsteroids.gameObject, _spaceAsteroids);
			// Remember the value, workaround for how GUI.Toggle works when you can't perform the if statement on the actual toggle
			_oldSpaceAsteroids = _spaceAsteroids;

		}			
	}
		
	void Update () {
		// Hotkey set to F1 to switch to SpaceScene1
		if (Input.GetKeyDown(KeyCode.F1)) {
			SU_SpaceSceneSwitcher.Switch("SpaceScene1");
		}
		// Hotkey set to F2 to switch to SpaceScene1
		if (Input.GetKeyDown(KeyCode.F2)) {
			SU_SpaceSceneSwitcher.Switch("SpaceScene2");
		}		
	}	
}
