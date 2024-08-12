using UnityEngine.SceneManagement;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public AudioSource bgSound;
    public AudioSource efSound;

    public SceneAudioClip[] sceneAudioClips;

    [System.Serializable]
    public struct SceneAudioClip
    {
        public string sceneName;
        public AudioClip audioClip;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        foreach (var sceneAudio in sceneAudioClips)
        {
            if (arg0.name == sceneAudio.sceneName)
            {
                BgSoundPlay(sceneAudio.audioClip);
                break;
            }
        }
    }

    public void BgSoundPlay(AudioClip clip)
    {
        bgSound.clip = clip;
        bgSound.loop = true;
        bgSound.volume = 0.1f;
        bgSound.Play();
    }

    public void PlayWalkEffect(AudioClip clip)
    {
        efSound.PlayOneShot(clip);
    }
}
