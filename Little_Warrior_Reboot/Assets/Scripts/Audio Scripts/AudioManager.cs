using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioMixerGroup _musicGroup;
    [SerializeField] AudioMixerGroup _SFXGroup;

    [SerializeField] Sound[] _sounds;

    public static AudioManager instance;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        foreach(Sound s in _sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.playOnAwake = false;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

            if (s.name.Contains("FX"))
            {
                s.source.outputAudioMixerGroup = _SFXGroup;
            }
            else
            {
                s.source.outputAudioMixerGroup = _musicGroup;
            }
        }
    }

    public void Play(string name)
    {
        Sound s = null;

        for(int i = 0; i < _sounds.Length; i++)
        {
            if(_sounds[i].name == name)
            {
                s = _sounds[i];
                i = _sounds.Length;
            }
        }

        if(s == null)
        {
            return;
        }

        for(int i = 0; i < _sounds.Length; i++)
        {
            if(_sounds[i].source.isPlaying && !name.Contains("FX"))
            {
                _sounds[i].source.Stop();
            }
        }

        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(_sounds, sound => sound.name == name);
        
        if(s == null)
        {
            return;
        }

        s.source.Stop();
    }

    public bool IsPlaying(string name)
    {
        Sound s = Array.Find(_sounds, sound => sound.name == name);
        if (s == null) return false;

        return (s.source.isPlaying);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

}
