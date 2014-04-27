// ************************************************************************** //
// InteractiveMusicEditor.cs
// Copyright 2013 - Artific Games LLC
// 
// Created by: Brian Marshburn
// Created on: 10/11/2013
// ************************************************************************** //
using UnityEditor;
using UnityEngine;

/// <summary>
/// A custom editor for the InteractiveMusic component.
/// </summary>
[CustomEditor(typeof(InteractiveMusic))]
public class InteractiveMusicEditor : Editor
{

    #region Fields

    #region Fields - Serialized

    /// <summary>
    /// The currentTheme property of the InteractiveMusic component.
    /// </summary>
    protected SerializedProperty currentThemeProp;

    /// <summary>
    /// The currentMode property of the InteractiveMusic component.
    /// </summary>
    protected SerializedProperty currentModeProp;

    /// <summary>
    /// The fadeTime property of the InteractiveMusic component.
    /// </summary>
    protected SerializedProperty fadeTimeProp;

    /// <summary>
    /// The playOnAwake property of the InteractiveMusic component.
    /// </summary>
    protected SerializedProperty playOnAwakeProp;

    /// <summary>
    /// The volume property of the InteractiveMusic component.
    /// </summary>
    protected SerializedProperty volumeProp;

    /// <summary>
    /// The themes property of the InteractiveMusic component.
    /// </summary>
    protected SerializedProperty themesProp;

    #endregion

    #region Fields - Other

    /// <summary>
    /// A collection of mode names for selection.
    /// </summary>
    protected string[] modeNames;

    /// <summary>
    /// A collection of theme names for selection.
    /// </summary>
    protected string[] themeNames;

    #endregion

    #endregion

    #region Methods

    #region Methods - Public

    /// <summary>
    /// Implement this function to make a custom inspector.
    /// </summary>
    public override void OnInspectorGUI()
    {
        int selectTheme = currentThemeProp.intValue;

        // Draw the default inspector for the Themes array
        EditorGUIUtility.LookLikeControls();
        DrawDefaultInspector();
        EditorGUILayout.Space();

        // Draw the custom inspector for other GUI controls
        serializedObject.Update();

        playOnAwakeProp.boolValue = EditorGUILayout.Toggle("Play On Awake", playOnAwakeProp.boolValue);
        EditorGUILayout.PropertyField(fadeTimeProp, new GUIContent("Fade Time"));
        EditorGUILayout.Slider(volumeProp, 0, 1, new GUIContent("Volume"));
        currentThemeProp.intValue = EditorGUILayout.Popup("Current Theme", currentThemeProp.intValue, themeNames);
        currentModeProp.intValue = EditorGUILayout.Popup("Current Mode", currentModeProp.intValue, modeNames);

        // Refresh popups if needed
        if (selectTheme != currentThemeProp.intValue)
        {
            RefreshPopups();
            currentModeProp.intValue = 0;
        }

        serializedObject.ApplyModifiedProperties();
    }

    #endregion

    #region Methods - Protected

    /// <summary>
    /// This function is called when the object is loaded.
    /// </summary>
    protected void OnEnable()
    {
        // Setup serialized properties
        currentThemeProp = serializedObject.FindProperty("currentTheme");
        currentModeProp = serializedObject.FindProperty("currentMode");
        fadeTimeProp = serializedObject.FindProperty("fadeTime");
        playOnAwakeProp = serializedObject.FindProperty("playOnAwake");
        volumeProp = serializedObject.FindProperty("volume");
        themesProp = serializedObject.FindProperty("themes");

        // Refresh popups
        RefreshPopups();
    }

    /// <summary>
    /// Refreshes the theme and mode popup values.
    /// </summary>
    protected void RefreshPopups()
    {
        InteractiveMusicTheme[] themes = themesProp.GetArray<InteractiveMusicTheme>();
        themeNames = InteractiveMusic.GetThemeNames(themes);
        InteractiveMusicTheme theme = themes.Length == 0 ? null : themes[currentThemeProp.intValue];
        modeNames = InteractiveMusic.GetModeNames(theme);
    }

    #endregion

    #endregion

}
