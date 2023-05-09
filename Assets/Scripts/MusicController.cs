using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    AudioSource audioSource;
    public static MusicController instance;
    public AudioClip savannaMusic;
    public float secondsToFadeOut = 1;

    private void Awake() {
        if (instance == null) {
            instance = this;
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = savannaMusic;
            audioSource.loop = true;
            audioSource.playOnAwake = true;
            DontDestroyOnLoad(this);
        }
        else {
            Destroy(this);
        }
    }

    private void Start() {
        audioSource.Play();
    }


    public void StopMainSong() {
        StartCoroutine(FadeOutSong());
    }


    IEnumerator FadeOutSong() {
        // Check Music Volume and Fade Out
        while (audioSource.volume > 0.01f) {
            audioSource.volume -= Time.deltaTime / secondsToFadeOut;
            yield return null;
        }

        // Make sure volume is set to 0
        audioSource.volume = 0;

        // Stop Music
        audioSource.Stop();
    }

    public void DebugLowVolume() {
        audioSource.volume = 0f;
    }
}
