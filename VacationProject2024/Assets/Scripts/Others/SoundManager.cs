using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource Audio;

    public void SetMusicVolume(float volume)
    {
        Audio.volume = volume;
    }
    public void SetEffectVolume(float volume)
    {
        Audio.volume = volume;
    }
}
