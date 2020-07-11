using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager instance;
    [SerializeField]
    private List<AudioSource> allSources = new List<AudioSource> { };
    public AudioSource musicSource;
    public AudioClip battleMusicClip;
    public AudioClip calmMusicClip;
    public AudioClip victoryMusicClip;

    public float audioVolume = 1f;
    public float musicVolume = 1f;

    public GameObject muted;

    void Awake () {
        if (instance == null) {
            instance = this;
        } else {
            Destroy (this);
        }
    }

    public void ResetSources (bool onlyNulls = false) {
        if (!onlyNulls) {
            for (int i = 0; i < allSources.Count; i++) {
                if (allSources[i] == null) {
                    allSources.RemoveAt (i);
                }
            }
        } else {
            allSources.RemoveAll (x => x == null);
        }
        SetAudioVolume (audioVolume);
        SetMusicVolume (musicVolume);
    }
    public void AddSource (AudioSource source) { // Automagically happens on initialization of audio source
        if (!allSources.Contains (source)) {
            allSources.Add (source);
            source.volume = audioVolume;
        }
    }
    public void RemoveSource (AudioSource source) {
        if (allSources.Contains (source)) {
            allSources.Remove (source);
        }
    }

    public void SetAudioVolume (float volume) {
        foreach (AudioSource source in allSources) {
            if (source != null) {
                source.volume = volume;
            };
        }
        if (muted != null) {
            if (volume == 0f) {
                muted.SetActive (true);
            } else {
                muted.SetActive (false);
            }
        };
        audioVolume = volume;
    }
    public void SetAudioVolumeReverse (float volume) {
        SetAudioVolume (1f - volume);
    }
    public void SetMusicVolumeReverse (float volume) {
        SetMusicVolume (1f - volume);
    }
    public void SetMusicVolume (float volume) {
        if (musicSource != null) {
            musicSource.volume = volume;
        }
        musicVolume = volume;
    }

    public IEnumerator FadeInOutMusic (AudioClip newClip) {
        while (musicSource.volume > 0f) {
            musicSource.volume -= Time.deltaTime * 2f;
            yield return null;
        }
        musicSource.clip = newClip;
        musicSource.Play ();
        while (musicSource.volume < 1f) {
            musicSource.volume += Time.deltaTime * 2f;
            yield return null;
        }
    }

    public void PlayBattleMusic () {
        if (musicSource.clip != battleMusicClip) {
            StartCoroutine (FadeInOutMusic (battleMusicClip));
        };
    }
    public void PlayCalmMusic () {
        if (musicSource.clip != calmMusicClip) {
            StartCoroutine (FadeInOutMusic (calmMusicClip));
        };
    }
    public void PlayVictoryMusic () {
        if (musicSource.clip != victoryMusicClip) {
            StartCoroutine (FadeInOutMusic (victoryMusicClip));
        };
    }
}