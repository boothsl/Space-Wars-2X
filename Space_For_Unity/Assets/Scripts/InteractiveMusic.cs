// ************************************************************************** //
// InteractiveMusic.cs
// Copyright 2013 - Artific Games LLC
// 
// Created by: Brian Marshburn
// Created on: 10/2/2013
// ************************************************************************** //
using System;
using UnityEngine;

/// <summary>
/// The root object of an Interactive Music system.
/// </summary>
public class InteractiveMusic : MonoBehaviour
{

    #region Fields

    #region Fields - Public

    /// <summary>
    /// The index of the currently selected theme.
    /// </summary>
    [HideInInspector]
    public int currentTheme;

    /// <summary>
    /// The index of the currently selected mode.
    /// </summary>
    [HideInInspector]
    public int currentMode;

    /// <summary>
    /// The time to fade interactive music components in and out.
    /// </summary>
    [HideInInspector]
    public float fadeTime;

    /// <summary>
    /// If set to true, the audio source will automatically start playing on awake.
    /// </summary>
    [HideInInspector]
    public bool playOnAwake = true;

    /// <summary>
    /// The volume of the interactive music (0.0 to 1.0).
    /// </summary>
    [HideInInspector]
    public float volume;

    /// <summary>
    /// A collection of themes used in the interactive music.
    /// </summary>
    public InteractiveMusicTheme[] themes;

    /// <summary>
    /// A collection of AudioSources used to play the current mode.
    /// </summary>
    protected AudioSource[] current;

    /// <summary>
    /// A collection of AudioSources used to play the mode that is fading in.
    /// </summary>
    protected AudioSource[] next;

    /// <summary>
    /// Indicates whether the system is fading between modes.
    /// </summary>
    protected bool isFading;

    /// <summary>
    /// The inverse of the time to fade music stems in and out (for update optimization).
    /// </summary>
    protected float fadeConstant;

    /// <summary>
    /// The elapsed time since a fade began.
    /// </summary>
    protected float fadeTimer;

    #endregion

    #endregion

    #region Methods

    #region Methods - Public Static

    /// <summary>
    /// Gets a list of mode names for the given theme.
    /// </summary>
    /// <param name="theme">The theme from which to get mode names.</param>
    /// <returns>A list of mode names for the given theme.</returns>
    public static string[] GetModeNames(InteractiveMusicTheme theme)
    {
        if (theme == null)
            return new string[0];

        string[] modeNames = new string[theme.modes.Length];
        for (int i = 0; i < theme.modes.Length; i++)
            modeNames[i] = theme.modes[i] == null ? "" : theme.modes[i].name;

        return modeNames;
    }

    /// <summary>
    /// Gets a list of theme names for the given array of themes.
    /// </summary>
    /// <param name="theme">The array of themes from which to get names.</param>
    /// <returns>A list of theme names for the given array of themes.</returns>
    public static string[] GetThemeNames(InteractiveMusicTheme[] themes)
    {
        if (themes == null)
            return new string[0];

        string[] themeNames = new string[themes.Length];
        for (int i = 0; i < themes.Length; i++)
            themeNames[i] = themes[i] == null ? "" : themes[i].title;

        return themeNames;
    }

    #endregion

    #region Methods - Public

    /// <summary>
    /// Changes to the theme and mode with the given indices.
    /// </summary>
    public void ChangeMode(int theme, int mode)
    {
        if (current == null)
        {
            Debug.LogWarning("Cannot change mode. No music is currently playing.");
            return;
        }

        if (isFading)
        {
            Debug.LogWarning("Cannot change mode. Fade is currently in progress.");
            return;
        }

        if (theme < 0 || theme >= themes.Length)
        {
            Debug.LogWarning("Cannot change mode. Theme index is out of bounds.");
            return;
        }

        if (mode < 0 || mode >= themes[theme].modes.Length)
        {
            Debug.LogWarning("Cannot change mode. Mode index is out of bounds.");
            return;
        }

        isFading = true;
        float startTime = current[0].time;
        next = CreateStems(themes[theme].modes[mode].stems);
        for (int i = 0; i < next.Length; i++)
        {
            next[i].Play();
            next[i].time = startTime;
        }

        fadeTimer = 0;
        currentTheme = theme;
        currentMode = mode;
    }

    /// <summary>
    /// Changes to the theme and mode with the given title and name.
    /// </summary>
    /// <remarks>The title and name are not case-sensitive.</remarks>
    public void ChangeMode(string theme, string mode)
    {
        for (int i = 0; i < themes.Length; i++)
        {
            if (String.Compare(themes[i].title, theme, true) == 0)
            {
                for (int j = 0; j < themes[i].modes.Length; j++)
                {
                    if (String.Compare(themes[i].modes[j].name, mode, true) == 0)
                    {
                        ChangeMode(i, j);
                        return;
                    }
                }

                Debug.LogWarning("Cannot change mode. Mode name not found.");
                return;
            }
        }

        Debug.LogWarning("Cannot change mode. Theme name not found.");
    }

    /// <summary>
    /// Plays the currently selected theme and mode.
    /// </summary>
    public void Play()
    {
        if (current != null && current.Length > 0)
        {
            Debug.LogWarning("Music is currently playing.");
            return;
        }

        isFading = false;
        fadeTimer = float.MaxValue;

        current = CreateStems(themes[currentTheme].modes[currentMode].stems);
        next = new AudioSource[0];

        for (int i = 0; i < current.Length; i++)
        {
            current[i].volume = volume;
            current[i].Play();
        }
    }

    /// <summary>
    /// Stops playing all clips.
    /// </summary>
    public void Stop()
    {
        for (int i = 0; i < current.Length; i++)
        {
            current[i].Stop();
            GameObject.Destroy(current[i]);
        }
        for (int i = 0; i < next.Length; i++)
        {
            next[i].Stop();
            GameObject.Destroy(next[i]);
        }

        isFading = false;
        fadeTimer = float.MaxValue;

        current = new AudioSource[0];
        next = new AudioSource[0];
    }

    #endregion

    #region Methods - Protected

    /// <summary>
    /// Initializes music stems for playback.
    /// </summary>
    protected AudioSource[] CreateStems(AudioClip[] clips)
    {
        AudioSource[] stems = new AudioSource[clips.Length];
        for (int i = 0; i < clips.Length; i++)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = clips[i];
            audioSource.playOnAwake = false;
            audioSource.loop = true;
            audioSource.volume = 0;
            stems[i] = audioSource;
        }
        return stems;
    }

    /// <summary>
    /// Use this for initialization.
    /// </summary>
    protected void Awake()
    {
        fadeConstant = 1 / fadeTime;
        if (playOnAwake)
            Play();
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    protected void Update()
    {
        // No Fade
        if (!isFading)
            return;

        // End Fade
        if (fadeTimer > fadeTime)
        {
            for (int i = 0; i < current.Length; i++)
            {
                current[i].volume = 0;
                current[i].Stop();
                GameObject.Destroy(current[i]);
            }
            for (int i = 0; i < next.Length; i++)
            {
                next[i].volume = volume;
            }
            current = next;
            next = new AudioSource[0];

            isFading = false;
            fadeTimer = float.MaxValue;

            return;
        }

        // Continue Fade
        fadeTimer += Time.deltaTime;
        for (int i = 0; i < current.Length; i++)
            current[i].volume = Mathf.Lerp(volume, 0, fadeTimer * fadeConstant);
        for (int i = 0; i < next.Length; i++)
            next[i].volume = Mathf.Lerp(0, volume, fadeTimer * fadeConstant);
    }

    #endregion

    #endregion

}
