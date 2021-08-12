using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoverController : MonoBehaviour {

    private Vector3[] _wayPoints;
    public int _actualPoint;
	public float _distanceToPoint;
    private bool _move;

    public float _speed;
	// Esta variable puede dar problemas, depende de Time.deltaTime y por tatno de la potencia del ordenador //
	// Controla la velocidad de rotación cunado supera la rotación maxima de rotacion _maxStreerAngle // 
	public float _speedRotation;
	public Rigidbody _rb;

    private Vector3 _utimatePosition;
    private float _distanceTraveled;

	public WheelCollider _wheelFL;
	public WheelCollider _wheelFR;
	public WheelCollider _wheelML;
	public WheelCollider _wheelMR;
	public WheelCollider _wheelBL;
	public WheelCollider _wheelBR;

	public GameObject _FL;
	public GameObject _FR;
	public GameObject _ML;
	public GameObject _MR;
	public GameObject _BL;
	public GameObject _BR;

	public float _topSpeed = 250f;
	public float _maxTorque = 200f;
	public float _maxSteerAngle = 45f;
	public float _currentSpeed;
	public float _angle;


    void Start()
    {
        _actualPoint = 1;
		_move = true;
        _distanceTraveled = 0;   
    }
	void Update(){

		/*if(Input.GetKeyDown(KeyCode.Q) || OVRInput.GetDown(OVRInput.Button.DpadDown))
		{
			_move = !_move;
			_rb.useGravity = _move;
		}*/
		rotateUpdate ();
	}
    // Update is called once per frame
    void FixedUpdate () {

		// Si ha llegado al final //
		if (_actualPoint >= _wayPoints.Length) {
			_move = false;
			_rb.isKinematic = true;
			return;
		}

        if (!_move) {
            return;
        }

		float distance = Vector3.Distance(_rb.gameObject.transform.position, _wayPoints[_actualPoint]);

        // Si esta cerca del siguiente punto //
		if (distance < _distanceToPoint)
        {
            _actualPoint++;
        }
        else {
			rotate ();
            move();
            _distanceTraveled = Vector3.Distance(_utimatePosition,transform.position);
        }

	}

	private void rotate()
	{
		// Vector que apunta en el plano xz al siguiente wayPoint
		Vector3 direction = new Vector3(_wayPoints [_actualPoint].x - _rb.transform.position.x,0,_wayPoints [_actualPoint].z - _rb.transform.position.z).normalized;
		// Vector que apunta en el plano xz hacia la derecha
		Vector3 right = new Vector3(_rb.transform.right.x,0,_rb.transform.right.z).normalized;

		int turn = (Vector3.Angle (right, direction) < 90)? 1:-1;

		_angle = (Vector3.Angle (_rb.transform.forward, direction)) * turn;
		if(Mathf.Abs(_angle)>_maxSteerAngle) _angle = _maxSteerAngle * turn;

		float speedRotationFixed = Time.deltaTime * _speedRotation;
		float anglePerWheel = (_angle * speedRotationFixed) / 2;

		_wheelBL.steerAngle = turn * (180 - Mathf.Abs (anglePerWheel)) ;
		_wheelBR.steerAngle = turn * (180 - Mathf.Abs (anglePerWheel)) ;
		//_wheelML.steerAngle = anglePerWheel;
		//_wheelMR.steerAngle = anglePerWheel;
		_wheelFL.steerAngle = anglePerWheel;
		_wheelFR.steerAngle = anglePerWheel;
	}

	/// <summary>
	/// Atualiza la posicion de las "reudas" con respecto a los coliders de las ruedas
	/// </summary>
	private void rotateUpdate()
	{

		// RUEDAS FRONTALES //
		Quaternion flq;
		Vector3 flv;
		_wheelFL.GetWorldPose (out flv,out flq);
		_FL.transform.position = flv;
		_FL.transform.rotation = flq;
	
		Quaternion frq;
		Vector3 frv;
		_wheelFR.GetWorldPose (out frv,out frq);
		_FR.transform.position = frv;
		_FR.transform.rotation = frq;

		// RUEDAS MEDIAS // 
		Quaternion mlq;
		Vector3 mlv;
		_wheelML.GetWorldPose (out mlv,out mlq);
		_ML.transform.position = mlv;
		_ML.transform.rotation = mlq;

		Quaternion mrq;
		Vector3 mrv;
		_wheelMR.GetWorldPose (out mrv,out mrq);
		_MR.transform.position = mrv;
		_MR.transform.rotation = mrq;

		// RUEDAS TRASERAS //

		Quaternion blq;
		Vector3 blv;
		_wheelBL.GetWorldPose (out blv,out blq);
		_BL.transform.position = blv;
		_BL.transform.rotation = blq;

		Quaternion brq;
		Vector3 brv;
		_wheelBR.GetWorldPose (out brv,out brq);
		_BR.transform.position = brv;
		_BR.transform.rotation = brq;
	}

    private void move() {
		
		_currentSpeed = 2 * 22 / 7 * _wheelBL.radius * _wheelBL.rpm * 60 / 1000; // Velocidad en km//
		if(_currentSpeed < _topSpeed)
		{
			// El moetor solo funciona en las ruedas de en medio //
			_wheelML.motorTorque = _maxTorque;
			_wheelMR.motorTorque = _maxTorque;
		}

    }

    public void setWayPoints(Vector3[] wayPoints) {
        _utimatePosition = transform.position;
        _wayPoints = wayPoints;
    }

    public void setMove(bool move) {
        _move = move;
    }
}
