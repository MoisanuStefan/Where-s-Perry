using UnityEngine.Audio;
using UnityEngine;
using System;

public class SoundManager : MonoBehaviour
{

    public Sound[] sounds;

    private void Awake()
    {

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;

        }
        PlayLoop("theme");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s.source != null)
        {
            s.source.Play();
        }
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s.source != null)
        {
            s.source.Stop();
        }
    }

    public void PlayLoop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s.source != null)
        {
            s.source.loop = true;
            s.source.Play();
        }
    }

    public void MapBackButtonPress()
    {
       
        Array.Find(sounds, sound => sound.name == "platform_go").source.Stop();
        Array.Find(sounds, sound => sound.name == "platform_come").source.Stop();


    }

}
