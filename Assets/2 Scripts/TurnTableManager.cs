using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TurnTableManager : MonoBehaviour {

    public static TurnTableManager instance;

    public delegate void ChangeTurnTablePanel();
    public ChangeTurnTablePanel changeTurnTablePanel;
    
    public Transform turnTableSlotHolder;
    public TurnTableSlot[] turnTableSlots;
    public GameObject turnTableSlotPrefab;
    public AudioSource turnTablePlayer;
    public int turnTableSlotNum;

    public AudioClip[] audioClips;

    private void Awake() {
        instance = this;
        turnTablePlayer = gameObject.AddComponent<AudioSource>();
        changeTurnTablePanel += RedrawTurnTablePanel;
        GenerateTurnTableSlots();
    }

    public void PlayTurnTable(int num) { // 턴테이블을 재생해주는 메소드
        turnTablePlayer.Stop(); // 먼저 재생중일 수 있으니 정지시켜주고
        turnTablePlayer.PlayOneShot(audioClips[num]); // 선택한 음반을 재생해주기
    }

    public void StopTurnTable() {
        turnTablePlayer.Stop();
    }

    private void GenerateTurnTableSlots() {
        for(int i = 0; i < turnTableSlotNum; i++) {
            Instantiate(turnTableSlotPrefab, turnTableSlotHolder);
        }
        turnTableSlots = turnTableSlotHolder.GetComponentsInChildren<TurnTableSlot>();
        changeTurnTablePanel.Invoke();
    }

    private void RedrawTurnTablePanel() {

        int idx = 0;

        for(int i = 0; i < turnTableSlotNum; i++) {
            turnTableSlots[i].gameObject.SetActive(false); // 우선 모든 음반 슬롯을 꺼주기
        }

        for(int i = 0; i < Inventory.instance.recordCnt; i++) {
            turnTableSlots[i].gameObject.SetActive(true); // 플레이어가 인벤토리에 보유한 음반의 갯수만큼 슬롯을 켜주기
        }

        for(int i = 0; i < Inventory.instance.possessItems.Count; i++) {

            if(Inventory.instance.possessItems[i].itemType == ItemType.Record) {
                Item item = Inventory.instance.possessItems[i];
                turnTableSlots[idx].recordNum = item.recordNum;
                string recordName = item.itemName.ToString();
                Text recordText = turnTableSlots[idx].GetComponentInChildren<Text>();
                recordText.text = recordName;
                idx++;
            }
        }
    }
    
}