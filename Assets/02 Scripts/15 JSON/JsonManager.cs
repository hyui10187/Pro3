using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class MonsterData
{
    // Json의 배열 안에 들어있는 속성들
    public float maxHealth; // 최대 체력
    public float exp; // 경험치
    public int collisionDamage; // 충돌 데미지
}

[Serializable]
public class EnemyDataJson
{
    public List<MonsterData> monsterData; // Json 파일 안에 들어있는 배열의 이름
}

public class JsonManager : Singleton<JsonManager>
{
    private class ExampleData
    {
        public string name;
        public int age;
    }
    
    
    [SerializeField] private TextAsset monsterData;

    public EnemyDataJson enemyDataJson;

    private void Start()
    {
        enemyDataJson = JsonUtility.FromJson<EnemyDataJson>(monsterData.text); // 유니티 내장 기본 Json 유틸리티

        GetExcel();
    }

    public void SavePlayerData(PlayerData playerData) // 플레이어의 정보를 JSON 파일로 저장해주는 메소드
    {
        // JSON 데이터를 저장할 폴더 경로
        string folderPath = Path.Combine(Application.dataPath, "10 Data").Replace("\\", "/"); 
        
        if(!Directory.Exists(folderPath)) // 이런 폴더 경로가 없다면
            Directory.CreateDirectory(folderPath); // 경로에 폴더를 새로 생성해주기

        string json = JsonUtility.ToJson(playerData, true); // 문자열로 구성된 JSON 변수
        string filePath = Path.Combine(folderPath, "playerData.json").Replace("\\", "/"); // 저장해줄 JSON 파일명
        
        File.WriteAllText(filePath, json); // 지정된 경로에 JSON 파일 저장하기
    }

    public PlayerData LoadPlayerData() // 저장되어 있는 플레이어의 정보를 불러오는 메소드
    {
        string folderPath = Path.Combine(Application.dataPath, "10 Data").Replace("\\", "/");
        string filePath = Path.Combine(folderPath, "playerData.json").Replace("\\", "/"); // 저장해줄 JSON 파일명
        
        if(!File.Exists(filePath)) // 기존에 저장된 파일이 없으면
            return null;

        string json = File.ReadAllText(filePath);
        PlayerData playerData = JsonUtility.FromJson<PlayerData>(json); // 읽어온 JSON을 객체로 다시 만들어주기
        
        return playerData;
    }

    private void GetExcel()
    {
        string folderPath = Path.Combine(Application.dataPath, "ABC");
        string filePath = Path.Combine(folderPath, "example.xlsx");
        string json = ExcelToJson.ConvertExcelToJson(filePath);
        
        string jsonFilePath = Path.Combine(folderPath, "example.json");
        File.WriteAllText(jsonFilePath, json);
    }
    
}