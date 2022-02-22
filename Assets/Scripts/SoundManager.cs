using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    private int maxAuds = 100;
    private AudioSource[] soundBarSFX;
    private AudioSource soundBarBGM;
    private float sfxVolume = 1;
    private float bgmVolume = 1;

    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if (!instance)
            {
                GameObject soundManager = new GameObject("SoundManager");
                instance = soundManager.AddComponent<SoundManager>();
                instance.soundBarSFX = new AudioSource[instance.maxAuds];

                for (int i = 0; i < instance.soundBarSFX.Length; i++)
                {
                    GameObject soundBarSFX = new GameObject("SoundBarSFX");
                    instance.soundBarSFX[i] = soundBarSFX.AddComponent<AudioSource>();
                    soundBarSFX.transform.SetParent(soundManager.transform);
                }
                GameObject soundBarBGM = new GameObject("SoundBarBGM");
                instance.soundBarBGM = soundBarBGM.AddComponent<AudioSource>();
                instance.soundBarBGM.loop = true;
                soundBarBGM.transform.SetParent(soundManager.transform);

                DontDestroyOnLoad(soundManager);
                SceneManager.sceneLoaded += instance.SoundAllMute;
            }

            return instance;
        }
    }

    public static void PlaySFX(AudioClip clip)
    {
        Instance.SoundPlay(clip);
    }

    public void SoundPlay(AudioClip clip)
    {
        int playingAuds = 1;
        int notPlayingAud = 0;
        for (int i = 0; i < Instance.soundBarSFX.Length; i++)
        {
            if (Instance.soundBarSFX[i].isPlaying)
                playingAuds++;
            else
                notPlayingAud = i;
        }

        float volume = (0.3f + (0.7f / playingAuds)) * sfxVolume;
        foreach (AudioSource audioSource in Instance.soundBarSFX)
            audioSource.volume = volume;

        Instance.soundBarSFX[notPlayingAud].clip = clip;
        Instance.soundBarSFX[notPlayingAud].Play();
    }

    private void SoundAllMute(Scene scene, LoadSceneMode loadSceneMode)
    {
        SoundAllMute();
    }

    public static void PlayBGM(AudioClip clip)
    {
        Instance.soundBarBGM.clip = clip;
        Instance.soundBarBGM.Play();
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = value;

        foreach (AudioSource audioSource in Instance.soundBarSFX)
            audioSource.volume = sfxVolume;
    }

    public void SetBGMVolume(float value)
    {
        bgmVolume = value;

        Instance.soundBarBGM.volume = bgmVolume;
    }

    public float GetSFXVolume()
    {
        return sfxVolume;
    }

    public float GetBGMVolume()
    {
        return bgmVolume;
    }

    public static void SoundAllMute()
    {
        foreach (AudioSource audioSource in Instance.soundBarSFX)
            audioSource.Stop();
    }
}