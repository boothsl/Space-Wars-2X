// ************************************************************************** //
// InteractiveMusicEditor.cs
// Copyright 2013 - Artific Games LLC
// 
// Created by: Brian Marshburn
// Created on: 11/2/2013
// ************************************************************************** //
using UnityEditor;
using UnityEngine;

/// <summary>
/// A custom editor for the InteractiveMusicTrigger component.
/// </summary>
[CustomEditor(typeof(InteractiveMusicTrigger))]
public class InteractiveMusicTriggerEditor : Editor
{

    #region Fields

    #region Fields - Serialized

    /// <summary>
    /// The changeOnEnter property of the InteractiveMusic component.
    /// </summary>
    protected SerializedProperty changeOnEnterProp;

    /// <summary>
    /// The changeOnExit property of the InteractiveMusic component.
    /// </summary>
    protected SerializedProperty changeOnExitProp;

    /// <summary>
    /// The enterMode property of the InteractiveMusic component.
    /// </summary>
    protected SerializedProperty enterModeProp;

    /// <summary>
    /// The enterTheme property of the InteractiveMusic component.
    /// </summary>
    protected SerializedProperty enterThemeProp;

    /// <summary>
    /// The exitMode property of the InteractiveMusic component.
    /// </summary>
    protected SerializedProperty exitModeProp;

    /// <summary>
    /// The exitTheme property of the InteractiveMusic component.
    /// </summary>
    protected SerializedProperty exitThemeProp;

    /// <summary>
    /// The music property of the InteractiveMusic component.
    /// </summary>
    protected SerializedProperty musicProp;

    #endregion

    #region Fields - Other

    /// <summary>
    /// A collection of mode names for enter selection.
    /// </summary>
    protected string[] enterModeNames;

    /// <summary>
    /// A collection of mode names for exit selection.
    /// </summary>
    protected string[] exitModeNames;

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
        int enterTheme = enterThemeProp.intValue;
        int exitTheme = exitThemeProp.intValue;
        Object music = musicProp.objectReferenceValue;

        // Draw the default inspector
        EditorGUIUtility.LookLikeControls();
        DrawDefaultInspector();
        EditorGUILayout.Space();

        // Draw the custom inspector
        serializedObject.Update();

        changeOnEnterProp.boolValue = EditorGUILayout.BeginToggleGroup("Change On Enter", changeOnEnterProp.boolValue);
        enterThemeProp.intValue = EditorGUILayout.Popup("Theme", enterThemeProp.intValue, themeNames);
        enterModeProp.intValue = EditorGUILayout.Popup("Mode", enterModeProp.intValue, enterModeNames);
        EditorGUILayout.EndToggleGroup();
        
        EditorGUILayout.Space();
        
        changeOnExitProp.boolValue = EditorGUILayout.BeginToggleGroup("Change On Exit", changeOnExitProp.boolValue);
        exitThemeProp.intValue = EditorGUILayout.Popup("Theme", exitThemeProp.intValue, themeNames);
        exitModeProp.intValue = EditorGUILayout.Popup("Mode", exitThemeProp.intValue, exitModeNames);
        EditorGUILayout.EndToggleGroup();

        // Refresh popups if needed
        if (enterTheme != enterThemeProp.intValue)
        {
            RefreshPopups();
            enterThemeProp.intValue = 0;
        }

        if (exitTheme != exitThemeProp.intValue)
        {
            RefreshPopups();
            exitThemeProp.intValue = 0;
        }

        if (music != musicProp.objectReferenceValue)
        {
            RefreshPopups();
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
        changeOnEnterProp = serializedObject.FindProperty("changeOnEnter");
        changeOnExitProp = serializedObject.FindProperty("changeOnExit");
        enterModeProp = serializedObject.FindProperty("enterMode");
        enterThemeProp = serializedObject.FindProperty("enterTheme");
        exitModeProp = serializedObject.FindProperty("exitMode");
        exitThemeProp = serializedObject.FindProperty("exitTheme");
        musicProp = serializedObject.FindProperty("music");

        // Refresh popups
        RefreshPopups();
    }

    /// <summary>
    /// Refreshes the theme and mode popup values.
    /// </summary>
    protected void RefreshPopups()
    {
        if (musicProp.objectReferenceValue == null)
        {
            themeNames = new string[0];
            enterModeNames = new string[0];
            exitModeNames = new string[0];
            return;
        }

        InteractiveMusicTheme[] themes = ((InteractiveMusic)musicProp.objectReferenceValue).themes;
        themeNames = InteractiveMusic.GetThemeNames(themes);
        InteractiveMusicTheme enterTheme = themes.Length == 0 ? null : themes[enterThemeProp.intValue];
        enterModeNames = InteractiveMusic.GetModeNames(enterTheme);
        InteractiveMusicTheme exitTheme = themes.Length == 0 ? null : themes[exitThemeProp.intValue];
        exitModeNames = InteractiveMusic.GetModeNames(exitTheme);
    }

    #endregion

    #endregion

}
