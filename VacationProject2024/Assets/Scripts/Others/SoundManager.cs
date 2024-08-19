using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Singleton 인스턴스
    public static SoundManager Instance;

    // 효과음과 BGM을 저장할 딕셔너리 (이름으로 접근)
    private Dictionary<string, AudioClip> soundEffects;
    private Dictionary<string, AudioSource> activeSounds;
    private Dictionary<string, AudioClip> bgmClips;
    private AudioSource bgmSource;

    [Range(0f, 1f)]
    public float MasterVolume = 1.0f;  // 전체 볼륨
    [Range(0f, 1f)]
    public float SFXVolume = 1.0f;  // 효과음 볼륨
    [Range(0f, 1f)]
    public float BGMVolume = 1.0f;  // BGM 볼륨

    private void Awake()
    {
        // Singleton 패턴 적용
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // 리소스 폴더에서 사운드를 불러와 딕셔너리에 저장
            LoadAllSoundsFromResources();

            // 재생 중인 사운드를 관리하기 위한 딕셔너리 초기화
            activeSounds = new Dictionary<string, AudioSource>();

            // BGM을 재생할 AudioSource 생성
            bgmSource = gameObject.AddComponent<AudioSource>();
            bgmSource.loop = true;
            bgmSource.volume = BGMVolume * MasterVolume;
        }
        else
        {
            Destroy(gameObject);
        }
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
    public void PlaySound(string soundName, float volume, int repeatCount)
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
            PlaySFX(soundName, volume, repeatCount);
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
    private void PlaySFX(string sfxName, float volume, int repeatCount)
    {
        if (!activeSounds.ContainsKey(sfxName))
        {
            if (soundEffects.TryGetValue(sfxName, out AudioClip clip))
            {
                // 새로운 AudioSource를 생성하여 효과음 재생
                AudioSource newSource = gameObject.AddComponent<AudioSource>();
                newSource.clip = clip;
                newSource.volume = volume * SFXVolume * MasterVolume;
                newSource.loop = (repeatCount == 0);
                newSource.Play();

                // 반복 재생이 필요하다면 코루틴을 통해 반복 재생
                if (repeatCount > 0)
                {
                    StartCoroutine(PlaySoundRepeatedly(newSource, clip, volume, repeatCount));
                }

                // 재생 중인 사운드를 딕셔너리에 추가
                activeSounds[sfxName] = newSource;
            }
        }
    }

    // 특정 사운드를 멈추는 메서드
    public void StopSound(string soundName)
    {
        if (activeSounds.TryGetValue(soundName, out AudioSource source))
        {
            source.Stop();
            Destroy(source); // 사용한 AudioSource 제거
            activeSounds.Remove(soundName); // 딕셔너리에서 제거
        }
    }

    private IEnumerator PlaySoundRepeatedly(AudioSource source, AudioClip clip, float volume, int repeatCount)
    {
        for (int i = 0; i < repeatCount; i++)
        {
            source.PlayOneShot(clip, volume * SFXVolume * MasterVolume);
            yield return new WaitForSeconds(clip.length);
        }

        // 반복 재생이 끝나면 AudioSource를 제거하고 딕셔너리에서 제거
        activeSounds.Remove(clip.name);
        Destroy(source);
    }

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
        foreach (var source in activeSounds.Values)
        {
            source.volume = SFXVolume * MasterVolume;  // 현재 모든 AudioSource의 볼륨을 SFXVolume과 MasterVolume으로 조정
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
}
