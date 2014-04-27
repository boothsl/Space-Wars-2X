// Asteroid Field Editor C# Script (version: 1.03)
// SPACE UNITY - Space Scene Construction Kit
// http://www.spaceunity.com
// (c) 2013 Stefan Persson

// DESCRIPTION
// This is an editor script (must reside in the /Editor project folder.) The purpose of
// the script is to improve presentation and configuration of the AsteroidField.cs script
// used to create asteroid fields.

// INSTRUCTIONS
// You don't need to do anything with this script, it will automatically detect objects
// that use the C# script AsteroidField and override the inspector to simplify configuration
// of the script parameters.

// Version History
// 1.03 - Removed deprecated EditorGUIUtility.LookLikeInspector() and EditorGUIUtility.LookLikeControls()
//        Changed deprecated Camera.mainCamera to Camera.main
// 1.02 - Prefixed with SU_AsteroidFieldEditor to avoid naming conflicts.
// 0.8 - Initial Release.

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SU_AsteroidField))]
public class SU_AsteroidFieldEditor : Editor {
	// Range Values Configuration
	private int _displayMinAsteroidCount = 1;
	private int _displayMaxAsteroidCount = 3000;
	private int _displayMinRange = 10;
	private int _displayMaxRange = 100000;
	
	// Warning Threshholds
	private int _warningHighAsteroidCount = 1000;

	// Serialized Object
	SerializedObject myTarget;
	
	// Serialized Properties
	SerializedProperty polyCount;
	SerializedProperty polyCountCollider;
	SerializedProperty maxAsteroids;
	SerializedProperty respawnIfOutOfRange;
	SerializedProperty respawnDestroyedAsteroids;
	SerializedProperty range;
	SerializedProperty distanceSpawn;
	SerializedProperty fadeAsteroids;
	SerializedProperty distanceFade;
	SerializedProperty minAsteroidScale;
	SerializedProperty maxAsteroidScale;
	SerializedProperty scaleMultiplier;
	SerializedProperty minAsteroidRotationSpeed;
	SerializedProperty maxAsteroidRotationSpeed;
	SerializedProperty rotationSpeedMultiplier;
	SerializedProperty minAsteroidDriftSpeed;
	SerializedProperty maxAsteroidDriftSpeed;
	SerializedProperty driftSpeedMultiplier;
	SerializedProperty isRigidbody;
	SerializedProperty mass;
	SerializedProperty minAsteroidAngularVelocity;
	SerializedProperty maxAsteroidAngularVelocity;
	SerializedProperty angularVelocityMultiplier;
	SerializedProperty minAsteroidVelocity;
	SerializedProperty maxAsteroidVelocity;
	SerializedProperty velocityMultiplier;
	
	// Temporary variables since properties can't be modified directly when using Ref and/or Out paremeters
	private float _minScale;
	private float _maxScale;
	private float _minRotationSpeed;
	private float _maxRotationSpeed;
	private float _minDriftSpeed;
	private float _maxDriftSpeed;
	private float _minAngularVelocity;
	private float _maxAngularVelocity;
	private float _minVelocity;
	private float _maxVelocity;
	
	// Bool display collapse/expand section helpers
	private bool _showPrefabs;
	private bool _showMaterials;		
	
	void OnEnable() {
		// Reference the serialized object (instance of AsteroidField.cs)
		myTarget = new SerializedObject(target);
	
		// Find and reference the properties of the target object		
		polyCount = myTarget.FindProperty("polyCount");
		polyCountCollider = myTarget.FindProperty("polyCountCollider");
		maxAsteroids = myTarget.FindProperty("maxAsteroids");
		respawnIfOutOfRange = myTarget.FindProperty("respawnIfOutOfRange");
		respawnDestroyedAsteroids = myTarget.FindProperty("respawnDestroyedAsteroids");
		range = myTarget.FindProperty("range");
		distanceSpawn = myTarget.FindProperty("distanceSpawn");
		fadeAsteroids = myTarget.FindProperty("fadeAsteroids");
		distanceFade = myTarget.FindProperty("distanceFade");
		minAsteroidScale = myTarget.FindProperty("minAsteroidScale");
		maxAsteroidScale = myTarget.FindProperty("maxAsteroidScale");
		scaleMultiplier = myTarget.FindProperty("scaleMultiplier");
		minAsteroidRotationSpeed = myTarget.FindProperty("minAsteroidRotationSpeed");
		maxAsteroidRotationSpeed = myTarget.FindProperty("maxAsteroidRotationSpeed");
		rotationSpeedMultiplier = myTarget.FindProperty("rotationSpeedMultiplier");
		minAsteroidDriftSpeed = myTarget.FindProperty("minAsteroidDriftSpeed");
		maxAsteroidDriftSpeed = myTarget.FindProperty("maxAsteroidDriftSpeed");
		driftSpeedMultiplier = myTarget.FindProperty("driftSpeedMultiplier");
		isRigidbody = myTarget.FindProperty("isRigidbody");
		minAsteroidAngularVelocity = myTarget.FindProperty("minAsteroidAngularVelocity");
		maxAsteroidAngularVelocity = myTarget.FindProperty("maxAsteroidAngularVelocity");
		angularVelocityMultiplier = myTarget.FindProperty("angularVelocityMultiplier");
		minAsteroidVelocity = myTarget.FindProperty("minAsteroidVelocity");
		maxAsteroidVelocity = myTarget.FindProperty("maxAsteroidVelocity");
		velocityMultiplier = myTarget.FindProperty("velocityMultiplier");
		mass = myTarget.FindProperty("mass");
		
	}		
		
	// Override the OnInspectorGUI and present these EditorGUI gadgets instead of the default ones
    public override void OnInspectorGUI() {
		// Update the serialized object
		myTarget.Update();		
		
		// Present inspector GUI gadgets/objects and modify AsteroidField.cs instances with configured values
		maxAsteroids.intValue = EditorGUILayout.IntSlider("Number of Asteroids", maxAsteroids.intValue, _displayMinAsteroidCount, _displayMaxAsteroidCount);
		if (maxAsteroids.intValue > _warningHighAsteroidCount) {
			EditorGUILayout.LabelField("Warning! Many asteroids may impact performance! Consider smaller range and fewer asteroids instead.", EditorStyles.wordWrappedMiniLabel);
		}
		range.floatValue = EditorGUILayout.Slider("Range", range.floatValue, _displayMinRange, _displayMaxRange);	
		if (range.floatValue > Camera.main.farClipPlane) {
			EditorGUILayout.LabelField("Warning! Main camera clipping plane is closer than asteroid range.", EditorStyles.wordWrappedMiniLabel);
		}
		EditorGUILayout.LabelField("Range is distance from the center to the edge of the asteroid field. If the transform of the AsteroidField moves, asteroids " +
			"that become out of range will respawn to a new location at spawn distance of range.", EditorStyles.wordWrappedMiniLabel);		
		respawnIfOutOfRange.boolValue = EditorGUILayout.Toggle("Respawn if Out of Range", respawnIfOutOfRange.boolValue);
		EditorGUILayout.LabelField("Note: Respawn if out of range must be enabled for endless/infinite asteroid fields", EditorStyles.wordWrappedMiniLabel);
		respawnDestroyedAsteroids.boolValue = EditorGUILayout.Toggle("Respawn if Destroyed", respawnDestroyedAsteroids.boolValue);
		EditorGUILayout.Separator();
		distanceSpawn.floatValue = EditorGUILayout.Slider("Spawn at % of Range", distanceSpawn.floatValue, 0.0f, 1.0f);		
		EditorGUILayout.Separator();
		
		EditorGUILayout.LabelField("Asteroid Scale (Min/Max Range)", EditorStyles.boldLabel);
		_minScale = minAsteroidScale.floatValue;
		_maxScale = maxAsteroidScale.floatValue;
		GUIContent _scaleContent = new GUIContent( string.Format( "Min:{0:F1}, Max:{1:F1}", _minScale , _maxScale ) );
		EditorGUILayout.MinMaxSlider(_scaleContent, ref _minScale, ref _maxScale, 0.1f, 1.0f);
		minAsteroidScale.floatValue = _minScale;
		maxAsteroidScale.floatValue = _maxScale;
		scaleMultiplier.floatValue = EditorGUILayout.FloatField( "Scale Multiplier", scaleMultiplier.floatValue);		
		EditorGUILayout.Separator();
		
		// Rigidbody or non-rigidbody Asteroids
		isRigidbody.boolValue = EditorGUILayout.Toggle("Is Rigidbody", isRigidbody.boolValue);		
		if (isRigidbody.boolValue) {
			mass.floatValue = EditorGUILayout.FloatField("Mass (scales with size)", mass.floatValue);
			EditorGUILayout.LabelField("Asteroid Angular Velocity (Min/Max Range)", EditorStyles.boldLabel);		
			_minAngularVelocity = minAsteroidAngularVelocity.floatValue;
			_maxAngularVelocity = maxAsteroidAngularVelocity.floatValue;
			GUIContent _rotationContent = new GUIContent( string.Format( "Min:{0:F1}, Max:{1:F1}", _minAngularVelocity , _maxAngularVelocity ) );
			EditorGUILayout.MinMaxSlider(_rotationContent, ref _minAngularVelocity, ref _maxAngularVelocity, 0.0f, 1.0f);
			minAsteroidAngularVelocity.floatValue = _minAngularVelocity;
			maxAsteroidAngularVelocity.floatValue = _maxAngularVelocity;		
			angularVelocityMultiplier.floatValue = EditorGUILayout.FloatField( "Rotation Speed Multiplier", angularVelocityMultiplier.floatValue);
			
			EditorGUILayout.LabelField("Asteroid Velocity (Min/Max Range)", EditorStyles.boldLabel);			
			_minVelocity = minAsteroidVelocity.floatValue;
			_maxVelocity = maxAsteroidVelocity.floatValue;
			GUIContent _driftContent = new GUIContent( string.Format( "Min:{0:F1}, Max:{1:F1}", _minVelocity , _maxVelocity ) );
			EditorGUILayout.MinMaxSlider(_driftContent, ref _minVelocity, ref _maxVelocity, 0.0f, 1.0f);
			minAsteroidVelocity.floatValue = _minVelocity;
			maxAsteroidVelocity.floatValue = _maxVelocity;
			velocityMultiplier.floatValue = EditorGUILayout.FloatField( "Drift Speed Multiplier", velocityMultiplier.floatValue);			
		} else {
			EditorGUILayout.LabelField("Asteroid Rotation Speed (Min/Max Range)", EditorStyles.boldLabel);		
			_minRotationSpeed = minAsteroidRotationSpeed.floatValue;
			_maxRotationSpeed = maxAsteroidRotationSpeed.floatValue;
			GUIContent _rotationContent = new GUIContent( string.Format( "Min:{0:F1}, Max:{1:F1}", _minRotationSpeed , _maxRotationSpeed ) );
			EditorGUILayout.MinMaxSlider(_rotationContent, ref _minRotationSpeed, ref _maxRotationSpeed, 0.0f, 1.0f);
			minAsteroidRotationSpeed.floatValue = _minRotationSpeed;
			maxAsteroidRotationSpeed.floatValue = _maxRotationSpeed;		
			rotationSpeedMultiplier.floatValue = EditorGUILayout.FloatField( "Rotation Speed Multiplier", rotationSpeedMultiplier.floatValue);
			
			EditorGUILayout.LabelField("Asteroid Drift Speed (Min/Max Range)", EditorStyles.boldLabel);
			_minDriftSpeed = minAsteroidDriftSpeed.floatValue;
			_maxDriftSpeed = maxAsteroidDriftSpeed.floatValue;
			GUIContent _driftContent = new GUIContent( string.Format( "Min:{0:F1}, Max:{1:F1}", _minDriftSpeed , _maxDriftSpeed ) );
			EditorGUILayout.MinMaxSlider(_driftContent, ref _minDriftSpeed, ref _maxDriftSpeed, 0.0f, 1.0f);
			minAsteroidDriftSpeed.floatValue = _minDriftSpeed;
			maxAsteroidDriftSpeed.floatValue = _maxDriftSpeed;
			driftSpeedMultiplier.floatValue = EditorGUILayout.FloatField( "Drift Speed Multiplier", driftSpeedMultiplier.floatValue);
		}		
		EditorGUILayout.Separator();
		
		// Visual Settings
		EditorGUILayout.LabelField("Visual Settings", EditorStyles.boldLabel);
		fadeAsteroids.boolValue = EditorGUILayout.Toggle("Fade Asteroids", fadeAsteroids.boolValue);		
		if (fadeAsteroids.boolValue) {
			distanceFade.floatValue = EditorGUILayout.Slider("Fade from % of Spawn", distanceFade.floatValue, 0.0f, 1.0f);
			EditorGUILayout.LabelField("Alpha is 1.0 at distanceFade*distanceSpawn*range and " +
			"gradually fades out to 0.0 at distanceSpawn*range.", EditorStyles.wordWrappedMiniLabel);			
		}				
		EditorGUILayout.Separator();
		
		// Asteroid Mesh Quality
		EditorGUILayout.LabelField("Asteroid Mesh Quality", EditorStyles.boldLabel);
		EditorGUILayout.PropertyField( polyCount);
		EditorGUILayout.PropertyField( polyCountCollider);
		if (polyCountCollider.enumValueIndex != (int) SU_Asteroid.PolyCount.LOW) {
			EditorGUILayout.LabelField("Warning! Using detailed collider meshes may heavily impact performance or raise errors if the mesh is too detailed.", EditorStyles.wordWrappedMiniLabel);	
		}
		
		// Asteroid Prefab (array of asteroid shapes the asteroid field should randomly consist of) 
		EditorGUILayout.LabelField("Asteroid Prefabs", EditorStyles.boldLabel);		
		_showPrefabs = EditorGUILayout.Foldout(_showPrefabs, "Prefabs");
		if (_showPrefabs) {
			ArrayGUI(myTarget, "prefabAsteroids");				
		}		
		EditorGUILayout.Separator();
		
		// Asteroid Materials (array of asteroid materials the asteroid field should randomly consist of)
		// The random selection is weighted between common and rare materials.
		EditorGUILayout.LabelField("Asteroid Materials", EditorStyles.boldLabel);
		_showMaterials = EditorGUILayout.Foldout(_showMaterials, "Materials");
		if (_showMaterials) {
			EditorGUILayout.LabelField("Very Common Materials (50%)", EditorStyles.boldLabel);
			ArrayGUI(myTarget, "materialVeryCommon");				
			EditorGUILayout.LabelField("Common Materials (30%)", EditorStyles.boldLabel);
			ArrayGUI(myTarget, "materialCommon");				
			EditorGUILayout.LabelField("Rare Materials (15%)", EditorStyles.boldLabel);
			ArrayGUI(myTarget, "materialRare");
			EditorGUILayout.LabelField("Very Rare Materials (5%)", EditorStyles.boldLabel);
			ArrayGUI(myTarget, "materialVeryRare");			
		}
		
		// Apply the modified properties
		myTarget.ApplyModifiedProperties();
	}
	
	
	// Function to ovveride and display custom object array in inspector
	void ArrayGUI(SerializedObject obj, string name) {	
		int size = obj.FindProperty(name + ".Array.size").intValue;
		int newSize = EditorGUILayout.IntField("Size", size);
		if (newSize != size) obj.FindProperty(name + ".Array.size").intValue = newSize;
		EditorGUI.indentLevel = 3;
	    for (int i=0;i<newSize;i++) {
	    	var prop = obj.FindProperty(string.Format("{0}.Array.data[{1}]", name, i));
			EditorGUILayout.PropertyField(prop);	
		}
		EditorGUI.indentLevel = 0;
    }	
}