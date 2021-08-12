using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Prueba : MonoBehaviour {

    public Text console;

    void Start()
    {

        if (Application.isMobilePlatform)
        {
            console.text = ("Aplicación Andorid:\n");
            console.text += FileProcessing.androidGetMissionById("ARG_1")._steps.ToString();
        }
        else
        {
            console.text = ("Aplicacion de PC:\n");
            console.text += ReadString();
        }

    }


    static string ReadString()
    {
        string path = "Assets/StreamingAssets/" + "ARG_1.txt";
        string res = "";
        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        res = reader.ReadToEnd();
        reader.Close();
        return res;
    }

    static string AndroidReadString()
    {
        string res = " NONE ";


        if (Application.platform == RuntimePlatform.Android)
        {
            // Android
            string oriPath = System.IO.Path.Combine(Application.streamingAssetsPath, "ARG_1.txt");

            res = oriPath;

            // Android only use WWW to read file
            WWW reader = new WWW(oriPath);
            while (!reader.isDone) { }

           res = reader.text;
        }

        return res;

    }

}
