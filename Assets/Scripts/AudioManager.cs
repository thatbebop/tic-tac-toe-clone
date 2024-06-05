using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    private void Awake()
    {
        foreach (var sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
        }
    }

    public void Play(string name)
    {
        Sound sound = Array.Find(sounds, x => x.name == name);
        sound.source.Play();
    }
}