using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioClipName
{
    Attack, Acquisition, Clock, Dead, Door, Heal, LevelUp, Alert, Talk, Stair, FirePlace,
    Water, Click, Buy, Plus, Consume, Equip, Quest, Panel, Quit, Key
}

public class SoundManager : MonoBehaviour
{

    public static SoundManager instance;

    [SerializeField]
    private AudioClip[] audioClips;
    
    private Queue<AudioSource> audioSourceQueue;
    
    [SerializeField]
    private AudioSource audioSourcePrefab;
    
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
    public AudioClip buySound;
    public AudioClip plusSound;
    public AudioClip consumeSound;
    public AudioClip equipSound;
    public AudioClip questSound;
    public AudioClip panelSound;
    public AudioClip quitSound;
    public AudioClip keySound;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        audioSourceQueue = new Queue<AudioSource>();

        audioSourceA = gameObject.AddComponent<AudioSource>();
        audioSourceB = gameObject.AddComponent<AudioSource>();
        audioSourceA.playOnAwake = false;
        audioSourceB.playOnAwake = false;
    }

    private void PlaySound(AudioClipName audioClipName)
    {

        if(audioSourceQueue.Count < 1) // 모든 Audio Source가 사용중인 상태이면
        {
            GenerateAudioSource(); // 새로운 Audio Source를 생성해주기
        }
        
        AudioSource remainAudioSource = audioSourceQueue.Dequeue();
        remainAudioSource.clip = audioClips[(int)audioClipName];
        remainAudioSource.gameObject.SetActive(true);
        remainAudioSource.Play();
        
        StartCoroutine(RetrieveAudioSource(remainAudioSource, audioClips[(int)audioClipName]));
    }

    private IEnumerator RetrieveAudioSource(AudioSource audioSource, AudioClip audioClip)
    {
        yield return new WaitForSeconds(audioClip.length); // AudioClip이 전부 재생되기를 기다렸다가
        audioSource.gameObject.SetActive(false); // Audio Source 꺼주기
        audioSourceQueue.Enqueue(audioSource);
    }

    private void GenerateAudioSource()
    {
        AudioSource newAudioSource = Instantiate(audioSourcePrefab, transform);
        audioSourceQueue.Enqueue(newAudioSource);
    }

    public void PlayKeySound() {
        audioSourceA.PlayOneShot(keySound);
    }
    
    public void PlayQuitSound() {
        audioSourceA.PlayOneShot(quitSound);
    }

    public void PlayPanelSound() {
        audioSourceA.PlayOneShot(panelSound);
    }

    public void PlayQuestSound() {
        audioSourceA.PlayOneShot(questSound);
    }
    
    public void PlayEquipSound() {
        audioSourceA.PlayOneShot(equipSound);
    }

    public void PlayConsumeSound() {
        audioSourceA.PlayOneShot(consumeSound);
    }

    public void PlayPlusSound() {
        audioSourceA.PlayOneShot(plusSound);
    }

    public void PlayBuySound() {
        audioSourceA.PlayOneShot(buySound);
    }
    
    public void PlayClickSound() {
        audioSourceA.PlayOneShot(clickSound);
    }
    
    public void PlayTalkSound() {
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