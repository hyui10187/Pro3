using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public static SoundManager instance;
    
    public AudioSource audioSourceA;
    public AudioSource audioSourceB;
    
    public AudioClip attackSound;
    public AudioClip acquisitionSound;
    public AudioClip clockSound;
    public AudioClip deadSound;
    public AudioClip doorSound;
    public AudioClip healSound;
    public AudioClip levelUpSound;
    public AudioClip alertSound;
    public AudioClip talkSound;
    public AudioClip stairSound;
    public AudioClip fireplaceSound;
    public AudioClip waterSound;
    public AudioClip clickSound;

    private void Awake() {
        instance = this;
        audioSourceA = gameObject.AddComponent<AudioSource>();
        audioSourceB = gameObject.AddComponent<AudioSource>();
        audioSourceA.playOnAwake = false;
        audioSourceB.playOnAwake = false;
    }
    
    public void ClickSound() {
        audioSourceA.PlayOneShot(clickSound);
    }
    
    public void TalkSound() {
        audioSourceA.PlayOneShot(talkSound);
    }
    
    public void PlayWaterSound() {
        audioSourceB.clip = waterSound;
        audioSourceB.Play();
        audioSourceB.loop = true;
    }

    public void StopWaterSound() {
        audioSourceB.Stop();
        audioSourceB.clip = null;
        audioSourceB.loop = false;
    }
    
    public void PlayFireplaceSound() {
        audioSourceB.clip = fireplaceSound;
        audioSourceB.Play();
        audioSourceB.loop = true;
    }

    public void StopFireplaceSound() {
        audioSourceB.Stop();
        audioSourceB.clip = null;
        audioSourceB.loop = false;
    }

    public void PlayAttackSound() {
        audioSourceA.PlayOneShot(attackSound);
    }

    public void PlayAcquisitionSound() {
        audioSourceA.PlayOneShot(acquisitionSound);
    }
    
    public void PlayClockSound() {
        audioSourceA.clip = clockSound;
        audioSourceA.Play();
        audioSourceA.loop = true;
    }
    
    public void StopClockSound() {
        audioSourceA.Stop();
        audioSourceA.clip = null;
        audioSourceA.loop = false;
    }
    
    public void PlayDeadSound() {
        audioSourceA.PlayOneShot(deadSound);
    }

    public void PlayDoorSound() {
        audioSourceA.PlayOneShot(doorSound);
    }
    
    public void PlayHealSound() {
        audioSourceA.PlayOneShot(healSound);
    }
    
    public void PlayLevelUpSound() {
        audioSourceA.PlayOneShot(levelUpSound);
    }
    
    public void PlayAlertSound() {
        audioSourceA.PlayOneShot(alertSound);
    }
    
    public void PlayStairSound() {
        audioSourceA.PlayOneShot(stairSound);
    }
    
}