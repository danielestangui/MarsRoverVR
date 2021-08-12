using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

public class MissionPlanningMenuController : MonoBehaviour {

	// PRIVATE //
	private int _actualMap;
    private int _actualMission;

	// PUBLIC //
	[Header("Interfaz")]
	//public Text _console;
	public Text _infoMapText;
	public Text _infoMissionText;
	public Image _mapDisplay;
    public Text _mapText;
    public Text _missionText;
    //public Dropdown _dropdownMap;
    //public Dropdown _dropdownMission;

    [Header("Nuevas Rutas")]
	//public GameObject _newPathPanel;
	//public InputField _startPositonXIF;
	//public InputField _startPositonYIF;
	//public InputField _endPositonXIF;
	//public InputField _endPositonYIF;


	[Header("Pintar rutas")]
	public Text _startPoint2D;
	public Text _endPoint2D;
	public Text _point2D;

	[Header("Mapas")]
	public Map[] _maps;
    private Mission[] _missions;

	[Header("Calculo de rutas")]
	public float _valorHeuristico;
	public int _pendiente;

    public bool _enablePathPlanningExecutor = false;

    private PathPlanningExecutor _pathPlanningExecutor;


	void Start () 
	{
        // Deshabilitar realiada virtual
        //GameObject gm = GameObject.FindGameObjectWithTag("GameController");
        //gm.GetComponent<VXManager>().disableVR();

		_actualMap = 0;


		loadMapInfo();
	}

	/// <summary>
	/// Inicia la ejecucion del hilo que se encarga de ejecutar el proceso del algoritmo de pathplanning.
	/// </summary>
	/*
     * public void startPathPlanningExecutor()
	{
		
		ThreadStart pathPlanningExecutorThread;
		pathPlanningExecutorThread = new ThreadStart (Run);
		Thread hilo = new Thread (pathPlanningExecutorThread);

		_pathPlanningExecutor = new PathPlanningExecutor ();
		_pathPlanningExecutor.setArgs (_maps [_actualMap],
			int.Parse (_startPositonXIF.text), 
			int.Parse (_startPositonYIF.text), 
			int.Parse (_endPositonXIF.text), 
			int.Parse (_endPositonYIF.text), 
			_valorHeuristico, _pendiente);

		hilo.Start ();

		closeNewPathPanel ();
	}
    */

	/// <summary>
	/// Metodo que va a ejecutar el hilo.
	/// </summary>
    /// 
    /*
	private void Run()
	{
		if (_enablePathPlanningExecutor) {
			Debug.Log ("Resultado: " + _pathPlanningExecutor.execute ());
		} else
			Debug.Log ("CMD: java " + _pathPlanningExecutor.getArgs ());
	}
    */

	public void nextMap ()
	{
		_infoMissionText.text = "";
		_actualMap = (_actualMap + 1)%_maps.Length;
        loadMapInfo();
	}

    public void previousMap()
    {
        _infoMissionText.text = "";

        _actualMap--;

        if (_actualMap < 0) _actualMap = _maps.Length - 1;
        loadMapInfo();
    }

    public void nextMission ()
	{
        _actualMission = (_actualMission + 1) % _missions.Length;
        loadMissionInfo();
    }

    public void previousMission()
    {

        _actualMission--;

        if (_actualMission < 0) _actualMission = _missions.Length - 1;
        loadMissionInfo();
    }

    /// <summary>
    /// Actualiza el menu con la información del mapa seleccionado.
    /// </summary>
    /// <param name="index">Index.</param>
    private void loadMapInfo ()
	{

		// Borra la ruta dubujada sobre el mapDisplay
		foreach (Transform point in _mapDisplay.transform) 
		{
			Destroy (point.gameObject);
		}

		Map map = _maps [_actualMap];

        _mapText.text = map._name;

        _mapDisplay.sprite = map._image;
		string infoText = "INFORMACION DEL MAPA\n\n";
		infoText += "Nombre: " + map._name + "\n";
		infoText += "ID: " + map._id + "\n";
		infoText += "Tamaño: " + map._lines + "\n";
		infoText += "Factor de escala: " + map._scalingFactor + "\n"; 
		_infoMapText.text = infoText;

        // Cargar misiones
        _missions = _maps[_actualMap].getMissions();

        _missionText.text = "";

        if (_missions.Length > 0)
        {
            _actualMission = 0;
            loadMissionInfo();
        }
    }

	/// <summary>
	/// Carga la información de la misión.
	/// </summary>
	private void loadMissionInfo ()
	{
		clearMapDisplay ();
			
		Mission mission = _maps[_actualMap].getMissions()[_actualMission];

        _missionText.text = mission._id;

		string infoText = "INFORMACION DE LA MISION\n\n";
		infoText += "Distnacia: " + mission._length + "\n";
		infoText += "Número de pasos: " + mission._steps + "\n";
		//infoText += "Ruta:\n";
		//foreach(Vector3 vec in mission._path) infoText += vec.x + " " + vec.y + " " + vec.z + "\n";
		_infoMissionText.text = infoText;
		drawPath(mission);

	}

	private void clearMapDisplay(){
		// Borra la ruta dubujada sobre el mapDisplay
		foreach (Transform point in _mapDisplay.transform) 
		{
			Destroy (point.gameObject);
		}
		// Borra la información de la misión
		_infoMissionText.text = "";
	}

	/// <summary>
	/// Actualiza el dropDown de missiones dependiendo del mapa
	/// </summary>
    /*
	private void loadMissionDropDown()
	{
		
		List<string> missionNames = new List<string>();

		if (_maps[_actualMap].getMissions().Length > 0) 
		{
			foreach (Mission p in _maps[_actualMap].getMissions()) 
			{
				missionNames.Add (p._id + "");
			}
			_dropdownMission.AddOptions (missionNames);
			loadMissionInfo (0);
		}
	}
    */

	/// <summary>
	/// Dibuja una ruta de la mision sobre el display del mapa.
	/// </summary>
	/// <value>The draw path.</value>
	private void drawPath(Mission path){
		int count = 0;
		foreach(Vector3 point in path._path){
			if(count == 0) 
				drawPoint (point,_startPoint2D);
			else if (count == (path._path.Count - 1)) 
				drawPoint (point,_endPoint2D);
			else 
				drawPoint (point, _point2D);
			count++;
		}	
	}

	/// <summary>
	/// Dibuja un punto (_point) sobre el dispay del mapa.
	/// </summary>
	/// <param name="point">Point.</param>
	private void drawPoint(Vector3 point, Text prefabPoint2D)
	{
		// Funciona: Si el origne de cordenadas es el centro del MapDisplay //

		//Debug.Log("Posicion real: " + x + "-" + y);

		int realTX = (int)_maps[_actualMap]._lines.x;
		int realTY = (int)_maps[_actualMap]._lines.y;
		//Debug.Log("Tamaño real del mapa: " + realTX + "-" + realTY);

		int TY = (int)_mapDisplay.rectTransform.rect.height;
		int TX = (realTX * TY) / realTY;
		int MY = TY / 2;
		int MX = TX / 2;
		//Debug.Log("MX y MY: " + MX + "-" + MY);

		int pointX = (TX * (int)point.x) / realTX;
		int pointY = (TY * (int)point.y) / realTY;
		//Debug.Log("Puntos sobre la escala: " + pointX + "-" + pointY);

		pointX = pointX - MX;
		pointY = -pointY + MY;
		//Debug.Log("Puntos sobre la escala traducidos: " + pointX + "-" + pointY);

		Text newPoint = Instantiate(prefabPoint2D, _mapDisplay.transform);
		newPoint.rectTransform.localPosition = new Vector3(pointX,pointY,0);

	}

	public void setNewScene(string scene){

		Mission[] missions = _maps [_actualMap].getMissions ();

		GameManager._map = _maps [_actualMap];
		GameManager._mission = missions [_actualMission];

		SceneManagerController.loadScene (scene);
	}

    /*
	public void openNewPathPanel(){
		setNewPathPanelToZero ();
		_infoMapText.text = "";
		_infoMissionText.text = "";
		clearMapDisplay ();
		_newPathPanel.SetActive (true);

	}

	public void closeNewPathPanel(){
		loadMapInfo (_actualMap);
		if(_maps[_actualMap].getMissions().Length > 0){
			loadMissionInfo (0);
		}
		_newPathPanel.SetActive (false);
	}

	public void updateStartAndEnd(){
		clearMapDisplay ();
		drawPoint (new Vector3(int.Parse (_startPositonXIF.text),int.Parse (_startPositonYIF.text),0),_startPoint2D);
		drawPoint (new Vector3(int.Parse (_endPositonXIF.text),int.Parse (_endPositonYIF.text),0),_endPoint2D);
	}

	private void setNewPathPanelToZero(){
		_startPositonXIF.text = 0 + "";
		_startPositonYIF.text = 0 + "";
		_endPositonXIF.text = 0 + "";
		_endPositonYIF.text = 0 + "";
	}
    */
}
