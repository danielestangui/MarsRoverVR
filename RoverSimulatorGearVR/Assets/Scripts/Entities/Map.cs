using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
	public string _id;
	public string _name;
	public string _strDMT;
	public Sprite _image;
	public Vector2 _lines;
	public float _scalingFactor;
	public Mesh _terrain;

    public string[] _pathStrings;


    public Mission[] getMissions()
	{
        if (Application.platform == RuntimePlatform.Android)
        {
            List<Mission> missions = new List<Mission>();

            foreach (string path in _pathStrings)
            {
                Mission newPath = FileProcessing.androidGetMissionById(path);
                missions.Add(newPath);
            }
            return missions.ToArray();
        }
        else {
            return FileProcessing.getMissionsByMapId(_id);
        }
    }
}
