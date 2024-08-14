using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Singleton 인스턴스
    public static SoundManager Instance;


    // 효과음을 저장할 딕셔너리 (이름으로 접근)
    private Dictionary<string, AudioClip> soundEffects;
    private Dictionary<string, AudioSource> activeSounds;

    [Range(0f, 1f)]
    public float MasterVolume = 1.0f;  // 전체 볼륨


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

        // Resources/Audio 폴더 안의 모든 AudioClip을 불러옴
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Audio");

        foreach (var clip in clips)
        {
            soundEffects[clip.name] = clip;  // 사운드 이름을 키로 딕셔너리에 저장
        }

        Debug.Log("Loaded " + soundEffects.Count + " sound effects.");
    }

    // 특정 사운드를 재생하는 메서드
    public void PlaySound(string soundName, float volume, int repeatCount)
    {
        if (soundEffects.TryGetValue(soundName, out AudioClip clip))
        {
            if (!activeSounds.ContainsKey(soundName))
            {
                // 새로운 AudioSource를 생성하여 사운드 재생
                AudioSource newSource = gameObject.AddComponent<AudioSource>();
                newSource.clip = clip;
                newSource.volume = volume;
                newSource.loop = (repeatCount == 0);
                newSource.Play();

                // 반복 재생이 필요하다면 코루틴을 통해 반복 재생
                if (repeatCount > 0)
                {
                    StartCoroutine(PlaySoundRepeatedly(newSource, clip, volume, repeatCount));
                }

                // 재생 중인 사운드를 딕셔너리에 추가
                activeSounds[soundName] = newSource;
            }
        }
        else
        {
            Debug.LogWarning($"Sound '{soundName}' not found!");
        }
    }

    // 특정 사운드를 인덱스로 멈추는 메서드
    public void StopSound(string soundName)
    {
        if (activeSounds.TryGetValue(soundName, out AudioSource source))
        {
            source.Stop();
            Destroy(source); // 사용한 AudioSource 제거
            activeSounds.Remove(soundName); // 딕셔너리에서 제거
        }
        else
        {
            Debug.LogWarning($"No active sound found with name '{soundName}' to stop.");
        }
    }

    private IEnumerator PlaySoundRepeatedly(AudioSource source, AudioClip clip, float volume, int repeatCount)
    {
        for (int i = 0; i < repeatCount; i++)
        {
            source.PlayOneShot(clip, volume);
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
    private void UpdateAllVolumes()
    {
        foreach (var source in activeSounds.Values)
        {
            source.volume = MasterVolume;  // 현재 모든 AudioSource의 볼륨을 MasterVolume으로 조정
        }
    }

    // 슬라이더가 변경될 때 호출될 메서드
    public void OnSliderValueChanged(float value)
    {
        SetMasterVolume(value);
    }
}
