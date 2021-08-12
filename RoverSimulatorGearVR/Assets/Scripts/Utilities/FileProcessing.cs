using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class FileProcessing {
	
	public static string readFile(string pathFile)
	{
		string line;
		string fileText = "";

		System.IO.StreamReader file = new System.IO.StreamReader (pathFile);

		while((line = file.ReadLine()) != null)
		{
				fileText += line + "\n";
		}
		file.Close ();

		return fileText;
	}

	public static void writeFile(string pathFile,string text)
	{
		System.IO.File.WriteAllText(pathFile,text);	
	}

	public static Mission readPath(string path)
	{
		Mission newPath = new Mission ();

        string[] splitPath = path.Split ('\\');
		string[] splitName = splitPath [splitPath.Length - 1].Split ('.');
		// Id de la mission
		newPath._id = splitName [0];

		string file = readFile (path);
		string [] values = file.Split('#');
		string []strLenght = values [1].Split (' ');
		// Distancia recorrida
		newPath._length = float.Parse(strLenght [1]);

		string []strSteps1 = values [7].Split (' ');
		string[] strSteps2 = strSteps1 [1].Split ('\n');
		// Numeros de pasos
		newPath._steps  = int.Parse(strSteps2[0]);

		string[] strAllSteps = file.Split (':');
		string[] strPoints = strAllSteps[8].Split ('\n');

		// Ruta
		for (int i = 1; i < strPoints.Length-1; i++)
		{
			string[] strCoordiantes = strPoints [i].Split (' ');
			//rtn += strCoordiantes [0] + "-" + strCoordiantes[1] + "-" + strCoordiantes[2] + "\n";
			float x = float.Parse (strCoordiantes[0]);
			float y = float.Parse (strCoordiantes[1]);
			float z = float.Parse (strCoordiantes[2]);
			Vector3 point = new Vector3 (x, y, z);
			newPath._path.Add (point);
		}
		return newPath;
	}


	public static Mission[] getMissionsByMapId(string id){

        Mission[] missions;

		try
		{
			string[] missionsPath = Directory.GetFiles (".\\PathPlanning\\Paths",id + "*");
			missions = new Mission[missionsPath.Length];
			for(int i = 0; i < missionsPath.Length; i++)
			{
				missions[i] = readPath(missionsPath[i]);
			}
			return missions;
		}
		catch(Exception e)
		{
			Debug.Log ("Exception: "+ e.ToString());
			return null;
		}
	}

    public static Mission androidGetMissionById(string id)
    {
        Mission newPath = new Mission();

        // Id de la mission
        newPath._id = id;

        string file = androidReadString(id);
        string[] values = file.Split('#');
        string[] strLenght = values[1].Split(' ');
        // Distancia recorrida
        newPath._length = float.Parse(strLenght[1]);

        string[] strSteps1 = values[7].Split(' ');
        string[] strSteps2 = strSteps1[1].Split('\n');
        // Numeros de pasos
        newPath._steps = int.Parse(strSteps2[0]);

        string[] strAllSteps = file.Split(':');
        string[] strPoints = strAllSteps[8].Split('\n');

        // Ruta
        for (int i = 1; i < strPoints.Length - 1; i++)
        {
            string[] strCoordiantes = strPoints[i].Split(' ');
            //rtn += strCoordiantes [0] + "-" + strCoordiantes[1] + "-" + strCoordiantes[2] + "\n";
            float x = float.Parse(strCoordiantes[0]);
            float y = float.Parse(strCoordiantes[1]);
            float z = float.Parse(strCoordiantes[2]);
            Vector3 point = new Vector3(x, y, z);
            newPath._path.Add(point);
        }
        return newPath;
    }

    static string androidReadString(string id)
    {
        string res = " NONE ";


        if (Application.platform == RuntimePlatform.Android)
        {
            // Android
            string oriPath = System.IO.Path.Combine(Application.streamingAssetsPath, id + ".txt");

            res = oriPath;

            // Android only use WWW to read file
            WWW reader = new WWW(oriPath);
            while (!reader.isDone) { }

            res = reader.text;
        }

        return res;

    }

}
