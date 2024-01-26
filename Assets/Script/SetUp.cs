using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System.Text;



public struct TaskConfig
{
    public List<Sprite> Image;
    public Sprite Final;
}
public class SetUp : MonoBehaviour
{

    public static string playerName;

    public Toggle isTask1;
    public Toggle isTask2;
    public Toggle isTask3;
    static public Dictionary<int, List<TaskConfig>> taskDic;
    public List<TaskConfig> task1;
    public List<TaskConfig> task2;
    public List<TaskConfig> task3;
    public Text nameText;

    public static float positionX;

    public void Awake()
    {
        taskDic = new Dictionary<int, List<TaskConfig>>();
        taskDic[1] = task1;
        taskDic[2] = task2;
        taskDic[3] = task3;

        DontDestroyOnLoad(this.gameObject);
    }

    public void setcontroller(string name)
    {
        
        playerName = nameText.text.ToString();
        if (isTask1.isOn)
        {
            SceneManager.LoadScene(1);
            playerName += "_task1";
        }
        else if(isTask2.isOn)
        {
            SceneManager.LoadScene(2);
            playerName += "_task2";
        }
        else if(isTask3.isOn)
        {
            SceneManager.LoadScene(3);
            playerName += "_task3";
        }
        CreatePlayerCsv("player");

    }




    public void CreatePlayerCsv(string name)
    {
        List<string[]> rowData = new List<string[]>();

        // Creating First row of titles manually..
        string[] rowDataTemp = new string[6];
        rowDataTemp[0] = "Date";
        rowDataTemp[1] = "time_from_start";
        rowDataTemp[2] = "Character_type";
        rowDataTemp[3] = "Character_position.x";
        rowDataTemp[4] = "Character_position.y";
        rowDataTemp[5] = "Character_position.z";
        rowData.Add(rowDataTemp);

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


        string filePath = Application.dataPath + "/CSV/" + name + "_" + SetUp.playerName + ".csv";
        Debug.Log(filePath);

        StreamWriter outStream = System.IO.File.CreateText(filePath);
        outStream.Write(sb);
        outStream.Close();

    }
}
