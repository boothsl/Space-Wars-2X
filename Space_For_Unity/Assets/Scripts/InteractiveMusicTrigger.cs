// ************************************************************************** //
// InteractiveMusic.cs
// Copyright 2013 - Artific Games LLC
// 
// Created by: Brian Marshburn
// Created on: 11/2/2013
// ************************************************************************** //
using System;
using UnityEngine;

/// <summary>
/// Triggers a change in the Interactive Music system.
/// </summary>
[RequireComponent(typeof(Collider))]
public class InteractiveMusicTrigger : MonoBehaviour
{

    /// <summary>
    /// Indicates whether to change on entering the trigger.
    /// </summary>
    [HideInInspector]
    public bool changeOnEnter;

    /// <summary>
    /// Indicates whether to change on exiting the trigger.
    /// </summary>
    [HideInInspector]
    public bool changeOnExit;

    /// <summary>
    /// The index of the mode to play on entering the trigger.
    /// </summary>
    [HideInInspector]
    public int enterMode;

    /// <summary>
    /// The index of the theme to play on entering the trigger.
    /// </summary>
    [HideInInspector]
    public int enterTheme;

    /// <summary>
    /// The index of the mode to play on exiting the trigger.
    /// </summary>
    [HideInInspector]
    public int exitMode;

    /// <summary>
    /// The index of the theme to play on exiting the trigger.
    /// </summary>
    [HideInInspector]
    public int exitTheme;

    /// <summary>
    /// A reference to the Interactive Music.
    /// </summary>
    public InteractiveMusic music;

    /// <summary>
    /// The tag of colliders that can trigger a change.
    /// </summary>
    public string otherTag;

    /// <summary>
    /// Use this for initialization.
    /// </summary>
	void Start ()
    {
        if (music == null)
        {
            music = (InteractiveMusic)FindObjectOfType(typeof(InteractiveMusic));
            if (music == null)
                Debug.LogWarning("An instance of InteractiveMusic cannot be found.");
        }
	}

    /// <summary>
    /// Called when the Collider other enters the trigger.
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        if (changeOnEnter && other.tag == otherTag)
            music.ChangeMode(enterTheme, enterMode);
	}

    /// <summary>
    /// Called when the Collider other exits the trigger.
    /// </summary>
    void OnTriggerExit(Collider other)
    {
        if (changeOnExit && other.tag == otherTag)
            music.ChangeMode(exitTheme, exitMode);
    }

}
