using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{

    private static Dictionary<string, AudioSource> AudioSources;

    void Awake()
    {
        Initialise();
    }

    void OnDestroy()
    {
        AudioSources.Clear();
        AudioSources = null;
    }

    void Initialise()
    {
        if (AudioSources == null)
        {
            AudioSources = new Dictionary<string, AudioSource>();

            AudioSource[] sources = gameObject.GetComponentsInChildren<AudioSource>();
            for (int i = 0; i < sources.Length; i++)
            {
                AudioSources[sources[i].gameObject.name] = sources[i];
                DontDestroyOnLoad(transform.parent);
            }
        }
    }

    public static void PlayAssimilationSound()
    {
        AudioSources["Assimilation"].Play();
    }

    public static void PlayCannonballStunSound()
    {
        AudioSources["CannonStun"].Play();
    }

    public static void PlayCannonballIntoWallSound()
    {
        AudioSources["CannonHitWall"].Play();
    }

    public static void PlayCannonballFireSound()
    {
        AudioSources["CannonShoot"].Play();
    }

    public static void PlayGameAlmostOverSound()
    {
        AudioSources["Timer"].Play();
    }

    public static void StopGameAlmostOverSound()
    {
        if (AudioSources["Timer"].isPlaying)
        {
            AudioSources["Timer"].Stop();
        }
    }

    public static void PlayGameOverSound()
    {
        AudioSources["GameOver"].Play();
    }

    public static void PlayBlinkSound()
    {
        AudioSources["Blink"].Play();
    }

    public static void PlayCloneSound()
    {
        AudioSources["Blink"].Play();
    }

    public static void StartMenuMusic()
    {
        if (AudioSources["LevelMusic"].isPlaying)
        {
            AudioSources["LevelMusic"].Stop();
        }
        AudioSources["AnotherTune"].Play();
    }

    public static void StartLevelMusic()
    {
        if (AudioSources["AnotherTune"].isPlaying)
        {
            AudioSources["AnotherTune"].Stop();
        }
        AudioSources["LevelMusic"].Play();
    }

    /// <summary>
    /// Pauses music for level and plays music for menu
    /// </summary>
    public static void PauseLevelWithAudio()
    {
        if (AudioSources["LevelMusic"].isPlaying)
        {
            AudioSources["LevelMusic"].Pause();
        }
        AudioSources["AnotherTune"].Play();
    }

    /// <summary>
    /// Unpauses level music and stops menu music
    /// </summary>
    public static void UnPauseLevelWithAudio()
    {
        if (AudioSources["AnotherTune"].isPlaying)
        {
            AudioSources["AnotherTune"].Stop();
        }
        AudioSources["LevelMusic"].UnPause();
    }

}

