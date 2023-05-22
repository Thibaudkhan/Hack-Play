using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WireShark : MonoBehaviour
{

    
    public string ShowPacketLog(string file = "log1.txt")
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, file);
        string logString = "";
        try
        {
            string[] lines = File.ReadAllLines(filePath);
            // convert lines to a string
            logString = string.Join("\n", lines);
            // print the string
            // or iterate through the lines

        }
        catch (FileNotFoundException)
        {
            logString = "File not found";
        }

        return logString;
    }

}
