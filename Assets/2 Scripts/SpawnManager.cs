using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    public static SpawnManager instance;
    
    public GameObject enemyPrefab;
    public Transform enemyParent;
    public Transform[] spawnPos;
    public GameObject[] enemies;
    
    private void Awake() {

        enemies = new GameObject[spawnPos.Length];

        for(int i = 0; i < spawnPos.Length; i++) {
            GameObject enemy = Instantiate(enemyPrefab, spawnPos[i].position, quaternion.identity, enemyParent);
            enemy.SetActive(false); // 생성하자마자 일단 전부 꺼두기
            enemies[i] = enemy;
        }
    }

    public void GenerateEnemy() {
        for(int i = 0; i < spawnPos.Length; i++) {
            enemies[i].SetActive(true); // 꺼두었던 Enemy를 전부 켜주기
        }
    }
    
}