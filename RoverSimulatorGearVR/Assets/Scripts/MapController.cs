using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapController : MonoBehaviour {

	public GameObject _point3D;
	public GameObject _terrain;

	private MeshFilter _terrainMesh;
	private MeshCollider _meshCollider;
    private GameObject _player;
    private GameObject _rover;
	private bool _isPlayer;
	private List<Vector3> _unityPath = new List<Vector3> ();

	private Map _map;
	private Mission _mission;


    [Header("Configuración")]
    public bool _enableVR = false;

    [Header("Para pruebas")]
	public Map _mapDefault;
	private Mission _missionDefault;



	void Awake(){

       // if(_enableVR)
       //     GetComponent<VXManager>().enableVR();


        _player = GameObject.FindGameObjectWithTag (Tag.player);
		_isPlayer = _player;
        _rover = GameObject.FindGameObjectWithTag(Tag.rover);
		_terrainMesh = _terrain.gameObject.GetComponent<MeshFilter> ();
		_meshCollider = _terrain.gameObject.GetComponent<MeshCollider> ();

		_map = GameManager._map;

		// Para hacer pruebas
		if(_map == null){
			_map = _mapDefault;
		}

		_terrainMesh.mesh = _map._terrain;
		_meshCollider.sharedMesh = _map._terrain;

	}


	void Start () {


		_mission = GameManager._mission;

		// Para hacer pruebas
		if(_mission == null){
			_mission = _map.getMissions()[0];
		}	

		// Hasta que no se dibuja la ruta no se rellena _unityPath
		drawPath (_mission);

		if(_isPlayer)setPlayerStartPosition ();
        setRoverStartPosition();

	}

    private void Update()
    {

        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            SceneManagerController.loadScene("MenuScene");
        }
    }

    private void setPlayerStartPosition(){

		_player.transform.position = _unityPath.ToArray()[0] + Vector3.up * 3 - Vector3.forward * 5;

       
	}

    private void setRoverStartPosition()
    {
        _rover.transform.position = _unityPath.ToArray()[0] + Vector3.up * 1;
        _rover.GetComponent<RoverController>().setWayPoints(_unityPath.ToArray());
    }

    public void stopRover() {
        _rover.GetComponent<RoverController>().setMove(false);
    }

    public void moveRover() {
        _rover.GetComponent<RoverController>().setMove(true);
    }

	/// <summary>
	/// Dibuja una ruta de la sobre el mapa.
	/// </summary>
	/// <value>The draw path.</value>
	private void drawPath(Mission mission){
		
		foreach(Vector3 point in mission._path){
			drawPoint (point);
		}	
	}

	/// <summary>
	/// Dibuja un punto (_point) sobre el dispay del mapa.
	/// </summary>
	/// <param name="point">Point.</param>
	private void drawPoint(Vector3 point)
	{


		//Debug.Log("Posicion real: " + x + "-" + y);

		int realTX = (int)_map._lines.x;
		int realTY = (int)_map._lines.y;
		//Debug.Log("Tamaño real del mapa: " + realTX + "-" + realTY);

		int MY = realTY / 2;
		int MX = realTX / 2;
		//Debug.Log("MX y MY: " + MX + "-" + MY);



		int pointX = -((int) point.x - MX);
		int pointY = -(int)point.y + MY;
		//Debug.Log("Puntos sobre la escala traducidos: " + pointX + "-" + pointY);

		GameObject newPoint = Instantiate(_point3D, _terrain.transform);

		newPoint.transform.localPosition = new Vector3(pointX,200, pointY);

		//Debug.Log (newPoint.transform.position);
			
		RaycastHit hit;
		Ray ray = new Ray (newPoint.transform.position,Vector3.down);


		if (Physics.Raycast(ray,out hit,Mathf.Infinity)){
            //Debug.DrawRay(newPoint.transform.position, Vector3.down * hit.distance, Color.green);
            //Debug.Log("Hit" + hit.point);
            if (hit.collider.gameObject.tag.Equals(Tag.terrain))
            {
                newPoint.transform.position = hit.point;
            }
        }

		_unityPath.Add (newPoint.transform.position);
	}

	private void goToPrincipalMenu(){
		SceneManagerController.loadScene ("MenuScene");
	}
}
