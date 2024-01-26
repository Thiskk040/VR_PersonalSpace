using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.XR.Oculus;


public class MoveWayPoint : MonoBehaviour
{
    public List<GameObject> waypoints;
    public GameObject secondCapsulePrefab; // Prefab of the second capsule
    public GameObject thirdCapsulePrefab; // Prefab of the third capsule
    public float speed = 1;
    public float waitTime = 10f;
    public float timeTotalOfTask;
    public string csv_name = "player_";
    private int currentIndex = 0;
    private float waitTimer = 0f;
    private bool isWaiting = false;
    private bool canMove = true;
    //public GameObject waypoint1Canvas;
    

    private List<string[]> rowData = new List<string[]>();

    void Start()
    {

        MoveToWaypoint();
        StartCoroutine(save());
    }

    void Update()
    {
        if (canMove)
        {
            if (!isWaiting)
            {
                MoveToWaypoint();

            }
            else
            {
                WaitAtWaypoint();

            }
        }

        // Check for button press
        if(OVRInput.GetDown(OVRInput.Button.One))
        {
            if (canMove)
            {
                StopMoving();
            }
            else
            {
                ResumeMoving();
            }
        }
    }

    void MoveToWaypoint()
    {
        Vector3 destination = waypoints[currentIndex].transform.position;
        Vector3 newPos = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        transform.position = newPos;

        float distance = Vector3.Distance(transform.position, destination);
        if (distance <= 0.05)
        {
            // Reached the waypoint, start waiting
            isWaiting = true;
            waitTimer = waitTime;

            // Check if this is the last waypoint
            if (currentIndex == waypoints.Count - 1)
            {
                // Destroy the current capsule
                Destroy(gameObject);

                // Spawn the second capsule at the first waypoint
                GameObject secondCapsule = Instantiate(secondCapsulePrefab, waypoints[0].transform.position, Quaternion.identity);
                MoveWayPoint moveScript2 = secondCapsule.AddComponent<MoveWayPoint>();
                moveScript2.waypoints = waypoints; // Use the same waypoints as the previous capsule
                moveScript2.secondCapsulePrefab = thirdCapsulePrefab; // Pass the prefab of the third capsule
    
            }
       
        }
    }


    void WaitAtWaypoint()
    {
        waitTimer -= Time.deltaTime;

        if (waitTimer <= 0f)
        {
            // Finished waiting, move to the next waypoint
            currentIndex = (currentIndex + 1) % waypoints.Count;
            isWaiting = false;
        }
    }

    public void StopMoving()
    {
        canMove = false;
        Debug.Log($"{gameObject.name} has Stopped at timestamp: " + Time.time + ", Position: " + transform.position.x);

    }

    void ResumeMoving()
    {
        canMove = true;
    }

    IEnumerator save()
    {
        while (true)
        {
            SaveData();

            yield return new WaitForSeconds(0.2f);
        }
    }

    public string[] rowDataTemp = new string[6];

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
        rowDataTemp[4] = (transform.position.y).ToString();
        rowDataTemp[5] = (transform.position.z).ToString();
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


        string filePath = Application.dataPath + "/CSV/" + csv_name + SetUp.playerName + ".csv";


        StreamWriter outStream = System.IO.File.AppendText(filePath);
        outStream.Write(sb);
        outStream.Close();

        Debug.Log(filePath);

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

