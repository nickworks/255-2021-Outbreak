using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// An effect that is played when triggered by another script.
/// </summary>
[System.Serializable]
public class Sound
{
    /// <summary>
    /// The name used to identify the sound effect in other scripts.
    /// </summary>
    public string name;
    /// <summary>
    /// The clip that is played when this sound is triggered.
    /// </summary>
    public AudioClip clip;
    /// <summary>
    /// How loud the sound is.
    /// </summary>
    [Range(0f, 1f)]
    public float volume;
    /// <summary>
    /// The pitch of the sound.
    /// </summary>
    [Range(.1f, 3f)]
    public float pitch;

    [HideInInspector]
    public AudioSource source;
}
