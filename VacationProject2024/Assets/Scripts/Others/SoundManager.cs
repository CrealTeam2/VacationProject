using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Singleton �ν��Ͻ�
    public static SoundManager Instance;


    // ȿ������ ������ ��ųʸ� (�̸����� ����)
    private Dictionary<string, AudioClip> soundEffects;
    private Dictionary<string, AudioSource> activeSounds;

    [Range(0f, 1f)]
    public float MasterVolume = 1.0f;  // ��ü ����


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

        // Resources/Audio ���� ���� ��� AudioClip�� �ҷ���
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Audio");

        foreach (var clip in clips)
        {
            soundEffects[clip.name] = clip;  // ���� �̸��� Ű�� ��ųʸ��� ����
        }

        Debug.Log("Loaded " + soundEffects.Count + " sound effects.");
    }

    // Ư�� ���带 ����ϴ� �޼���
    public void PlaySound(string soundName, float volume, int repeatCount)
    {
        if (soundEffects.TryGetValue(soundName, out AudioClip clip))
        {
            if (!activeSounds.ContainsKey(soundName))
            {
                // ���ο� AudioSource�� �����Ͽ� ���� ���
                AudioSource newSource = gameObject.AddComponent<AudioSource>();
                newSource.clip = clip;
                newSource.volume = volume;
                newSource.loop = (repeatCount == 0);
                newSource.Play();

                // �ݺ� ����� �ʿ��ϴٸ� �ڷ�ƾ�� ���� �ݺ� ���
                if (repeatCount > 0)
                {
                    StartCoroutine(PlaySoundRepeatedly(newSource, clip, volume, repeatCount));
                }

                // ��� ���� ���带 ��ųʸ��� �߰�
                activeSounds[soundName] = newSource;
            }
        }
        else
        {
            Debug.LogWarning($"Sound '{soundName}' not found!");
        }
    }

    // Ư�� ���带 �ε����� ���ߴ� �޼���
    public void StopSound(string soundName)
    {
        if (activeSounds.TryGetValue(soundName, out AudioSource source))
        {
            source.Stop();
            Destroy(source); // ����� AudioSource ����
            activeSounds.Remove(soundName); // ��ųʸ����� ����
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

        // �ݺ� ����� ������ AudioSource�� �����ϰ� ��ųʸ����� ����
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
            source.volume = MasterVolume;  // ���� ��� AudioSource�� ������ MasterVolume���� ����
        }
    }

    // �����̴��� ����� �� ȣ��� �޼���
    public void OnSliderValueChanged(float value)
    {
        SetMasterVolume(value);
    }
}
