using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TurnTableManager : MonoBehaviour {

    public AudioSource turnTablePlayer;

    public AudioClip[] audioClips;

    private void Awake() {
        turnTablePlayer.AddComponent<AudioSource>();
    }
    
    
}