using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;
using UnityEngine.UI;

public class TimeRecord : MonoBehaviour
{
    private List<string[]> rowData = new List<string[]>();
    public string csv_name = "player_";


    void Start()
    {
        StartCoroutine(save());
    }


    IEnumerator save()
    {
        while (true)
        {
            SaveData();

            yield return new WaitForSeconds(0.2f);
        }
    }

    public string[] rowDataTemp = new string[4];

    public void SaveData()
    {
        rowData = new List<string[]>();

        // Creating First row of titles manually..

        DateTime serverTime = DateTime.Now;
        long unixTime = ((DateTimeOffset)serverTime).ToUnixTimeMilliseconds();

        rowDataTemp[0] = unixTime.ToString();
        rowDataTemp[1] = Time.time.ToString();
        rowDataTemp[2] = gameObject.name.ToString();
        rowDataTemp[3] = (transform.position.x).ToString();

        string[][] output = new string[rowData.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = rowData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < length; index++)
            sb.AppendLine(string.Join(delimiter, output[index]));


        string filePath = Application.dataPath + "/CSV/" + csv_name + SetUp.playerName + ".csv";
        Debug.Log(filePath);

        StreamWriter outStream = System.IO.File.AppendText(filePath);
        outStream.Write(sb);
        outStream.Close();

    }




    // Following method is used to retrive the relative path as device platform
    private string getPath()
    {
#if UNITY_EDITOR
        return Application.dataPath + "/CSV/" + csv_name + SetUp.playerName + ".csv";
#elif UNITY_ANDROID
        return Application.persistentDataPath + "/Saved_data.csv";
#elif UNITY_IPHONE
        return Application.persistentDataPath + "/"+"Saved_data.csv";
#else
        return "/"+"Saved_data.csv";
#endif
        //Debug.Log("get path leido");
    }
}
