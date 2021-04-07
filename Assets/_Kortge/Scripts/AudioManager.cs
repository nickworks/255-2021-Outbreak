using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

/// <summary>
/// Plays sounds when asked to by other objects.
/// </summary>
public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    /// <summary>
    /// Each sound effect is stored and allowed to have their volume and pitch altered.
    /// </summary>
    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    /// <summary>
    /// Plays the selected sound.
    /// </summary>
    /// <param name="name"></param>
    public void Play (string name)
    {
        Sound s = Array.Find(sounds, Sound => Sound.name == name);
        s.source.Play();
    }
    /// <summary>
    /// Stops the selected sound.
    /// </summary>
    /// <param name="name"></param>
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, Sound => Sound.name == name);
        s.source.Stop();
    }
}
