using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public static SoundManager instance;
    
    public AudioSource audioSource;
    
    public AudioClip attackSound;
    public AudioClip acquisitionSound;
    public AudioClip clockSound;
    public AudioClip deadSound;
    public AudioClip doorSound;
    public AudioClip healSound;
    public AudioClip levelUpSound;
    public AudioClip alertSound;
    public AudioClip stairSound;

    private void Awake() {
        instance = this;
    }

    public void PlayAttackSound() {
        audioSource.PlayOneShot(attackSound);
    }

    public void PlayAcquisitionSound() {
        audioSource.PlayOneShot(acquisitionSound);
    }
    
    public void PlayClockSound() {
        audioSource.PlayOneShot(clockSound);
    }
    
    public void PlayDeadSound() {
        audioSource.PlayOneShot(deadSound);
    }

    public void PlayDoorSound() {
        audioSource.PlayOneShot(doorSound);
    }
    
    public void PlayHealSound() {
        audioSource.PlayOneShot(healSound);
    }
    
    public void PlayLevelUpSound() {
        audioSource.PlayOneShot(levelUpSound);
    }
    
    public void PlayAlertSound() {
        audioSource.PlayOneShot(alertSound);
    }
    
    public void PlayStairSound() {
        audioSource.PlayOneShot(stairSound);
    }
    
}