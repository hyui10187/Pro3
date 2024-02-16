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
    public int enemyCount;
    
    private void Awake() {

        instance = this;
        enemies = new GameObject[spawnPos.Length];

        for(int i = 0; i < spawnPos.Length; i++) { // 스폰 위치마다 몬스터를 1마리씩 생성해주기
            GameObject enemy = Instantiate(enemyPrefab, spawnPos[i].position, quaternion.identity, enemyParent);
            enemy.SetActive(false); // 생성하자마자 일단 전부 꺼두기
            enemies[i] = enemy;
        }

        enemyCount = enemies.Length; // 몬스터의 숫자는 몬스터가 생성된 숫자만큼
    }

    public void GenerateEnemy() {
        for(int i = 0; i < enemies.Length; i++) {
            enemies[i].SetActive(true); // 꺼두었던 Enemy를 전부 켜주기
        }
    }

}