using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Singleton �ν��Ͻ�
    public static SoundManager Instance;

    // ȿ������ BGM�� ������ ��ųʸ� (�̸����� ����)
    private Dictionary<string, AudioClip> soundEffects;
    private Dictionary<string, AudioSource> activeSounds;
    private Dictionary<string, AudioClip> bgmClips;
    private AudioSource bgmSource;

    [Range(0f, 1f)]
    public float MasterVolume = 1.0f;  // ��ü ����
    [Range(0f, 1f)]
    public float SFXVolume = 1.0f;  // ȿ���� ����
    [Range(0f, 1f)]
    public float BGMVolume = 1.0f;  // BGM ����

    private void Awake()
    {
        // Singleton ���� ����
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // ���ҽ� �������� ���带 �ҷ��� ��ųʸ��� ����
            LoadAllSoundsFromResources();

            // ��� ���� ���带 �����ϱ� ���� ��ųʸ� �ʱ�ȭ
            activeSounds = new Dictionary<string, AudioSource>();

            // BGM�� ����� AudioSource ����
            bgmSource = gameObject.AddComponent<AudioSource>();
            bgmSource.loop = true;
            bgmSource.volume = BGMVolume * MasterVolume;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Resources/Audio ������ ��� ����� ������ �ҷ��ͼ� ��ųʸ��� ����
    private void LoadAllSoundsFromResources()
    {
        soundEffects = new Dictionary<string, AudioClip>();
        bgmClips = new Dictionary<string, AudioClip>();

        // Resources/Audio ���� ���� ��� AudioClip�� �ҷ���
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Audio");

        foreach (var clip in clips)
        {
            // ���� �̸��� "BGM"�� ���Ե� ��� BGM���� ����
            if (clip.name.Contains("BGM"))
            {
                bgmClips[clip.name] = clip;
            }
            else
            {
                soundEffects[clip.name] = clip;  // ���� �̸��� Ű�� ��ųʸ��� ����
            }
        }

        Debug.Log("Loaded " + soundEffects.Count + " sound effects and " + bgmClips.Count + " BGM clips.");
    }

    // Ư�� ���带 ����ϴ� �޼��� (BGM�� SFX�� �����Ͽ� ó��)
    public void PlaySound(string soundName, float volume, int repeatCount)
    {
        // BGM���� ȿ�������� ����
        if (bgmClips.ContainsKey(soundName))
        {
            // BGM ���
            PlayBGM(soundName, volume, repeatCount);
        }
        else if (soundEffects.ContainsKey(soundName))
        {
            // ȿ���� ���
            PlaySFX(soundName, volume, repeatCount);
        }
        else
        {
            Debug.LogWarning($"Sound '{soundName}' not found!");
        }
    }

    // BGM ��� �޼���
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

    // ȿ���� ��� �޼���
    private void PlaySFX(string sfxName, float volume, int repeatCount)
    {
        if (!activeSounds.ContainsKey(sfxName))
        {
            if (soundEffects.TryGetValue(sfxName, out AudioClip clip))
            {
                // ���ο� AudioSource�� �����Ͽ� ȿ���� ���
                AudioSource newSource = gameObject.AddComponent<AudioSource>();
                newSource.clip = clip;
                newSource.volume = volume * SFXVolume * MasterVolume;
                newSource.loop = (repeatCount == 0);
                newSource.Play();

                // �ݺ� ����� �ʿ��ϴٸ� �ڷ�ƾ�� ���� �ݺ� ���
                if (repeatCount > 0)
                {
                    StartCoroutine(PlaySoundRepeatedly(newSource, clip, volume, repeatCount));
                }

                // ��� ���� ���带 ��ųʸ��� �߰�
                activeSounds[sfxName] = newSource;
            }
        }
    }

    // Ư�� ���带 ���ߴ� �޼���
    public void StopSound(string soundName)
    {
        if (activeSounds.TryGetValue(soundName, out AudioSource source))
        {
            source.Stop();
            Destroy(source); // ����� AudioSource ����
            activeSounds.Remove(soundName); // ��ųʸ����� ����
        }
    }

    private IEnumerator PlaySoundRepeatedly(AudioSource source, AudioClip clip, float volume, int repeatCount)
    {
        for (int i = 0; i < repeatCount; i++)
        {
            source.PlayOneShot(clip, volume * SFXVolume * MasterVolume);
            yield return new WaitForSeconds(clip.length);
        }

        // �ݺ� ����� ������ AudioSource�� �����ϰ� ��ųʸ����� ����
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
            source.volume = SFXVolume * MasterVolume;  // ���� ��� AudioSource�� ������ SFXVolume�� MasterVolume���� ����
        }

        // BGM�� ������ ����
        bgmSource.volume = BGMVolume * MasterVolume;
    }

    // �����̴��� ����� �� ȣ��� �޼���
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
