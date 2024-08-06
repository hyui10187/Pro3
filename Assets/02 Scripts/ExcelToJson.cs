using System.Collections.Generic;
using System.Data;
using System.IO;
using ExcelDataReader;
using Newtonsoft.Json;

public class ExcelToJson
{
    
    public static string ConvertExcelToJson(string filePath)
    {
        FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
        IExcelDataReader reader;
        
        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
        
        DataSet dataSet = reader.AsDataSet(); // 엑셀 파일에서 데이터 읽기
        reader.Close();
        
        List<Dictionary<string, object>> jsonData = new List<Dictionary<string, object>>(); // 엑셀 파일을 JSON으로 변환
        DataTable table = dataSet.Tables[0];
        for (int i = 1; i < table.Rows.Count; i++)
        {
            DataRow row = table.Rows[i];
            Dictionary<string, object> rowData = new Dictionary<string, object>();
            for (int j = 0; j < table.Columns.Count; j++)
            {
                string key = table.Rows[0][j].ToString();
                object value = row[j];
                rowData[key] = value;
            }
            jsonData.Add(rowData);
        }
        
        return JsonConvert.SerializeObject(jsonData, Formatting.Indented);
    }

}