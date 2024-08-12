using System.Collections.Generic;
using System.Data;
using System.IO;
using ExcelDataReader;
using Newtonsoft.Json;
using UnityEngine;

public class ExcelToJson
{
    
    public static string ConvertExcelToJson(string filePath)
    {
        FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
        IExcelDataReader reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
        DataSet dataSet = reader.AsDataSet(); // 엑셀 파일에서 데이터 읽기
        reader.Close();
        
        List<Dictionary<string, object>> jsonData = new List<Dictionary<string, object>>(); // 엑셀 파일을 JSON으로 변환
        DataTable table = dataSet.Tables[0]; // 엑셀의 첫번째 시트
        Debug.Log("table: " + table);
        Debug.Log("table.Rows.Count: " + table.Rows.Count);
        Debug.Log("table.Columns.Count: " + table.Columns.Count);

        for (int i = 1; i < table.Rows.Count; i++) // 엑셀의 시트의 row 갯수   // 최소 2줄은 있어야 헤더랑 값이 나온다
        {
            DataRow row = table.Rows[i];
            Debug.Log("row: " + row);
            
            Dictionary<string, object> rowData = new Dictionary<string, object>();
            for (int j = 0; j < table.Columns.Count; j++) // 엑셀 시트의 열 갯수
            {
                string key = table.Rows[0][j].ToString(); // 키 값은 항상 첫번째 row(헤더)로 고정
                object value = row[j];
                
                Debug.Log("key: " + key);
                Debug.Log("value: " + value);
                
                rowData[key] = value;
            }
            jsonData.Add(rowData);
        }
        
        return JsonConvert.SerializeObject(jsonData, Formatting.Indented);
    }

}