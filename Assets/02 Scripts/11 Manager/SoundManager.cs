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

    private Queue<AudioSource> audioSourceQueue;
    
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private AudioSource audioSourcePrefab;

    private void Awake()
    {
        instance = this;
        //DontDestroyOnLoad(gameObject);
        audioSourceQueue = new Queue<AudioSource>();
    }

    public void PlaySound(AudioClipName audioClipName, bool loop = false)
    {
        if(audioSourceQueue.Count < 1) // 모든 Audio Source가 사용중인 상태이면
            GenerateAudioSource(); // 새로운 Audio Source를 생성해주기

        AudioSource remainAudioSource = audioSourceQueue.Dequeue();
        remainAudioSource.clip = audioClips[(int)audioClipName];
        remainAudioSource.loop = loop;
        remainAudioSource.gameObject.SetActive(true);
        remainAudioSource.Play();

        if(!loop) // 반복 재생이 아닌 경우에만 자동으로 Audio Source를 꺼주기
            StartCoroutine(RetrieveAudioSource(remainAudioSource, audioClips[(int)audioClipName]));
    }
    
    public void StopSound()
    {
        AudioSource[] childAudioSource = GetComponentsInChildren<AudioSource>();
        for(int i = 0; i < childAudioSource.Length; i++)
        {
            if(childAudioSource[i].loop && childAudioSource[i].gameObject.activeSelf)
            {
                childAudioSource[i].loop = false;
                childAudioSource[i].gameObject.SetActive(false);
                audioSourceQueue.Enqueue(childAudioSource[i]);
            }
        }
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

}