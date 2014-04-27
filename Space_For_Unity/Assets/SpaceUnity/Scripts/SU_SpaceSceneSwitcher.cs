// Space Scene Switcher C# Script (version: 1.03)
// SPACE UNITY - Space Scene Construction Kit
// http://www.spaceunity.com
// (c) 2013 Stefan Persson

// DESCRIPTION:
// Switches between Space Scenes that have been saved as Prefabs in the Project window.

// INSTRUCTIONS:
// Drag the SpaceSceneSwitcher Prefab (SpaceUnity/Prefabs/Tools/SpaceSceneSwitcher) into your scene,
// or drag this script onto a game object in your scene.
// Using the inspector, configure the array Space Scenes by dragging Space Scenes prefabs onto the array.
// Set mode to LOAD_ALL_AT_STARTUP or LOAD_ON_DEMAND depending on when you want to instantiate the space scene prefabs
// Set sceneIndexLoadFirst to the index of the element/prefab in the array you want to be loaded once initialied


// PARAMETERS:
//  mode				LOAD_ALL_AT_STARTUP or LOAD_ON_DEMAND depending on when you want to load and instantiate the Space Scene prefabs
//	spaceScenes			Array containing the Space Scene Prefabs (Note! Prefabs, not scenes!) you want to be able to switch between
//	sceneIndexLoadFirst The index number of the element/prefab in the array you wish to activate once initiated, default is 0

// Version History
// 1.03 - Fixed deprecated code (changed Camera.mainCamera to Camera.main)
// 1.02 - Prefixed with SU_SpaceSceneSwitcher to avoid naming conflicts.
//        Fixed warning message in Unity 4.0 by verifying compiler version to
//          not use deprecated function SetActiveRecursively.
// 1.01 - Initial Release.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SU_SpaceSceneSwitcher : MonoBehaviour {
	// Modes available:
	//	LOAD_ALL_AT_STARTUP = load all space scene prefabs at startup and instatiate (but disable) them
	//	LOAD_ON_DEMAND (default) = load space scene prefabs and instaniate when needed (destroy previous instance)
	public enum Mode {LOAD_ALL_AT_STARTUP, LOAD_ON_DEMAND}
	// The mode which can be configured in the inspector
	public Mode mode = Mode.LOAD_ON_DEMAND;	
	// The static mode which cannot be configured through inspector. The static value is set to the same value as the
	// value for non-static at startup.
	static public Mode staticMode;	
	// Array of Space Scene prefabs, configured in the inspector, that can be switched between
	public GameObject[] spaceScenes;
	// Static list of Space Scene prefabs (static variables cannot be configured through the inspector so the
	// values are transferred from spaceScenes to this static list upon start.	
	static public List<GameObject> staticSpaceScenes = new List<GameObject>();
	//static public GameObject[] staticSpaceScenes = new GameObject[32];
	// Hashtable containing instantiated scenes when LOAD_ALl_AT_STARTUP mode is used
	static public Hashtable hSpaceScenes = new Hashtable();
	// Reference to the current space scene so we can destroy it if mode is LOAD_ON_DEMAND as we switch mode
	static public GameObject currentSpaceScene;
	// Space Scene Prefab to instantiate/activate first by index in array once initiated
	public int sceneIndexLoadFirst = 0;
		
	
	// Configure as the script is started
	void Start () {
		// Set the staticMode to mode configured in Inspector
		staticMode = mode;
		
		// If the Space Scenes array has been configured in inspector...
		if (spaceScenes.Length > 0) {			
			// And if the static space scenes list has not been populated yet...
			if (staticSpaceScenes.Count == 0) {
				// Loop through all scenes configured in the inspector
				for (int _i = 0; _i < spaceScenes.Length; _i++) {
					// Add the space scene prefab to the static list
					staticSpaceScenes.Add(spaceScenes[_i]);
				}
			}
		} else {
			Debug.LogError("No Space Scene Prefabs configured for the Space Scene array. Populate array in the inspector with Space Scene prefabs " +
				"from the Project window. Note! You have to create Prefabs(!) of the space scenes - you cannot assign Unity Scenes to the array.");
		}
				
		// If mode is LOAD_ALL_AT_STARTUP all space scenes prefabs should be instantiated (but disabled)...
		if (staticMode == Mode.LOAD_ALL_AT_STARTUP) {
			// Clear the hashtable of instantiated prefabs, if there are any.
			hSpaceScenes.Clear();
			// Loop through the space scene prefabs in the array configured in the inspector
			foreach (GameObject _spaceScene in spaceScenes) {
				// Instantiate the space scenes as game objects
				GameObject _instantiated = (GameObject) GameObject.Instantiate(_spaceScene, new Vector3 (0,0,0), new Quaternion(0, 0, 0, 0));
				// Add the instanitated game objects to the hashtable
				hSpaceScenes.Add(_spaceScene.name, _instantiated);
				// Disable the instantiated object to hide it
				SetActive(_instantiated, false);
			}
		}			
		
		if (sceneIndexLoadFirst >= spaceScenes.Length) {
			// sceneIndexLoadFirst is greater than the array size... that's no good, load the first instead
			Debug.LogWarning("Scene Index Load First value is greater than the number of Space Scene prefabs in the array. " +
				"Loading scene with index 0 instead.");
			sceneIndexLoadFirst = 0;
			Switch(sceneIndexLoadFirst);						
			
		} else {
			Switch(sceneIndexLoadFirst);
		}
	}
	
	/// <summary>
	/// Switch between Space Scene prefabs by array index (int)
	/// </summary>
	/// <param name='_arrayIndex'>
	///  The integer array index of the space scene prefab to switch to
	/// </param>
	static public void Switch(int _arrayIndex) {
		if (staticSpaceScenes.Count > 0) {
			Switch(staticSpaceScenes[_arrayIndex].name);
		}
	}
	/// <summary>
	/// Switch between Space Scene prefabs
	/// </summary>
	/// <param name='_sceneName'>
	///  The name (case sensitive) of the space scene prefab to be instantiated / enabled
	/// </param>
	static public void Switch(string _sceneName) {	
		// Loop through the space scenes configured in the inspector (which have been copied to the static list)
		for (int _i = 0; _i < staticSpaceScenes.Count; _i++) {
			// Reference the space scene prefab from the list
			GameObject _spaceScene = staticSpaceScenes[_i];
			// If the space scene is not null...
			if (_spaceScene != null) {												
				// ...and the space scene name matches the space scene we want to switch to...
				if (_spaceScene.name == _sceneName) {										
					// ...and if the mode was set to LOAD_ALL_AT STARTUP...
					if (staticMode == Mode.LOAD_ALL_AT_STARTUP) {
						// ...and the hashtable entry for the space scene prefab name is not null...
						if (hSpaceScenes[_sceneName] != null) {
							// We need to flag if we found and enabled the space scene game object
							bool _found = false;
							// Loop through all the entries in the hashtable...
							foreach (DictionaryEntry _entry in hSpaceScenes) {								
								// if the hashtable entry is the scene we want to switch to...
								if (_entry.Key.ToString() == _sceneName) { 
									// Set the space scene to active
									SetActive((GameObject) _entry.Value, true);									
									_found = true;
								} else {
									// This is not the scene we want to switch to, make sure it is disabled 
									SetActive((GameObject) _entry.Value, false);	
								}
							}
							// If the instantiated space scene game object was found and enabled, return to avoid throwing the error below
							if (_found) return;
						}
					} else {
						// mode was set to LOAD_ON_DEMAND
						// Destroy the current instantiated space scene
						Destroy(currentSpaceScene);
						// instantiate the new space scene
						currentSpaceScene = (GameObject) GameObject.Instantiate(_spaceScene, new Vector3 (0,0,0), new Quaternion(0, 0, 0, 0));
						return;
					}
				}				
			}
		}
		Debug.LogWarning("Tried to switch to a space scene named " + _sceneName + " but the scene was not found. " +
			"Ensure that you configured the array on the SpaceSceneSwitcher prefab correctly and that you typed the name " +
			"of the space scene prefab correctly (case sensitive) for the Switch function call");
	}	
	
	// Since SetActiveRecursively has been deprecated this function performs game object 
	// activation correctly based regardless of Unity version.
	public static void SetActive (GameObject _gameObject, bool _bool) {
		#if UNITY_3_5
		if (_gameObject != null) _gameObject.SetActiveRecursively(_bool);
		#endif
		#if UNITY_4_0
		if (_gameObject != null) _gameObject.SetActive(_bool);
		#endif
	}
}
