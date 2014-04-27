// ************************************************************************** //
// InteractiveMusicTheme.cs
// Copyright 2013 - Artific Games LLC
// 
// Created by: Brian Marshburn
// Created on: 10/11/2013
// ************************************************************************** //
using System;
using UnityEngine;

/// <summary>
/// A serializable data class to store music themes and modes.
/// </summary>
[Serializable]
public class InteractiveMusicTheme : MonoBehaviour
{

    #region Fields

    /// <summary>
    /// The title of the theme.
    /// </summary>
    public string title;

    /// <summary>
    /// A collection of modes within the theme.
    /// </summary>
    public Mode[] modes;

    #endregion

    #region Classes

    /// <summary>
    /// A serializable data class to store the stems comprising a mode.
    /// </summary>
    [Serializable]
    public class Mode
    {

        #region Fields

        /// <summary>
        /// The title of the mode.
        /// </summary>
        public string name;

        /// <summary>
        /// A collection of stems comprising the mode.
        /// </summary>
        public AudioClip[] stems;

        #endregion

    }

    #endregion

}
