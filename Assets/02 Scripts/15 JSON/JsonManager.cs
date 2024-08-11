using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using ExcelDataReader;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class EnemyDataJson
{
    public List<MonsterData> monsterData; // Json 파일 안에 들어있는 배열의 이름
}

public class JsonManager : Singleton<JsonManager>
{
    [SerializeField] private TextAsset monsterData;
    public EnemyDataJson enemyDataJson;

    private void Start()
    {
        enemyDataJson = JsonUtility.FromJson<EnemyDataJson>(monsterData.text); // 유니티 내장 기본 Json 유틸리티
    }

    public void SavePlayerData(PlayerData playerData) // 플레이어의 정보를 JSON 파일로 저장해주는 메소드
    {
        // JSON 데이터를 저장할 폴더 경로
        string folderPath = Path.Combine(Application.persistentDataPath, "10 Data").Replace("\\", "/"); 
        
        if(!Directory.Exists(folderPath)) // 이런 폴더 경로가 없다면
            Directory.CreateDirectory(folderPath); // 경로에 폴더를 새로 생성해주기

        string json = JsonUtility.ToJson(playerData, true); // 문자열로 구성된 JSON 변수
        string filePath = Path.Combine(folderPath, "playerData.json").Replace("\\", "/"); // 저장해줄 JSON 파일명
        
        File.WriteAllText(filePath, json); // 지정된 경로에 JSON 파일 저장하기
    }

    public PlayerData LoadPlayerData() // 저장되어 있는 플레이어의 정보를 불러오는 메소드
    {
        string folderPath = Path.Combine(Application.persistentDataPath, "10 Data").Replace("\\", "/");
        string filePath = Path.Combine(folderPath, "playerData.json").Replace("\\", "/"); // 저장해줄 JSON 파일명
        
        if(!File.Exists(filePath)) // 기존에 저장된 파일이 없으면
            return null;

        string json = File.ReadAllText(filePath);
        PlayerData playerData = JsonUtility.FromJson<PlayerData>(json); // 읽어온 JSON을 객체로 다시 만들어주기
        
        return playerData;
    }

    public void SaveInventoryData(List<InventoryData> inventoryDataList) // 인벤토리의 아이템 정보를 JSON 파일로 저장해주는 메소드
    {
        string folderPath = Path.Combine(Application.persistentDataPath, "10 Data").Replace("\\", "/");
        string json = JsonConvert.SerializeObject(inventoryDataList, Formatting.Indented);
        string filePath = Path.Combine(folderPath, "inventoryData.json").Replace("\\", "/");
        
        File.WriteAllText(filePath, json); // 지정된 경로에 JSON 파일 저장하기
    }

    public List<InventoryData> LoadInventoryData()
    {
        string folderPath = Path.Combine(Application.persistentDataPath, "10 Data").Replace("\\", "/");
        string filePath = Path.Combine(folderPath, "inventoryData.json").Replace("\\", "/"); // 저장해줄 JSON 파일명
        
        if(!File.Exists(filePath)) // 기존에 저장된 파일이 없으면
            return null;

        string json = File.ReadAllText(filePath);
        List<InventoryData> playerData = JsonConvert.DeserializeObject<List<InventoryData>>(json); // 읽어온 JSON을 객체로 다시 만들어주기
        
        return playerData;
    }
    
    public List<Dictionary<string, object>> LoadExcel(string filePath) // 엑셀 파일을 불러오는 메소드
    {
        FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read); // 엑셀파일 열기
        IExcelDataReader reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
        DataSet dataSet = reader.AsDataSet(); // 엑셀 파일에서 데이터 읽기
        reader.Close();
        
        List<Dictionary<string, object>> excelDataList = new List<Dictionary<string, object>>(); // 엑셀 파일을 JSON으로 변환
        DataTable table = dataSet.Tables[0]; // 엑셀의 첫번째 시트

        for (int i = 1; i < table.Rows.Count; i++) // 엑셀의 시트의 row 갯수   // 최소 2줄은 있어야 헤더랑 값이 나온다
        {
            DataRow row = table.Rows[i];

            Dictionary<string, object> rowData = new Dictionary<string, object>();
            for (int j = 0; j < table.Columns.Count; j++) // 엑셀 시트의 열 갯수
            {
                string key = table.Rows[0][j].ToString(); // 키 값은 항상 첫번째 row(헤더)로 고정
                object value = row[j];
                rowData[key] = value;
            }
            excelDataList.Add(rowData);
        }

        return excelDataList;
    }
    
}