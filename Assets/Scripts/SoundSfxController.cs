using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSfxController : MonoBehaviour
{
    AudioSource audioSource;
    public static SoundSfxController instance;

    public AudioClip click;
    public AudioClip stepGrass;
    public AudioClip stepWood;
    public AudioClip cameraShutter;
    public AudioClip photoBoardClip;
    public AudioClip okSound;
    public AudioClip wrongSound;
    public AudioClip fadeInSound;


    private void Awake() {
        if (instance == null) {
            instance = this;
            audioSource = GetComponent<AudioSource>();
            audioSource.volume = 0.95f;
            DontDestroyOnLoad(this);
        }
        else {
            Destroy(this);
        }
    }


    public void PlayClick() {
        audioSource.pitch = 1;
        audioSource.PlayOneShot(click);
    }

    public void PlayStepGrass() {
        audioSource.pitch = 1;
        audioSource.PlayOneShot(stepGrass);
    }

    public void PlayStepWood() {
        audioSource.pitch = 1;
        audioSource.PlayOneShot(stepWood);
    }

    public void PlayCameraShutter() {
        audioSource.pitch = 1;
        audioSource.PlayOneShot(cameraShutter);
    }

    public void PlayPhotoBoardClip() {
        audioSource.pitch = 1;
        audioSource.PlayOneShot(photoBoardClip);
    }
    public void PlayOkSound() {
        audioSource.pitch = 1;
        audioSource.PlayOneShot(okSound);
    }
    public void PlayWrongSound() {
        audioSource.pitch = 1;
        audioSource.PlayOneShot(wrongSound);
    }
    public void PlayFadeInSound() {
        audioSource.pitch = 1;
        audioSource.PlayOneShot(fadeInSound);
    }


}
