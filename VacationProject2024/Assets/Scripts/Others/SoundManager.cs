using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class SoundManager : Singleton<SoundManager>
{
    // 효과음과 BGM을 저장할 딕셔너리 (이름으로 접근)
    private Dictionary<string, AudioClip> soundEffects;
    private List<(GameObject, AudioSource, int)> activeSounds;
    private Dictionary<string, AudioClip> bgmClips;
    private AudioSource bgmSource;
    private GameObject channelPrefab;
    private Player player;

    [Range(0f, 1f)]
    public float MasterVolume = 1.0f;  // 전체 볼륨
    [Range(0f, 1f)]
    public float SFXVolume = 1.0f;  // 효과음 볼륨
    [Range(0f, 1f)]
    public float BGMVolume = 1.0f;  // BGM 볼륨

    private void Awake()
    {
        activeSounds = new();
        LoadAllSoundsFromResources();
        channelPrefab = Resources.Load("Prefab/AudioChannel") as GameObject;
    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    private void Update()
    {
        ManageActiveSound();
    }

    // Resources/Audio 폴더의 모든 오디오 파일을 불러와서 딕셔너리에 저장
    private void LoadAllSoundsFromResources()
    {
        soundEffects = new Dictionary<string, AudioClip>();
        bgmClips = new Dictionary<string, AudioClip>();

        // Resources/Audio 폴더 안의 모든 AudioClip을 불러옴
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Audio");

        foreach (var clip in clips)
        {
            // 파일 이름에 "BGM"이 포함된 경우 BGM으로 구분
            if (clip.name.Contains("BGM"))
            {
                bgmClips[clip.name] = clip;
            }
            else
            {
                soundEffects[clip.name] = clip;  // 사운드 이름을 키로 딕셔너리에 저장
            }
        }

        Debug.Log("Loaded " + soundEffects.Count + " sound effects and " + bgmClips.Count + " BGM clips.");
    }

    // 특정 사운드를 재생하는 메서드 (BGM과 SFX를 구분하여 처리)
    public void PlaySound(GameObject soundObject, string soundName, float volume, int repeatCount)
    {
        // BGM인지 효과음인지 구분
        if (bgmClips.ContainsKey(soundName))
        {
            // BGM 재생
            PlayBGM(soundName, volume, repeatCount);
        }
        else if (soundEffects.ContainsKey(soundName))
        {
            // 효과음 재생
            PlaySFX(soundObject, soundName, volume, repeatCount);
        }
        else
        {
            Debug.LogWarning($"Sound '{soundName}' not found!");
        }
    }

    // BGM 재생 메서드
    private void PlayBGM(string bgmName, float volume, int repeatCount)
    {
        if (bgmClips.TryGetValue(bgmName, out AudioClip clip))
        {
            bgmSource.clip = clip;
            bgmSource.volume = volume * BGMVolume * MasterVolume;
            bgmSource.loop = (repeatCount == 0);
            bgmSource.Play();
        }
    }

    // 효과음 재생 메서드
    private void PlaySFX(GameObject soundObject, string sfxName, float volume, int repeatCount)
    {
        if (soundEffects.TryGetValue(sfxName, out AudioClip clip))
        {
            var channel = GetAvaliableChannel(soundObject);
            // 새로운 AudioSource를 생성하여 효과음 재생
            channel.clip = clip;
            channel.volume = volume * SFXVolume * MasterVolume;
            channel.loop = true;
            channel.spatialBlend = 1;
            channel.maxDistance = channel.volume * 30;
            channel.rolloffMode = AudioRolloffMode.Custom;
            channel.Play();

            // 재생 중인 사운드를 딕셔너리에 추가
            activeSounds.Add((soundObject, channel, repeatCount));
        }
    }

    private AudioSource GetAvaliableChannel(GameObject obj)
    {
        AudioSource channel;
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            channel = obj.transform.GetChild(i).GetComponent<AudioSource>();
            if (channel && channel.gameObject.activeSelf)
            {
                channel.gameObject.SetActive(true);
                channel.transform.localPosition = Vector3.zero;
                return channel;
            }
        }

        channel = Instantiate(channelPrefab).GetComponent<AudioSource>();
        channel.transform.parent = obj.transform;
        channel.transform.localPosition = Vector3.zero;
        channel.gameObject.SetActive(true);
        return channel;
    }

    private void UnableChannel(AudioSource source)
    {
        source.Stop();
        source.gameObject.SetActive(false);
        source.transform.parent = transform;
    }

    // 특정 사운드를 멈추는 메서드
    public void StopSound(GameObject soundObject, string soundName)
    {
        var sounds = activeSounds.FindAll(tuple => tuple.Item1 == soundObject && tuple.Item2.clip.name == soundName);
        if (sounds.Count > 0)
        {
            foreach (var sound in sounds)
            {
                sound.Item2.Stop();
                UnableChannel(sound.Item2);
                activeSounds.Remove(sound);
            }
        }
    }

    private void ManageActiveSound()
    {
        print(activeSounds.Count);
        for(int i = 0; i < activeSounds.Count;)
        {
            var sound = activeSounds[i];
            if (!sound.Item2.isPlaying)
            {
                if (sound.Item3 - 1 <= 0)
                {
                    UnableChannel(sound.Item2);
                    activeSounds.RemoveAt(i);
                    continue;
                }
                activeSounds[i] = (sound.Item1, sound.Item2, sound.Item3 - 1);
            }
            i++;
        }
    }

/*    private IEnumerator PlaySoundRepeatedly(AudioSource source, AudioClip clip, float volume, int repeatCount)
    {
        for (int i = 0; i < repeatCount; i++)
        {
            source.PlayOneShot(clip, volume * SFXVolume * MasterVolume);
            yield return new WaitForSeconds(clip.length);
        }

        // 반복 재생이 끝나면 AudioSource를 제거하고 딕셔너리에서 제거
        activeSounds.Remove((source.gameObject, clip.name));
        Destroy(source);
    }*/

    public void SetMasterVolume(float volume)
    {
        MasterVolume = volume;
        UpdateAllVolumes();
    }

    public void SetSFXVolume(float volume)
    {
        SFXVolume = volume;
        UpdateAllVolumes();
    }

    public void SetBGMVolume(float volume)
    {
        BGMVolume = volume;
        bgmSource.volume = BGMVolume * MasterVolume;
    }

    private void UpdateAllVolumes()
    {
        foreach (var source in activeSounds)
        {
            source.Item2.volume = SFXVolume * MasterVolume;  // 현재 모든 AudioSource의 볼륨을 SFXVolume과 MasterVolume으로 조정

            if (DetectPlayer(source.Item1.transform))
            {
/*                source.Item2.volume */
            }
        }

        // BGM의 볼륨도 갱신
        bgmSource.volume = BGMVolume * MasterVolume;
    }

    // 슬라이더가 변경될 때 호출될 메서드
    public void OnMasterVolumeChanged(float value)
    {
        SetMasterVolume(value);
    }

    public void OnSFXVolumeChanged(float value)
    {
        SetSFXVolume(value);
    }

    public void OnBGMVolumeChanged(float value)
    {
        SetBGMVolume(value);
    }
    public bool DetectPlayer(Transform rayOrigin)
    {
        Ray ray = new Ray(rayOrigin.position, player.transform.position - transform.position);
        Debug.DrawRay(rayOrigin.position, player.transform.position - rayOrigin.position);
        RaycastHit[] hits = Physics.RaycastAll(ray, (player.transform.position - rayOrigin.position).magnitude, layerMask: LayerMask.GetMask("Wall"));
        if (hits.Length > 0) return false;
        return true;
    }
}
