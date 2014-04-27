// Space Scene Construction Kit Editor C# Script (version: 1.02)
// SPACE UNITY - Space Scene Construction Kit
// http://www.spaceunity.com
// (c) 2013 Stefan Persson
 
// Thank you for purchasing SPACE UNITY - Space Scene Construction kit.
 
// IMPORTANT: This script must (remain to) reside in the project folder SpaceUnity/Editor in order to work.
 
// This script creates a new menu option under Window | Space Scene Construction Kit which enables you
// to quickly create unique random space scenes for your game(s) and/or projects. 

// You shouldn't need to make any changes to this script or its parameters so inline documentation is limited.

// Version History
// 1.02 - Prefixed with SU_SpaceSceneConstructionKitEditor to avoid naming conflicts.
// 0.8 - Initial Release.

using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SU_SpaceSceneConstructionKitEditor : EditorWindow {
	private const string _version = "1.02";
	
	// Base path of assets where this script will search for space assets (materials, prefabs, etc.)
	private const string _basePath = "Assets/SpaceUnity/";

	// Name of the space scene prefab created by this script
	private const string _nameOfSpacePrefab = "SpaceScene";
	// MATERIAL PATHS
	// Space Unity iterates through all materials in the paths below and indexes the content based on labels
	private const string _pathStars = "Materials/Stars/";
	private const string _pathNebulas = "Materials/Nebulas/";
	private const string _pathGalaxies = "Materials/Galaxies/";
	private const string _pathPlanets = "Materials/Planets/";	
	private const string _pathRings = "Materials/Rings/";
	private const string _pathMoons = "Materials/Moons/";
	private const string _pathAtmospheres = "Materials/Atmospheres/";
	private const string _pathLocalStarsBody = "Materials/LocalStars/Body/";
	private const string _pathLocalStarsProminence = "Materials/LocalStars/Prominence/";
	private const string _pathLocalStarsFlare = "Materials/LocalStars/Flare/";
	
	// LABEL PREFIXES 
	// Space Unity uses these prefix to index material assets by color, complexity, brightness, etc.
	private const string _labelPrefixColor = "color-";
	private const string _labelPrefixComplexity = "complexity-";
	private const string _labelPrefixBrightness = "brightness-";
	private const string _labelPrefixClimate = "climate-";
	private const string _labelPrefixAtmosphere = "atmosphere-";
	private const string _labelPrefixStyle = "style-";
	private const string _labelPrefixSize = "size-";
	
	// TAGS
	private const string _tagNebula = "SpaceScene_Nebula";
	private const string _tagGalaxy = "SpaceScene_Galaxy";
	private const string _tagPlanet = "SpaceScene_Planet";
	private const string _tagLocalStar = "SpaceScene_LocalStar";
	private const string _tagCamera = "SpaceScene_Camera";
	
	// PREFAB PATHS + NAMES	
	private const string _prefabSpaceCamera = "Prefabs/SpaceSceneElements/SpaceCamera.prefab";
	private const string _prefabNebula = "Prefabs/SpaceSceneElements/Nebula.prefab";
	private const string _prefabGalaxy = "Prefabs/SpaceSceneElements/Galaxy.prefab";	
	private const string _prefabLocalStarSmall = "Prefabs/SpaceSceneElements/LocalStarSmall.prefab";
	private const string _prefabLocalStarMedium = "Prefabs/SpaceSceneElements/LocalStarMedium.prefab";
	private const string _prefabLocalStarLarge = "Prefabs/SpaceSceneElements/LocalStarLarge.prefab";
	// Planet and moon Prefabs are built with a prefix and a suffix as the mesh quality is reflected in the name, e.g. "PlanetHighPoly.prefab"
	private const string _prefabPlanetPrefix = "Prefabs/SpaceSceneElements/Planet";
	private const string _prefabPlanetSuffix = "Poly.prefab";
	private const string _prefabPlanetRingsPrefix = "Prefabs/SpaceSceneElements/PlanetRings";
	private const string _prefabPlanetRingsSuffix = "Poly.prefab";
	private const string _prefabPlanetMoonPrefix = "Prefabs/SpaceSceneElements/Moon";
	private const string _prefabPlanetMoonSuffix = "Poly.prefab";
	private const string _prefabPlanetAtmosphere = "Prefabs/SpaceSceneElements/PlanetAtmosphere.prefab";
	
	
	// PLANET DISTANCES
	private const float _distancePlanetVeryClose = 10.0f;
	private const float _distancePlanetClose = 30.0f;
	private const float _distancePlanetDistant = 80.0f;
	private const float _distancePlanetVeryDistant = 400.0f;
	private const float _distanceMinRangeMultiplier = 0.7f;
	private const float _distanceMaxRangeMultiplier = 1.3f;
	
	// PLANET ROTATION SPEED
	private const float _speedPlanetRotationSlow = 0.1f;
	private const float _speedPlanetRotationMedium = 0.5f;
	private const float _speedPlanetRotationFast = 1.0f;	
	
	// PLANET RING ODDS
	private const float _oddsPlanetRingsVeryRare = 0.05f;
	private const float _oddsPlanetRingsRare = 0.2f;
	private const float _oddsPlanetRingsCoinFlip = 0.5f;
	private const float _oddsPlanetRingsCommon = 0.8f;
	private const float _oddsPlanetRingsVeryCommon = 0.95f;

	// MOON SCALE
	private const float _scaleMoonMinRangeMultiplier = 0.1f;
	private const float _scaleMoonMaxRangeMultiplier = 0.3f;
	
	// MOON ORBIT SPEEDS
	private const float _speedMoonOrbitSlow = 0.01f;
	private const float _speedMoonOrbitMedium = 0.05f;
	private const float _speedMoonOrbitFast = 0.1f;	

	// MOON DISTANCES
	private const float _distanceMoonVeryClose = 1.0f;
	private const float _distanceMoonClose = 1.5f;
	private const float _distanceMoonDistant = 3.0f;
	private const float _distanceMoonVeryDistant = 5.0f;
	
	// GALAXY DISTANCES / SCALES
	private const float _scaleGalaxyVeryClose = 1.0f;
	private const float _scaleGalaxyClose = 0.6f;
	private const float _scaleGalaxyDistant = 0.3f;
	private const float _scaleGalaxyVeryDistant = 0.1f;
	
	// LOCAL STAR LIGHT INTENSITIES AND LIGHT COLORS
	private const float _intensityLocalStarVeryLow = 0.1f;
	private const float _intensityLocalStarLow = 0.3f;
	private const float _intensityLocalStarMedium = 0.5f;
	private const float _intensityLocalStarHigh = 0.8f;
	private const float _intensityLocalStarVeryHigh = 1.2f;
	private Color _colorLocalStarBlue = new Color(0.7f, 0.7f, 1.0f, 1.0f);
	private Color _colorLocalStarYellow = new Color(1.0f, 1.0f, 0.7f, 1.0f);
	private Color _colorLocalStarOrange = new Color(1.0f, 0.9f, 0.7f, 1.0f);
	private Color _colorLocalStarRed = new Color(1.0f, 0.7f, 0.7f, 1.0f);
	
	
	// Enums used for menu options
	private enum StarCount { RANDOM = 0, LOW = 1, MEDIUM = 2, HIGH = 4 }	
	private enum StarBackground { RANDOM = 0, BLACK = 1, BLUE = 2, ORANGE = 4, GREEN = 8, RED = 16, PURPLE = 32, GRAY = 64, CYAN = 128 }	
	
	private enum NebulaBrightness { VERY_DARK = 1, DARK = 2, MEDIUM = 4, BRIGHT = 8, VERY_BRIGHT = 16}
	private enum NebulaColors { BLUE = 1, PINK = 2, PURPLE = 4, GREEN = 8, YELLOW = 16, ORANGE = 32, RED = 64 }
	private enum NebulaStyles { CLOUDY = 1, STREAKY = 2, GLITTERY = 4, DARK_MATTER = 8 }
	private enum NebulaComplexity { LOW = 1, MEDIUM = 2, HIGH = 4 }
	private enum NebulaTextureCount { VERY_LOW = 2, LOW = 4, MEDIUM = 8, HIGH = 16, VERY_HIGH = 32 }

	private enum GalaxyCount { RANDOM = 0, NONE = 1, ONE = 2, TWO = 4, THREE = 8 }
	private enum GalaxyColors { WHITE = 1, BLUE = 2, YELLOW = 4, PURPLE = 8, GREEN = 16, ORANGE = 32 }
	private enum GalaxyDistance { VERY_CLOSE = 1, CLOSE = 2, DISTANT = 4, VERY_DISTANT = 8 }
	private enum GalaxyIsLightsource { YES = 0, NO = 1 }
	private enum GalaxyLightHasFlare { YES = 0, NO = 1 }
	
	private enum PlanetMeshDetail { LOW = 0, MEDIUM = 1, HIGH = 2 }
	private enum PlanetCount { RANDOM = -1, NONE = 0, ONE = 1 , TWO = 2, THREE = 3, FOUR = 4, FIVE = 5}
	private enum PlanetDistance { VERY_CLOSE = 1, CLOSE = 2, DISTANT = 4, VERY_DISTANT = 8 }
	private enum PlanetClimate { EARTH_LIKE = 1, ICE = 2, DESERT = 4, GAS = 8, MOLTEN = 16, ALIEN = 32 }	
	private enum PlanetRotation { NONE = 1, SLOW = 2, MEDIUM = 4, FAST = 8 }
	private enum PlanetMoons { NONE = 1, ONE = 2, TWO = 4 }
	private enum PlanetMoonDistance { VERY_CLOSE = 1, CLOSE = 2, DISTANT = 4, VERY_DISTANT = 8 }
	private enum PlanetMoonOrbitSpeed { STATIONARY = 1, SLOW = 2, MEDIUM = 4, FAST = 8 }	
	private enum PlanetRings { NEVER = 1, VERY_RARE = 2, RARE = 3, COIN_FLIP = 4, COMMON = 8, VERY_COMMON = 16, ALWAYS = 32 }
	
	private enum LocalStarColor { RANDOM = 0, YELLOW = 1, RED = 2, ORANGE = 4, BLUE = 8 }	
	private enum LocalStarSize { RANDOM = 0, SMALL = 1, MEDIUM = 2, LARGE = 4 }
	private enum LocalStarIsLightsource { YES = 0, NO = 1 }
	private enum LocalStarLightHasFlare { YES = 0, NO = 1 }	
	private enum LocalStarLightIntensity { RANDOM = 0, VERY_LOW = 1, LOW = 2, MEDIUM = 4, HIGH = 8, VERY_HIGH = 16 }
	private enum LocalStarLightColor { WHITE = 0, STAR_COLOR = 1 }
	
	// Current sroll position of the Editor Window
	private Vector2 _scrollPos = new Vector2(0,0);
	
	// Variables for star backgrounds, these are SkyBox textures that can be seamlessly tiled
	private bool _starsEnabled = true;	
	private StarCount _starCount = StarCount.RANDOM;
	private static Hashtable _hStarCount = new Hashtable();
	private StarBackground _starBackground = StarBackground.RANDOM;
	private static Hashtable _hStarBackgrounds = new Hashtable();
	
	// Variables for nebulas
	private bool _nebulasEnabled = true;	
	private int _nebulaCountMax = 32;
	private int _nebulaCountMin = 0;
	private int _nebulaCount = 16;
	private NebulaBrightness _nebulaBrightness = (NebulaBrightness) (-1);
	private NebulaColors _nebulaColors = (NebulaColors) (-1);
	private NebulaStyles _nebulaStyles = (NebulaStyles) (-1);
	private float _nebulaComplexity = 0.5f;
	private NebulaTextureCount _nebulaTextureCount = NebulaTextureCount.MEDIUM;	
	private static Hashtable _hNebulaBrightness = new Hashtable();
	private static Hashtable _hNebulaColors = new Hashtable();
	private static Hashtable _hNebulaStyles = new Hashtable();
	private static Hashtable _hNebulaComplexity = new Hashtable();
	
	// Variables for galaxies
	private bool _galaxiesEnabled = true;
	private GalaxyCount _galaxyCount = GalaxyCount.RANDOM;
	private GalaxyColors _galaxyColors = (GalaxyColors) (-1);	
	private GalaxyDistance _galaxyDistance = (GalaxyDistance) (-1);
	private bool _galaxyIsLightsource = false;
	private bool _galaxyLightHasFlare = false;	
	private static Hashtable _hGalaxyColors = new Hashtable();	
	
	// Variables for planets
	private bool _planetsEnabled = true;
	private PlanetMeshDetail _planetMeshDetail = PlanetMeshDetail.HIGH;
	private PlanetCount _planetCount = PlanetCount.ONE;
	private PlanetClimate _planetClimate = (PlanetClimate) (-1);		
	private bool _planetAtmosphere = true;
	private PlanetDistance _planetDistance = (PlanetDistance) ((int) PlanetDistance.VERY_CLOSE + (int) PlanetDistance.CLOSE  + (int) PlanetDistance.DISTANT);
	private PlanetRotation _planetRotation = (PlanetRotation) ((int) PlanetRotation.NONE + (int) PlanetRotation.SLOW);
	private PlanetMoons _planetMoons = (PlanetMoons) ((int) PlanetMoons.NONE + (int) PlanetMoons.ONE);
	private PlanetMoonDistance _planetMoonDistance = (PlanetMoonDistance) (-1);
	private PlanetMoonOrbitSpeed _planetMoonOrbitSpeed = (PlanetMoonOrbitSpeed) ((int) PlanetMoonOrbitSpeed.STATIONARY + (int) PlanetMoonOrbitSpeed.SLOW);
	private PlanetRings _planetRings= PlanetRings.RARE;	
	private static Hashtable _hPlanetClimates = new Hashtable();
	private static Hashtable _hPlanetAtmospheres = new Hashtable();
	private static List<string> _lPlanetRings = new List<string>();
	private static List<string> _lPlanetMoons = new List<string>();
	
	// Variables for local star
	private bool _localStarEnabled = true;
	private LocalStarColor _localStarColor = LocalStarColor.RANDOM;
	private LocalStarSize _localStarSize = LocalStarSize.RANDOM;
	private bool _localStarIsLightsource = true;
	private LocalStarLightIntensity _localStarLightIntensity = LocalStarLightIntensity.MEDIUM;
	private bool _localStarLightHasFlare = true;
	private LocalStarLightColor _localStarLightColor = LocalStarLightColor.WHITE;
	private static Hashtable _hLocalStarBody = new Hashtable();
	private static Hashtable _hLocalStarProminence = new Hashtable();
	private static Hashtable _hLocalStarFlareColor = new Hashtable();
	private static Hashtable _hLocalStarFlareSize = new Hashtable();
			
	// Create the menu item in the Unity Editor
    [MenuItem ("Window/Space Scene Construction Kit")]	
	static void Init () {		
		// Initialize the Space Scene Construction Kit editor window	
		EditorWindow.GetWindow (typeof (SU_SpaceSceneConstructionKitEditor));
    }	  
	
	void OnEnable() {		
		// Clear hashtables to refresh space object labels
		_hStarBackgrounds.Clear();
		_hStarCount.Clear();
		_hNebulaBrightness.Clear();
		_hNebulaColors.Clear();
		_hNebulaStyles.Clear();
		_hNebulaComplexity.Clear();
		_hGalaxyColors.Clear();
		_hPlanetClimates.Clear();	
		_hPlanetAtmospheres.Clear();
		_lPlanetRings.Clear();
		_hLocalStarBody.Clear();
		_hLocalStarProminence.Clear();		
		_hLocalStarFlareColor.Clear();
		_hLocalStarFlareSize.Clear();
		
		// Automatically populate hashtables with labels from the project hierarchy
		// This is performed every time the editor is enabled supporting future addons as long as the assets contain labels.
		PopulateHashtable<Material>(_hStarBackgrounds, _basePath + _pathStars, _labelPrefixColor, typeof(StarBackground));
		PopulateHashtable<Material>(_hStarCount, _basePath + _pathStars, "count-", typeof(StarCount));
		PopulateHashtable<Material>(_hNebulaBrightness, _basePath + _pathNebulas, _labelPrefixBrightness, typeof(NebulaBrightness));
		PopulateHashtable<Material>(_hNebulaColors, _basePath + _pathNebulas, _labelPrefixColor, typeof(NebulaColors));
		PopulateHashtable<Material>(_hNebulaStyles, _basePath + _pathNebulas, _labelPrefixStyle, typeof(NebulaStyles));
		PopulateHashtable<Material>(_hNebulaComplexity, _basePath + _pathNebulas, _labelPrefixComplexity, typeof(NebulaComplexity));
		PopulateHashtable<Material>(_hGalaxyColors, _basePath + _pathGalaxies, _labelPrefixColor, typeof(GalaxyColors));
		PopulateHashtable<Material>(_hPlanetClimates, _basePath + _pathPlanets, _labelPrefixClimate, typeof(PlanetClimate));
		PopulateAtmospheres<Material>(_hPlanetAtmospheres, _basePath + _pathAtmospheres);
		PopulateList<Material>(_lPlanetRings, _basePath + _pathRings);
		PopulateList<Material>(_lPlanetMoons, _basePath + _pathMoons);
		PopulateHashtable<Material>(_hLocalStarBody, _basePath + _pathLocalStarsBody, _labelPrefixColor, typeof(LocalStarColor));
		PopulateHashtable<Material>(_hLocalStarProminence, _basePath + _pathLocalStarsProminence, _labelPrefixColor, typeof(LocalStarColor));
		PopulateHashtable<Flare>(_hLocalStarFlareColor, _basePath + _pathLocalStarsFlare, _labelPrefixColor, typeof(LocalStarColor));
		PopulateHashtable<Flare>(_hLocalStarFlareSize, _basePath + _pathLocalStarsFlare, _labelPrefixSize, typeof(LocalStarSize));				
	}
	
    void OnGUI () {
        GUILayout.Label ("SPACE SCENE CONSTRUCTION KIT (version: " + _version + ")", EditorStyles.boldLabel);
		if (GUILayout.Button ("Create Space Scene Prefab")) {
			// Create the Space Scene Prefab
			DoCreateSpaceScenePrefab();
		}
					
		_scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
		
		// Stars
		_starsEnabled = EditorGUILayout.BeginToggleGroup ("STARS", _starsEnabled);
		_starCount = (StarCount)EditorGUILayout.EnumPopup("Star Count: ", _starCount);
		_starBackground = (StarBackground)EditorGUILayout.EnumPopup("Star Background: ", _starBackground);
		EditorGUILayout.EndToggleGroup ();
		
		// Nebulas
		_nebulasEnabled = EditorGUILayout.BeginToggleGroup ("NEBULAS", _nebulasEnabled);
		_nebulaCount = EditorGUILayout.IntSlider ("Nebula Count", _nebulaCount, _nebulaCountMin, _nebulaCountMax);	
		_nebulaBrightness = (NebulaBrightness) EditorGUILayout.EnumMaskField ("Nebula Brightness", _nebulaBrightness);
		_nebulaColors = (NebulaColors) EditorGUILayout.EnumMaskField ("Nebula Colors", _nebulaColors);		
		_nebulaStyles = (NebulaStyles) EditorGUILayout.EnumMaskField ("Nebula Styles", _nebulaStyles);
		_nebulaComplexity = EditorGUILayout.Slider ("Nebula Complexity", _nebulaComplexity, 0.0f, 1.0f);	
		GUILayout.Space(10);
		_nebulaTextureCount = (NebulaTextureCount) EditorGUILayout.EnumPopup ("Nebula Texture Count", _nebulaTextureCount);
		EditorGUILayout.LabelField ("(Note: Texture count impacts memory use and distribution size)");
		EditorGUILayout.EndToggleGroup ();
		
		// Galaxies
		_galaxiesEnabled = EditorGUILayout.BeginToggleGroup ("GALAXIES", _galaxiesEnabled);
		_galaxyCount = (GalaxyCount) EditorGUILayout.EnumPopup("Galaxy Count:", _galaxyCount);
		_galaxyColors = (GalaxyColors) EditorGUILayout.EnumMaskField ("Galaxy Colors", _galaxyColors);		
		_galaxyDistance = (GalaxyDistance) EditorGUILayout.EnumMaskField("Galaxy Distance:", _galaxyDistance);
		_galaxyIsLightsource = EditorGUILayout.Toggle("Is Light Source:", _galaxyIsLightsource);
		_galaxyLightHasFlare = EditorGUILayout.Toggle("Light Source has Flare:", _galaxyLightHasFlare);
		EditorGUILayout.EndToggleGroup ();  
		
		// Planets
		_planetsEnabled = EditorGUILayout.BeginToggleGroup ("PLANETS", _planetsEnabled);
		_planetMeshDetail = (PlanetMeshDetail) EditorGUILayout.EnumPopup("Planet Mesh Detail: ", _planetMeshDetail);
		_planetCount = (PlanetCount) EditorGUILayout.EnumPopup("Planet Count:", _planetCount);
		_planetClimate = (PlanetClimate) EditorGUILayout.EnumMaskField("Planet Climate:", _planetClimate);
		_planetAtmosphere = EditorGUILayout.Toggle("Planet Atmosphere", _planetAtmosphere);
		_planetDistance = (PlanetDistance) EditorGUILayout.EnumMaskField("Planet Distance:", _planetDistance);
		_planetRotation = (PlanetRotation) EditorGUILayout.EnumMaskField("Planet Rotation:", _planetRotation);
		_planetRings = (PlanetRings) EditorGUILayout.EnumPopup("Planet Rings:", _planetRings);
		_planetMoons = (PlanetMoons) EditorGUILayout.EnumMaskField("Moons:", _planetMoons);
		_planetMoonDistance = (PlanetMoonDistance) EditorGUILayout.EnumMaskField("Moon Distance:", _planetMoonDistance);
		_planetMoonOrbitSpeed = (PlanetMoonOrbitSpeed) EditorGUILayout.EnumMaskField("Moon Orbit Speed:", _planetMoonOrbitSpeed);
		EditorGUILayout.EndToggleGroup (); 		
		
		// Local Star
		_localStarEnabled = EditorGUILayout.BeginToggleGroup ("LOCAL STAR", _localStarEnabled);
		_localStarColor = (LocalStarColor) EditorGUILayout.EnumPopup("Local Star Color:", _localStarColor);
		_localStarSize = (LocalStarSize) EditorGUILayout.EnumPopup("Local Star Size: ", _localStarSize);
		_localStarIsLightsource = EditorGUILayout.Toggle("Is Light Source:", _localStarIsLightsource);
		_localStarLightIntensity = (LocalStarLightIntensity) EditorGUILayout.EnumPopup("Light Intensity: ", _localStarLightIntensity);
		_localStarLightColor = (LocalStarLightColor) EditorGUILayout.EnumPopup("Light Color: ", _localStarLightColor);
		_localStarLightHasFlare = EditorGUILayout.Toggle("Light Source Has Flare:", _localStarLightHasFlare);
		EditorGUILayout.EndToggleGroup (); 		
		
		EditorGUILayout.EndScrollView(); 		
		
    }	
	
	public void DoCreateSpaceScenePrefab() { DoCreateSpaceScenePrefab(false); }
	public void DoCreateSpaceScenePrefab(bool _overwrite) {	
		string _planetMeshDetailString = "High";
		float _planetRingsOdds = 0.0f;
		
		// --- STARS ---
		// Generate a list of matching stars based on filters
		List<string> _starsMatch = new List<string>();
		
		if (_starsEnabled) {
			if (_starBackground == StarBackground.RANDOM) {
				// No star background color specified - add all stars to the list
				foreach (DictionaryEntry _entry in _hStarBackgrounds) _starsMatch.Add(_entry.Key.ToString());
			} else {
				// Star background color filter enabled, evaluate which stars match
				foreach (DictionaryEntry _entry in _hStarBackgrounds) {
					if (((int) _entry.Value & (int) _starBackground) != 0) {
						// The bit mask says this color is a match, add it
						_starsMatch.Add(_entry.Key.ToString());
					}
				}
			}					
			if (_starCount != StarCount.RANDOM) {
				// Iterate through stars and remove if count is not included in the filter
				foreach (DictionaryEntry _entry in _hStarCount) {
					if (((int) _entry.Value & (int) _starCount) == 0) {
						// This star count is not in filter, remove from list
						_starsMatch.Remove(_entry.Key.ToString());
					}
				}
			}
		}	
			
		// --- NEBULAS ---
		
		// Generate list of matching nebulas based on filters
		List<string> _nebulasMatch = new List<string>();		
		
		if (_nebulasEnabled) {
			if ((int) _nebulaColors == -1) {
				// Nebula colors are set to everything, add them all
				foreach (DictionaryEntry _entry in _hNebulaColors) _nebulasMatch.Add(_entry.Key.ToString());
			} else {
				// Nebula color filter is enabled - evaluate which nebulas match
				foreach (DictionaryEntry _entry in _hNebulaColors) {				
					if (((int) _entry.Value & (int) _nebulaColors) != 0) {					
						// one or more bits in the mask match - evaluate further
						bool addNebula = true;
						// iterate through all possible nebula colors (this is because there may also be unwanted colors in the nebula)										
						foreach(NebulaColors _n in Enum.GetValues(typeof(NebulaColors))) {						
							if (((int) _n & (int) _nebulaColors) == 0) {							
								// this color is not selected in the filter
								if (((int) _n & (int) _entry.Value) != 0) {
									// this nebula has a color that is not selected in the filter, don't add
									addNebula = false;
								}
							}					
						}
						if (addNebula) {
							_nebulasMatch.Add(_entry.Key.ToString());
						}
					}
				}
			}
				
			if ((int) _nebulaStyles != -1) {
				foreach (DictionaryEntry _entry in _hNebulaStyles) {				
					if (_nebulasMatch.Contains(_entry.Key.ToString())) {
						if (((int) _entry.Value & (int) _nebulaStyles) == 0) {
							// The nebula that was matched by color is not of the correct style, remove from list
							_nebulasMatch.Remove(_entry.Key.ToString());
						}			
					}
				}
			}
			
			if ((int) _nebulaBrightness != -1) {
				foreach (DictionaryEntry _entry in _hNebulaBrightness) {
					if (_nebulasMatch.Contains(_entry.Key.ToString())) {
						if (((int) _entry.Value & (int) _nebulaBrightness) == 0) {
							// The nebula brightness was not matched, remove from list
							_nebulasMatch.Remove(_entry.Key.ToString());
						}
					}
				}
			}
			
			// Adjust for nebula complexity
			foreach (DictionaryEntry _entry in _hNebulaComplexity) {				
				if (_nebulasMatch.Contains(_entry.Key.ToString())) {					
					// nebula is in the current matched list
					if (_nebulaComplexity <= 0.45f && (int) _entry.Value == (int) NebulaComplexity.HIGH) _nebulasMatch.Remove(_entry.Key.ToString());						
					if (_nebulaComplexity <= 0.25f && (int) _entry.Value == (int) NebulaComplexity.MEDIUM) _nebulasMatch.Remove(_entry.Key.ToString());					
					if (_nebulaComplexity >= 0.55f && (int) _entry.Value == (int) NebulaComplexity.LOW) _nebulasMatch.Remove(_entry.Key.ToString());
					if (_nebulaComplexity >= 0.75f && (int) _entry.Value == (int) NebulaComplexity.MEDIUM) _nebulasMatch.Remove(_entry.Key.ToString());
				}
				
			}
			
			if (_nebulasMatch.Count == 0) {			
				EditorUtility.DisplayDialog("Oups!", "No nebulas match your filter criterea. Please revise settings and try again.", "Ok - I'll do that!");
				return;
			}
					
			// Nebula Texture Count	
			_nebulasMatch = LimitedRandomList(_nebulasMatch, (int) _nebulaTextureCount);
		}		
		
		// --- GALAXIES ---				
		int _numberOfGalaxiesToAdd = 0;
		
		// Generate list of matching galaxies based on filters								
		List<string> _galaxiesMatch;		
		_galaxiesMatch = new List<string>();
		// Generate a list of matching galaxy distance scales
		List<float> _galaxyDistanceScales = new List<float>();
		string _galaxyName = "";
		
		if (_galaxyCount != GalaxyCount.NONE && _galaxiesEnabled) {
			switch (_galaxyCount) {
			case GalaxyCount.RANDOM:
				_numberOfGalaxiesToAdd = UnityEngine.Random.Range(0, 5);
				break;
			case GalaxyCount.ONE:
				_numberOfGalaxiesToAdd = 1;
				break;
			case GalaxyCount.TWO:
				_numberOfGalaxiesToAdd = 2;
				break;
			case GalaxyCount.THREE:
				_numberOfGalaxiesToAdd = 3;
				break;
			}
			if (_numberOfGalaxiesToAdd > 0) {			
				if ((int) _galaxyColors == -1) {
					// Galaxy colors are set to everything, add them all
					foreach (DictionaryEntry _entry in _hGalaxyColors) _galaxiesMatch.Add(_entry.Key.ToString());
				} else {
					// Galaxy color filter is enabled - evaluate which nebulas match
					foreach (DictionaryEntry _entry in _hGalaxyColors) {
						if (((int) _entry.Value & (int) _galaxyColors) != 0) {				
							// one or more bits in the mask match - evaluate further				
							bool addGalaxy = true;
							// iterate through all possible galaxy colors (this is because there may also be unwanted colors in the galaxy)
							foreach(GalaxyColors _n in Enum.GetValues(typeof(GalaxyColors))) {						
								if (((int) _n & (int) _galaxyColors) == 0) {
									// this color is not selected in the filter
									if (((int) _n & (int) _entry.Value) != 0) {
										// this galaxy has a color that is not selected in the filter, don't add
										addGalaxy = false;
									}
								}					
							}
							if (addGalaxy) {
								_galaxiesMatch.Add(_entry.Key.ToString());
							}
						}
					}
				}
				if (_galaxiesMatch.Count == 0) {			
					EditorUtility.DisplayDialog("Oups!", "No galaxies match your filter criterea. Please revise settings and try again.", "Ok - I'll do that!");
					return;			
				}
				if (_galaxyDistance == 0) {			
					EditorUtility.DisplayDialog("Oups!", "You must specify at least one galaxy distance in filter. Please revise settings and try again.", "Ok - I'll do that!");
					return;	
				}
								
				// Populate the list of galaxy distance scale - also used for light intensity
				foreach(GalaxyDistance g in Enum.GetValues(typeof(GalaxyDistance))) {
					if (((int) g & (int) _galaxyDistance) != 0) {
						switch (g) {
						case GalaxyDistance.VERY_CLOSE:
							_galaxyDistanceScales.Add(_scaleGalaxyVeryClose);
							break;
						case GalaxyDistance.CLOSE:							
							_galaxyDistanceScales.Add(_scaleGalaxyClose);
							break;
						case GalaxyDistance.DISTANT:							
							_galaxyDistanceScales.Add(_scaleGalaxyDistant);
							break;
						case GalaxyDistance.VERY_DISTANT:
							_galaxyDistanceScales.Add(_scaleGalaxyVeryDistant);
							break;
						}
					}
				}
			}					
		}				
		
		// --- PLANETS ---
		int _numberOfPlanetsToAdd = 0;
		// Generate list of matching planets based on filters	
		List<string> _planetsMatch = new List<string>();
		// Generate lists for planet and moon distance and rotation  scales
		List<PlanetDistance> _planetDistanceScales = new List<PlanetDistance>();
		List<float> _planetRotationScales = new List<float>();
		List<int> _numberOfMoonsForPlanet = new List<int>();
		List<float> _planetMoonRotationScales = new List<float>();
		List<float> _planetMoonDistanceScales = new List<float>();
		
		if (_planetCount != PlanetCount.NONE && _planetsEnabled) {
			// Set the number of planets to add
			if (_planetCount == PlanetCount.RANDOM) {
				_numberOfPlanetsToAdd = UnityEngine.Random.Range(0,6);
			} else {
				_numberOfPlanetsToAdd = (int) _planetCount;
			}
			
			if (_numberOfPlanetsToAdd > 0) {	
				if ((int) _planetClimate == -1) {
					// Filter set to all climates, add all planets to the matching list
					foreach (DictionaryEntry _entry in _hPlanetClimates) _planetsMatch.Add(_entry.Key.ToString());
				} else {
					// Planet climate filter is enabled - evaluate which planets match
					foreach (DictionaryEntry _entry in _hPlanetClimates) {
						if (((int) _entry.Value & (int) _planetClimate) != 0) {				
							// one or more bits in the mask match - evaluate further				
							bool addPlanet = true;
							// iterate through all possible planet climates (this is because there may also be unwanted climate on the planet)
							foreach(PlanetClimate p in Enum.GetValues(typeof(PlanetClimate))) {						
								if (((int) p & (int) _planetClimate) == 0) {							
									// this climate is not selected in the filter
									if (((int) p & (int) _entry.Value) != 0) {
										// this planet has a climate that is not selected in the filter, don't add
										addPlanet = false;
									}
								}					
							}
							if (addPlanet) {
								_planetsMatch.Add(_entry.Key.ToString());
							}
						}
					}					
				}
			}
			if (_planetsMatch.Count == 0) {			
				EditorUtility.DisplayDialog("Oups!", "No planets match your filter criterea. Please revise settings and try again.", "Ok - I'll do that!");
				return;			
			}
			if (_planetDistance == 0) {			
				EditorUtility.DisplayDialog("Oups!", "You must specify at least one planet distance in filter. Please revise settings and try again.", "Ok - I'll do that!");
				return;	
			}			
			if (_planetDistance == PlanetDistance.VERY_CLOSE && _numberOfPlanetsToAdd > 1) {
				EditorUtility.DisplayDialog("Oups!", "There can only be one planet at distance 'VERY_CLOSE'. Reduce number of planets or allow more alternatives in Planet Distance setting.", "Ok - I'll do that!");
				return;									
			}
			if (_planetDistance == PlanetDistance.CLOSE && _numberOfPlanetsToAdd > 1) {
				EditorUtility.DisplayDialog("Oups!", "There can only be one planet at distance 'CLOSE'. Reduce number of planets or allow more alternatives in Planet Distance setting.", "Ok - I'll do that!");
				return;									
			}
			if ((int) _planetDistance == ((int) PlanetDistance.CLOSE + (int) PlanetDistance.VERY_CLOSE) && _numberOfPlanetsToAdd > 2) {
				EditorUtility.DisplayDialog("Oups!", "There can only be one planet at distance 'CLOSE' and one at distance 'VERY_CLOSE'. Reduce number of planets or allow more alternatives in Planet Distance setting.", "Ok - I'll do that!");
				return;									
			}			
			
			
			// Populate the list of planet distance scale
			foreach(PlanetDistance _pD in Enum.GetValues(typeof(PlanetDistance))) {
				if (((int) _pD & (int) _planetDistance) != 0) {
					_planetDistanceScales.Add(_pD);
				}		
			}	
			
			// Populate the list of planet rotation scale
			foreach(PlanetRotation _pR in Enum.GetValues(typeof(PlanetRotation))) {
				if (((int) _pR & (int) _planetRotation) != 0) {
					switch (_pR) {
					case PlanetRotation.NONE:
						_planetRotationScales.Add(0.0f);
						break;
					case PlanetRotation.SLOW:
						_planetRotationScales.Add(_speedPlanetRotationSlow);
						break;
					case PlanetRotation.MEDIUM:							
						_planetRotationScales.Add(_speedPlanetRotationMedium);
						break;
					case PlanetRotation.FAST:
						_planetRotationScales.Add(_speedPlanetRotationFast);
						break;
					}
				}
			}
						
			
			// Populate the list for number of moons
			foreach (PlanetMoons _pM in Enum.GetValues(typeof(PlanetMoons))) {
				if (((int) _pM & (int) _planetMoons) != 0) {
					switch (_pM) {
					case PlanetMoons.NONE:
						_numberOfMoonsForPlanet.Add(0);
						break;
					case PlanetMoons.ONE:
						_numberOfMoonsForPlanet.Add(1);
						break;
					case PlanetMoons.TWO:
						_numberOfMoonsForPlanet.Add(2);
						break;						
					}			
				}
			}
			
			// Populate the list of moon rotation scale
			foreach(PlanetMoonOrbitSpeed _mR in Enum.GetValues(typeof(PlanetMoonOrbitSpeed))) {
				if (((int) _mR & (int) _planetMoonOrbitSpeed) != 0) {
					switch (_mR) {
					case PlanetMoonOrbitSpeed.STATIONARY:
						_planetMoonRotationScales.Add(0.0f);
						break;
					case PlanetMoonOrbitSpeed.SLOW:
						_planetMoonRotationScales.Add(_speedMoonOrbitSlow);
						break;
					case PlanetMoonOrbitSpeed.MEDIUM:							
						_planetMoonRotationScales.Add(_speedMoonOrbitMedium);
						break;
					case PlanetMoonOrbitSpeed.FAST:
						_planetMoonRotationScales.Add(_speedMoonOrbitFast);
						break;
					}
				}
			}
			
		
			
			// Populate the list of moon distance scale
			foreach(PlanetMoonDistance _mD in Enum.GetValues(typeof(PlanetMoonDistance))) {
				if (((int) _mD & (int) _planetMoonDistance) != 0) {
					switch (_mD) {
					case PlanetMoonDistance.VERY_CLOSE:
						_planetMoonDistanceScales.Add(_distanceMoonVeryClose);
						break;
					case PlanetMoonDistance.CLOSE:
						_planetMoonDistanceScales.Add(_distanceMoonClose);
						break;
					case PlanetMoonDistance.DISTANT:
						_planetMoonDistanceScales.Add(_distanceMoonDistant);
						break;
					case PlanetMoonDistance.VERY_DISTANT:
						_planetMoonDistanceScales.Add(_distanceMoonVeryDistant);
						break;
					}
				}
			}				
						
			// Set the planet and ring mesh detail level
			switch (_planetMeshDetail) {
			case PlanetMeshDetail.LOW:
				_planetMeshDetailString = "Low";
				break;
			case PlanetMeshDetail.MEDIUM:
				_planetMeshDetailString = "Medium";
				break;
			case PlanetMeshDetail.HIGH:
				_planetMeshDetailString = "High";
				break;
			}		
			
			// Set odds for planet rings
			switch (_planetRings) {
			case PlanetRings.VERY_RARE:
				_planetRingsOdds = _oddsPlanetRingsVeryRare;
				break;		
			case PlanetRings.RARE:
				_planetRingsOdds = _oddsPlanetRingsRare;
				break;
			case PlanetRings.COIN_FLIP:
				_planetRingsOdds = _oddsPlanetRingsCoinFlip;
				break;
			case PlanetRings.COMMON:
				_planetRingsOdds = _oddsPlanetRingsCommon;
				break;
			case PlanetRings.VERY_COMMON:
				_planetRingsOdds = _oddsPlanetRingsVeryCommon;
				break;
			}		
		}
				
		// --- LOCAL STAR ---
		List<string> _localStarBodyMatch = new List<string>();
		List<string> _localStarProminenceMatch = new List<string>();
		List<string> _localStarFlareMatch = new List<string>();
		LocalStarColor _localStarColorUsed = _localStarColor;
		LocalStarSize _localStarSizeUsed = _localStarSize;
		LocalStarLightIntensity _localStarLightIntensityUsed = _localStarLightIntensity;
		
		if (_localStarEnabled) {			
			if (_localStarColorUsed == LocalStarColor.RANDOM) {
				// Get a random local star color
				_localStarColorUsed = GetRandomEnum<LocalStarColor>(1);
			}

			if (_localStarSizeUsed == LocalStarSize.RANDOM) {
				// Get a random local star size
				_localStarSizeUsed = GetRandomEnum<LocalStarSize>(1);
			}

			if (_localStarLightIntensityUsed == LocalStarLightIntensity.RANDOM) {
				// Get a random local star size
				_localStarLightIntensityUsed = GetRandomEnum<LocalStarLightIntensity>(1);
			}
			
			foreach (DictionaryEntry _entry in _hLocalStarBody) {		
				if (((int) _entry.Value & (int) _localStarColorUsed)!= 0) {
					// The local star body has the correct color, add it to the matching list
					_localStarBodyMatch.Add(_entry.Key.ToString());
				}
			}
			
			foreach (DictionaryEntry _entry in _hLocalStarProminence) {		
				if (((int) _entry.Value & (int) _localStarColorUsed) != 0) {
					// The local star prominence has the correct color, add it to the matching list
					_localStarProminenceMatch.Add(_entry.Key.ToString());
				}
			}	

			foreach (DictionaryEntry _entry in _hLocalStarFlareColor) {	
				if (((int) _entry.Value & (int) _localStarColorUsed) != 0) {
					// The local star flare has the correct color, add it to the matching list
					_localStarFlareMatch.Add(_entry.Key.ToString());
				}
			}	
			
			foreach (DictionaryEntry _entry in _hLocalStarFlareSize) {		
				if (((int) _entry.Value & (int) _localStarSizeUsed) == 0) {
					// The local star flare has the incorrect color, remove it from the matching list if applicable					
					if (_localStarFlareMatch.Contains(_entry.Key.ToString())) {
						_localStarFlareMatch.Remove(_entry.Key.ToString());
						
					}					
				}
			}	
			
			if (_localStarBodyMatch.Count == 0) {
				EditorUtility.DisplayDialog("Oups!", "No local stars match your color filter criterea. Please revise settings and try again.", "Ok - I'll do that!");
				return;			
			}
			if (_localStarProminenceMatch.Count == 0) {
				EditorUtility.DisplayDialog("Oups!", "No local stars match your prominence filter criterea. Please revise settings and try again.", "Ok - I'll do that!");
				return;			
			}
			if (_localStarFlareMatch.Count == 0) {
				EditorUtility.DisplayDialog("Oups!", "No local stars match your flare filter criterea. Please revise settings and try again.", "Ok - I'll do that!");
				return;			
			}
			
			
		}
		
		// *** CREATE SPACE SCENE PREFAB ***
		bool _verifyOverwrite = false;
		GameObject _spaceScene = GameObject.Find(_nameOfSpacePrefab);
		
		// Return if user don't want to overwrite objects
		if (_spaceScene != null && !_overwrite) {			
			string _overwrittenElements = "";
			if (_starsEnabled) {
				if (TagExists(_tagCamera)) {
					if (GameObject.FindGameObjectWithTag(_tagCamera) != null) {
						if (GameObject.FindGameObjectWithTag(_tagCamera).GetComponent<Skybox>() != null) {
							if (GameObject.FindGameObjectWithTag(_tagCamera).GetComponent<Skybox>().material != null) {
								_overwrittenElements = "* Stars\n";
							}
						}					
					}
				}
			}
						
			if (_nebulasEnabled && TagExists(_tagNebula)) {
				if (GameObject.FindGameObjectsWithTag(_tagNebula).Length > 0) _overwrittenElements += "* Nebulas\n";
			}
			if (_galaxiesEnabled && TagExists(_tagGalaxy)) {
				if (GameObject.FindGameObjectsWithTag(_tagGalaxy).Length > 0) _overwrittenElements += "* Galaxies\n";
			}
			if (_planetsEnabled && TagExists(_tagPlanet)) { 
				if (GameObject.FindGameObjectsWithTag(_tagPlanet).Length > 0) _overwrittenElements += "* Planets\n";
			}
			if (_localStarEnabled && TagExists(_tagLocalStar)) {
				if (GameObject.FindGameObjectsWithTag(_tagLocalStar).Length > 0) _overwrittenElements += "* Local Star\n";
			}
			if (_overwrittenElements.Length > 0) {
				_verifyOverwrite = EditorUtility.DisplayDialog("Warning", "There is already a space scene. The following objects will be replaced and/or overwritten:\n" 
					+ _overwrittenElements, "Ok - Overwrite", "Cancel");
				if (!_verifyOverwrite) return;
			}
		}		
		
		GameObject _sGameObject;
		GameObject _sInstantiated;		
		if (_spaceScene != null) {
			_sInstantiated = _spaceScene;
		} else {
			_sGameObject = (GameObject)AssetDatabase.LoadAssetAtPath(_basePath + "Prefabs/SpaceSceneElements/"+_nameOfSpacePrefab+".prefab", typeof (GameObject));
			_sInstantiated = (GameObject)PrefabUtility.InstantiatePrefab((GameObject) _sGameObject);
		}
		
		// --- HANDLE STARS ---
		if (_starsEnabled) {
			string _starName = _starsMatch[UnityEngine.Random.Range(0, _starsMatch.Count)].ToString();
			Camera _spaceCamera = _sInstantiated.transform.Find("SpaceCamera").camera;
			if (_spaceCamera != null) {
				// This is a workaround because Unity seems to not keep material material changes to previously instantiated cameras
				Editor.DestroyImmediate(_spaceCamera.gameObject);
				_spaceCamera = ((GameObject)PrefabUtility.InstantiatePrefab((GameObject) (GameObject)AssetDatabase.LoadAssetAtPath(_basePath + _prefabSpaceCamera, typeof (GameObject)))).camera;
				_spaceCamera.transform.parent = _sInstantiated.transform;
			}
			_spaceCamera.GetComponent<Skybox>().material = (Material)AssetDatabase.LoadAssetAtPath(_basePath + _pathStars + _starName + ".mat", typeof (Material));			
		}
		
		// --- HANDLE NEBULAS ---
		if (_nebulasEnabled) {
			// Remove old nebulas (if any)
			if (_spaceScene != null) {				
				if (TagExists(_tagNebula)) {
					GameObject[] _nebulas = GameObject.FindGameObjectsWithTag(_tagNebula);
					foreach (GameObject _nebula in _nebulas) {
						Editor.DestroyImmediate(_nebula);
					}
				}
			}
			// Create new nebulas
			for (int _i = 1; _i <= _nebulaCount; _i++) {			
				GameObject _nebulaPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(_basePath + _prefabNebula, typeof (GameObject));
				GameObject _nebulaInstantiated = (GameObject)PrefabUtility.InstantiatePrefab((GameObject) _nebulaPrefab);			
				_nebulaInstantiated.transform.parent = _sInstantiated.transform;
				string _nebulaName = _nebulasMatch[UnityEngine.Random.Range(0, _nebulasMatch.Count)].ToString();
				_nebulaInstantiated.renderer.material = (Material)AssetDatabase.LoadAssetAtPath(_basePath + _pathNebulas + _nebulaName + ".mat", typeof (Material));					
				_nebulaInstantiated.transform.eulerAngles = new Vector3(UnityEngine.Random.Range(0,360), UnityEngine.Random.Range(0,360), UnityEngine.Random.Range(0,360));
			}
		}
			
		// --- HANDLE GALAXIES ---
		if (_galaxiesEnabled) {
			// Remove old galaxies (if any)
			if (TagExists(_tagGalaxy)) {
				GameObject[] _galaxies = GameObject.FindGameObjectsWithTag(_tagGalaxy);
				foreach (GameObject _galaxy in _galaxies) {
					Editor.DestroyImmediate(_galaxy);
				}
			}
			// Create new galaxies
			for (int _i = 1; _i <= _numberOfGalaxiesToAdd; _i++) {
				GameObject _galaxyPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(_basePath + _prefabGalaxy, typeof (GameObject));
				GameObject _galaxyInstantiated = (GameObject)PrefabUtility.InstantiatePrefab((GameObject) _galaxyPrefab);			
				_galaxyInstantiated.transform.parent = _sInstantiated.transform;
				Transform _galaxyTextureTransform = _galaxyInstantiated.transform.Find("GalaxyObject");
				_galaxyName = _galaxiesMatch[UnityEngine.Random.Range(0, _galaxiesMatch.Count)].ToString();
				_galaxyInstantiated.transform.eulerAngles = new Vector3(UnityEngine.Random.Range(0,360), UnityEngine.Random.Range(0,360), UnityEngine.Random.Range(0,360));
				float _newGalaxyScale = UnityEngine.Random.Range(-0.1f, 0.1f) + _galaxyDistanceScales[UnityEngine.Random.Range(0, _galaxyDistanceScales.Count)];
				_galaxyTextureTransform.renderer.material = (Material)AssetDatabase.LoadAssetAtPath(_basePath + _pathGalaxies+_galaxyName+".mat", typeof (Material));
				_galaxyInstantiated.transform.localScale = new Vector3(_newGalaxyScale, _newGalaxyScale, _newGalaxyScale);
				_galaxyTextureTransform.Translate(new Vector3(0,0,(1.0f - _newGalaxyScale) * 1000),Space.Self);
				_galaxyTextureTransform.Find("Point light").light.intensity = _galaxyTextureTransform.Find("Point light").light.intensity * _newGalaxyScale;				
				if (!_galaxyLightHasFlare) {
					// Disable light flare if not enabled
					_galaxyTextureTransform.Find("Point light").light.flare = null;
				}
				if (!_galaxyIsLightsource) {
					// Destroy light source if not enabled
					UnityEngine.Object.DestroyImmediate(_galaxyTextureTransform.Find("Point light").gameObject);
				}
			}
		}
		
		// --- HANDLE PLANETS ---		
		if (_planetsEnabled) {
			// Remove old planets (if any)
			if (TagExists(_tagPlanet)) {
				GameObject[] _planets = GameObject.FindGameObjectsWithTag(_tagPlanet);
				foreach (GameObject _planet in _planets) {
					Editor.DestroyImmediate(_planet);
				}
			}
			// Create new planets			
			for (int _i = 1; _i <= _numberOfPlanetsToAdd; _i++) {
				string _planetName = _planetsMatch[UnityEngine.Random.Range(0, _planetsMatch.Count)];
				GameObject _planetPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(_basePath + _prefabPlanetPrefix+_planetMeshDetailString + _prefabPlanetSuffix , typeof (GameObject));
				GameObject _planetInstantiated = (GameObject)PrefabUtility.InstantiatePrefab((GameObject) _planetPrefab);
				Transform _planetObject = _planetInstantiated.transform.Find("PlanetObject");
				_planetInstantiated.transform.parent = _sInstantiated.transform;
				_planetInstantiated.transform.position = new Vector3(0,0,0);
				PlanetDistance _planetDistanceUsed = _planetDistanceScales[UnityEngine.Random.Range(0, _planetDistanceScales.Count)];
				float _movePlanet = 0.0f; 
				switch (_planetDistanceUsed) {
					case PlanetDistance.VERY_CLOSE:
						_movePlanet = _distancePlanetClose;						
						// Remove VERY_CLOSE once it's been used once - only one planet can be very close
						_planetDistanceScales.Remove(PlanetDistance.VERY_CLOSE);
						break;
					case PlanetDistance.CLOSE:
						_movePlanet = _distancePlanetClose;
						// Remove CLOSE once it's been used once - only one planet can be close
						_planetDistanceScales.Remove(PlanetDistance.CLOSE);				
						break;
					case PlanetDistance.DISTANT:							
						_movePlanet = _distancePlanetDistant;
						break;
					case PlanetDistance.VERY_DISTANT:
						_movePlanet = _distancePlanetVeryDistant;
						break;
				}
								
				// Instantiate Planet				
				Vector3 _translatePlanetBy = new Vector3(
					UnityEngine.Random.Range(_movePlanet * _distanceMinRangeMultiplier, _movePlanet * _distanceMaxRangeMultiplier), 
					UnityEngine.Random.Range(_movePlanet * _distanceMinRangeMultiplier, _movePlanet * _distanceMaxRangeMultiplier), 
					UnityEngine.Random.Range(_movePlanet * _distanceMinRangeMultiplier, _movePlanet * _distanceMaxRangeMultiplier));
				_planetInstantiated.transform.Translate(_translatePlanetBy);
				_planetObject.transform.eulerAngles = new Vector3(UnityEngine.Random.Range(0,360), UnityEngine.Random.Range(0,360), UnityEngine.Random.Range(0,360));
				_planetObject.GetComponent<SU_Planet>().planetRotation = new Vector3(0,0,_planetRotationScales[UnityEngine.Random.Range(0, _planetRotationScales.Count)]);
				UnityEngine.Object _planetMaterial = AssetDatabase.LoadAssetAtPath(_basePath + _pathPlanets+_planetName+".mat", typeof (UnityEngine.Object));
				_planetObject.renderer.material = (Material) _planetMaterial;
				
				// Atmosphere
				if (_planetAtmosphere) {
					// Change the material of the atmosphere according to label on planet material
					_planetInstantiated.transform.Find("Atmosphere").renderer.material = (Material) _hPlanetAtmospheres[GetPlanetAtmosphere(_planetMaterial)];
				} else {
					// Atmosphere not enabled, remove the atmosphere transform from the instantiated prefab
					UnityEngine.Object.DestroyImmediate(_planetInstantiated.transform.Find("Atmosphere").gameObject);
				}
				
				// Planet Moons
				int _numberOfMoonsToAdd = _numberOfMoonsForPlanet[UnityEngine.Random.Range(0, _numberOfMoonsForPlanet.Count)];
				
				if (_numberOfMoonsToAdd > 0) {
					for (int _n = 1; _n <= _numberOfMoonsToAdd; _n++) {
						// Select a random orbit speed (from the filtered shortlist of orbit speeds)
						float _moonOrbitSpeed = _planetMoonRotationScales[UnityEngine.Random.Range(0,_planetMoonRotationScales.Count)];
						// Select a random distance (from the filtered shortlist of distances)						
						float _moonDistance = _planetMoonDistanceScales[UnityEngine.Random.Range(0,_planetMoonDistanceScales.Count)];
						// Load the moon prefab with the selected mesh quality
						GameObject _planetMoonPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(_basePath + _prefabPlanetMoonPrefix + _planetMeshDetailString + _prefabPlanetMoonSuffix, typeof (GameObject));
						// Instantiate the prefab to the current scene as a gameobject
						GameObject _planetMoonInstantiated  = (GameObject)PrefabUtility.InstantiatePrefab((GameObject) _planetMoonPrefab);
						// Reference the child "MoonObject"
						Transform _planetMoonMesh = _planetMoonInstantiated.transform.Find("MoonObject");
						// Set the orbit speed of the SU_Moon.cs script
						_planetMoonInstantiated.GetComponent<SU_Moon>().orbitSpeed = _moonOrbitSpeed;
						// Set the parent of the instantiated transform to previously instantiated planet
						_planetMoonInstantiated.transform.parent = _planetInstantiated.transform;
						// Set the local position of the moon to 0,0,0 (the child object of the moon prefab will be at a distance,
						// this is just so we can rotate the moon parent object and the child object moon will orbit at a distance)
						_planetMoonInstantiated.transform.localPosition = new Vector3(0,0,0);
						// Rotate the moon to a random position
						_planetMoonInstantiated.transform.Rotate(new Vector3(UnityEngine.Random.Range(0,360), UnityEngine.Random.Range(0,360), UnityEngine.Random.Range(0,360)));
						// Scale the moon to a random size
						float _moonScale = UnityEngine.Random.Range(_scaleMoonMinRangeMultiplier, _scaleMoonMaxRangeMultiplier);						
						_planetMoonMesh.localScale = new Vector3 (_moonScale, _moonScale, _moonScale);
						// Move the child transforms local position to the randomly selected moon distance						
						_planetMoonMesh.transform.localPosition = new Vector3(_planetMoonMesh.transform.position.x * _moonDistance,0,0);
						// Select and apply a random moon surface material
						string _moonMaterial = _lPlanetMoons[UnityEngine.Random.Range(0, _lPlanetMoons.Count)];						
						_planetMoonMesh.gameObject.renderer.material = (Material)AssetDatabase.LoadAssetAtPath(_basePath + _pathMoons + _moonMaterial + ".mat", typeof (Material));						
					}					
				}			
				
				// Planet Rings
				if (_planetRings != PlanetRings.NEVER) {
					string _planetRingsWidthString = "Normal";
					float _addPlanetRings = UnityEngine.Random.Range(0.0f, 1.0f);				
					if (_addPlanetRings < _planetRingsOdds || _planetRings == PlanetRings.ALWAYS) {
						switch (UnityEngine.Random.Range(0, 3)) {
						case 0:
							_planetRingsWidthString = "Narrow";
							break;
						case 1: 
							_planetRingsWidthString = "Wide";
							break;
						case 2:
							_planetRingsWidthString = "Normal";
							break;
						}
						GameObject _planetRingsPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(_basePath + _prefabPlanetRingsPrefix +_planetRingsWidthString+_planetMeshDetailString+_prefabPlanetRingsSuffix, typeof (GameObject));
						GameObject _planetRingsInstantiated = (GameObject)PrefabUtility.InstantiatePrefab((GameObject) _planetRingsPrefab);
						_planetRingsInstantiated.transform.parent = _planetInstantiated.transform;
						_planetRingsInstantiated.transform.localPosition = Vector3.zero;
						_planetRingsInstantiated.transform.localEulerAngles = Vector3.zero;
						float _planetRingsScale = UnityEngine.Random.Range(0.8f, 1.2f);
						_planetRingsInstantiated.transform.localScale = new Vector3(_planetRingsScale, _planetRingsScale, _planetRingsScale);
						string _ringMaterial = _lPlanetRings[UnityEngine.Random.Range(0, _lPlanetRings.Count)];
						_planetRingsInstantiated.renderer.material = (Material)AssetDatabase.LoadAssetAtPath(_basePath + _pathRings + _ringMaterial + ".mat", typeof (Material));
					}
				}
				
			}
		}	
		
		// --- HANDLE LOCAL STARS ---
		if (_localStarEnabled) {
			if (TagExists(_tagLocalStar)) {
			// Remove old local stars (if any)
				GameObject[] _localStars = GameObject.FindGameObjectsWithTag(_tagLocalStar);
				foreach (GameObject _localStar in _localStars) {
					Editor.DestroyImmediate(_localStar);
				}
			}
			// Create new local star		
			string _localStarPrefabSelected = _prefabLocalStarMedium;
			switch (_localStarSizeUsed) {
			case LocalStarSize.SMALL:
				_localStarPrefabSelected = _prefabLocalStarSmall;
				break;
			case LocalStarSize.MEDIUM:
				_localStarPrefabSelected = _prefabLocalStarMedium;
				break;	
			case LocalStarSize.LARGE:
				_localStarPrefabSelected = _prefabLocalStarLarge;
				break;					
			}
						
			GameObject _localStarPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(_basePath + _localStarPrefabSelected, typeof (GameObject));
			GameObject _localStarInstantiated = (GameObject)PrefabUtility.InstantiatePrefab((GameObject) _localStarPrefab);
			
			_localStarInstantiated.transform.parent = _sInstantiated.transform;
			_localStarInstantiated.transform.position = new Vector3(0,0,0);
									
			_localStarInstantiated.transform.eulerAngles = new Vector3(UnityEngine.Random.Range(0,360), UnityEngine.Random.Range(0,360), UnityEngine.Random.Range(0,360));
			
			Transform _localStarChildTransform = _localStarInstantiated.transform.Find("LocalStarChild");
			
			_localStarChildTransform.Find("ParticleSystem-Body").GetComponent<ParticleSystem>().renderer.material = (Material)AssetDatabase.LoadAssetAtPath(_basePath + _pathLocalStarsBody + _localStarBodyMatch[UnityEngine.Random.Range(0, _localStarBodyMatch.Count)] + ".mat", typeof (Material));				
			_localStarChildTransform.Find("ParticleSystem-Prominence").GetComponent<ParticleSystem>().renderer.material = (Material)AssetDatabase.LoadAssetAtPath(_basePath + _pathLocalStarsProminence + _localStarProminenceMatch[UnityEngine.Random.Range(0, _localStarProminenceMatch.Count)] + ".mat", typeof (Material));				
			if (!_localStarIsLightsource) {
				// Destroy light source if not enabled
				UnityEngine.Object.DestroyImmediate(_localStarChildTransform.Find("Point light").gameObject);
				UnityEngine.Object.DestroyImmediate(_localStarChildTransform.Find("Directional light").gameObject);
			} else {
				// Light Color
				if (_localStarLightColor != LocalStarLightColor.WHITE) {
					switch (_localStarColorUsed) {
					case LocalStarColor.BLUE:
						_localStarChildTransform.Find("Directional light").light.color = _colorLocalStarBlue;
						_localStarChildTransform.Find("Point light").light.color = _colorLocalStarBlue;
						break;
					case LocalStarColor.YELLOW:
						_localStarChildTransform.Find("Directional light").light.color = _colorLocalStarYellow;
						_localStarChildTransform.Find("Point light").light.color = _colorLocalStarYellow;
						break;
					case LocalStarColor.ORANGE:
						_localStarChildTransform.Find("Directional light").light.color = _colorLocalStarOrange;
						_localStarChildTransform.Find("Point light").light.color = _colorLocalStarOrange;
						break;
					case LocalStarColor.RED:
						_localStarChildTransform.Find("Directional light").light.color = _colorLocalStarRed;
						_localStarChildTransform.Find("Point light").light.color = _colorLocalStarRed;
						break;						
					}							
				}
				
				// Light Intensity
				switch (_localStarLightIntensityUsed) {
				case LocalStarLightIntensity.VERY_LOW:
					_localStarChildTransform.Find("Directional light").light.intensity = _intensityLocalStarVeryLow;
					_localStarChildTransform.Find("Point light").light.intensity = _intensityLocalStarVeryLow;
					break;
				case LocalStarLightIntensity.LOW:
					_localStarChildTransform.Find("Directional light").light.intensity = _intensityLocalStarLow;
					_localStarChildTransform.Find("Point light").light.intensity = _intensityLocalStarLow;
					break;
				case LocalStarLightIntensity.MEDIUM:
					_localStarChildTransform.Find("Directional light").light.intensity = _intensityLocalStarMedium;
					_localStarChildTransform.Find("Point light").light.intensity = _intensityLocalStarMedium;
					break;
				case LocalStarLightIntensity.HIGH:
					_localStarChildTransform.Find("Directional light").light.intensity = _intensityLocalStarHigh;
					_localStarChildTransform.Find("Point light").light.intensity = _intensityLocalStarHigh;
					break;
				case LocalStarLightIntensity.VERY_HIGH:
					_localStarChildTransform.Find("Directional light").light.intensity = _intensityLocalStarVeryHigh;
					_localStarChildTransform.Find("Point light").light.intensity = _intensityLocalStarVeryHigh;
					break;				
				}
								
				// Light Flare
				if (!_localStarLightHasFlare) {
					// Disable light flare if not enabled
					_localStarChildTransform.Find("Point light").light.flare = null;
				} else {
					// Select the correct flare color & size
					string _localStarFlareName = _localStarFlareMatch[UnityEngine.Random.Range(0, _localStarFlareMatch.Count)];
					_localStarChildTransform.Find("Point light").light.flare = (Flare)AssetDatabase.LoadAssetAtPath(_basePath + _pathLocalStarsFlare + _localStarFlareName + ".flare", typeof(Flare));
				}
			}			
		}										
	}
	
	/// <summary>
	/// Create a limited list with a random selection of _maxEntries.
	/// </summary>
	/// <returns>
	/// The limited random list.
	/// </returns>
	/// <param name='_list'>
	///  Source list
	/// </param>
	/// <param name='_maxEntries'>
	///  Number of random entries in the returned limited list.
	/// </param>
	static List<string> LimitedRandomList (List<string> _list, int _maxEntries) {
		// Number of attempts to select random entry before exiting while loop
		int _preventEndlessLoopBail = 10000;		
		
		// Create a new empty list
		List<string> _returnList = new List<string>();
		
		// If the maximum entries chosen is greater than the length of the source list, set it to the length of the source list
		if (_maxEntries > _list.Count) _maxEntries = _list.Count;
		
		// Iterate through the source list with a count of _maxEntries and add unique random entries
		for (int _i = 0; _i < _maxEntries; _i++) {				
			string _listItem = "";
			int _n = 0;
			// Loop until a unique random list item has been found
			while (_listItem.Length == 0 || _returnList.Contains(_listItem)) {
				// Select a random entry from the source list... it may not be unique...
				_listItem = _list[UnityEngine.Random.Range(0, _list.Count)];
				// Bail from loop if number of max tries to find unique entry has been reached
				if (_n++ > _preventEndlessLoopBail) break;
			}
			// Add the unique item
			_returnList.Add(_listItem);
		}
		// Return the new list
		return _returnList;		
	}
	
	// Populate hashtable with "andable"/"maskable" asset labels
	void PopulateHashtable<T>(Hashtable _ht, string _assetPath, string _labelPrefix, Type _enum) where T : UnityEngine.Object {
		// Get the application data path
		string _dataPath = Application.dataPath;
		// Generate the folder path to the assets
		string _folderPath = _dataPath.Substring(0 ,_dataPath.Length-6)+_assetPath;      		
		// Create an array of all the files (assets) in the folder
		string[] _filePaths = Directory.GetFiles(_folderPath);
		// Iterate through all the files in the array (folder)
		foreach (string _filePath in _filePaths) {
			// Get the path to the asset
			string _assetItemPath = _filePath.Substring(_dataPath.Length-6);
			// Load the asset as an object
			UnityEngine.Object _objAsset = AssetDatabase.LoadAssetAtPath(_assetItemPath,typeof(UnityEngine.Object));			
			// If the asset is of the same type we have spacified when calling the function...
			if (_objAsset is T) {				
				// Reference the object as _item and cast _objAsset into the correct type
				T _item = (T) _objAsset;				
				// Iterate through all the Unity labels for the asset
				foreach (string _label in AssetDatabase.GetLabels(_objAsset)) {
					// If the label has the same or more characters as the label prefix, we might be interested in this one...
					if (_label.Length >= _labelPrefix.Length) {
						// If the label prefix IS the same as the one we are after...
						if (_label.Substring(0,_labelPrefix.Length) == _labelPrefix) {
							// Create an int as reference to the index in the enum of the label
							int _enumValue = (int) System.Enum.Parse( _enum, _label.Substring(_labelPrefix.Length, _label.Length - _labelPrefix.Length).ToUpper());
							// If the hashtable doesn't already have the item...
							if (!_ht.Contains(_item.name)) {
								// Add the item to the hashtable, e.g. color-blue
								_ht.Add(_item.name, _enumValue);
							} else {
								// Item already exists in the hashtable and we need to AND the enum values to include
								// multiple labels  for the asset, e.g. color-blue AND color-red	
								// If the item in the hashtable & enumValue == 0 (it does not exist yet)...
								if (((int) _ht[_item.name] & _enumValue) == 0) {							
									// Add the "andable"/"maskable" enum value, e.g. _ht[_item.name] = 8 + 16
									_ht[_item.name] = (int) _ht[_item.name] + _enumValue;
								}
							}								
						}
					}
				}				
			}
		}			
	}		
		
	// Populate hashtable with atmospheres
	void PopulateAtmospheres<T>(Hashtable _ht, string _assetPath) where T : UnityEngine.Object {
		// Get the application data path
		string _dataPath = Application.dataPath;
		// Generate the folder path to the assets
		string _folderPath = _dataPath.Substring(0 ,_dataPath.Length-6)+_assetPath;      		
		// Create an array of all the files (assets) in the folder
		string[] _filePaths = Directory.GetFiles(_folderPath);
		// Iterate through all the files in the array (folder)
		foreach (string _filePath in _filePaths) {
			// Get the path to the asset
			string _assetItemPath = _filePath.Substring(_dataPath.Length-6);
			// Load the asset as an object
			UnityEngine.Object _objAsset = AssetDatabase.LoadAssetAtPath(_assetItemPath,typeof(UnityEngine.Object));			
			// If the asset is of the same type we have spacified when calling the function...
			if (_objAsset is T) {				
				// Reference the object as _item and cast _objAsset into the correct type
				T _item = (T) _objAsset;				
				// Add the item to the hashtable
				_ht.Add(_item.name.ToUpper(), _item);
			}
		}			
	}
	
	// Get the atsmosphere material 
	string GetPlanetAtmosphere(UnityEngine.Object _object) {
		foreach (string _label in AssetDatabase.GetLabels(_object)) {
			if (_label.Length > 11) {
				if (_label.Substring(0,11).ToUpper() == _labelPrefixAtmosphere.ToUpper()) {
					return _label.ToUpper();
				}
			}
		}
		return "";
	}
	
	// Populate a list with assets of a specific type T found in a path
	void PopulateList<T>(List<string> _list, string _assetPath) where T : UnityEngine.Object {
		// Get the application data path
		string _dataPath  = Application.dataPath;
		// Generate the folder path to the assets
		string _folderPath = _dataPath.Substring(0 ,_dataPath.Length-6)+_assetPath;      		
		// Create an array of all the files (assets) in the folder
		string[] _filePaths = Directory.GetFiles(_folderPath);
		// Iterate through all the files in the array (folder)
		foreach (string _filePath in _filePaths) {
			// Get the path to the asset
			string _assetItemPath = _filePath.Substring(_dataPath.Length-6);
			// Load the asset as an object
			UnityEngine.Object _objAsset =  AssetDatabase.LoadAssetAtPath(_assetItemPath,typeof(UnityEngine.Object));
			// If the asset is of the same type we have spacified when calling the function...
			if (_objAsset is T) {
				// Reference the object as _item and cast _objAsset into the correct type
				T _item = (T) _objAsset;
				// Add the asset/file to the list
				_list.Add(_item.name);
			}
		}
	}
	
	/// <summary>
	/// Gets a random enum and ignore values with index number before startValue.
	/// </summary>
	/// <returns>
	/// The random enum.
	/// </returns>
	/// <param name='startValue'>
	/// Start index value.
	/// </param>
	/// <typeparam name='T'>
	/// The type of enum.
	/// </typeparam>
	static T GetRandomEnum<T>(int startValue = 0) {
	    System.Array A = System.Enum.GetValues(typeof(T));
	    T V = (T)A.GetValue(UnityEngine.Random.Range(startValue,A.Length));
	    return V;
	}				
	
	// Unity throws an error if we look for a tag that isn't defined 
	// (e.g. you loaded the template scene without objects using SPACE UNITY tags
	// We must there for see if the tag exists before we see if any objects have the tag.
	bool TagExists(string _tag) {
		foreach (string _s in UnityEditorInternal.InternalEditorUtility.tags) {
			if (_s == _tag) return true;
		}
		return false;
	}
}