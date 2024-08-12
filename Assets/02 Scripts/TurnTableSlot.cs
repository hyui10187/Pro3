using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTableSlot : MonoBehaviour {

    public int recordNum;

    public void PlayTurnTable() {
        TurnTableManager.instance.PlayTurnTable(recordNum);
        SoundManager.instance.PlaySound(AudioClipName.Click);
    }
    
}