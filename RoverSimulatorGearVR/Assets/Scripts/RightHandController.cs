using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RightHandController : MonoBehaviour {

    private const float PLAYER_HEIGHT = 3f;
    private const float MAX_TELEPORT_DIST = 15f;

    public LineRenderer _lineRenderer;
    private Vector3 _roverPosition;
    private GameObject _player;
    private bool _isFollowing;

   // public Text _console;

    private float _rayDist = 0;

    void Start()
    {
        _isFollowing = false;
        //_lineRenderer = GetComponent<LineRenderer>();
        _player = GameObject.FindGameObjectWithTag(Tag.player);
        _roverPosition = GameObject.FindGameObjectWithTag(Tag.rover).transform.GetChild(0).transform.position;
    }
    private void Update()
    {

        RaycastHit hit = checkRayCastCollision();

        _lineRenderer.material.color = Color.red;

        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            _rayDist++;

            _lineRenderer.enabled = _isFollowing;
            _isFollowing = !_isFollowing;
        }


        // Si esta siguiendo al rover el laser esta desactivado
        if (_isFollowing)
        {
            _roverPosition = GameObject.FindGameObjectWithTag(Tag.rover).transform.GetChild(0).transform.position;
            _player.transform.position = _roverPosition + Vector3.up * PLAYER_HEIGHT;
            return;
        }

        if (hit.collider == null)
        {
            //_console.text = "null";
            _lineRenderer.SetPosition(1, transform.position + transform.forward * MAX_TELEPORT_DIST);
            _lineRenderer.SetPosition(0, transform.position);
            return;
        }

        if (hit.collider.gameObject.tag.Equals(Tag.terrain))
        {
            if (hit.distance < MAX_TELEPORT_DIST)
            {
                _lineRenderer.material.color = Color.green;
                if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
                    _player.transform.position = new Vector3(hit.point.x, hit.point.y + PLAYER_HEIGHT, hit.point.z);
            }
        }

        _lineRenderer.SetPosition(1, hit.point);
        _lineRenderer.SetPosition(0, transform.position);

        //_console.text = hit.collider.gameObject.tag + " - " + _rayDist ;
    }

    public RaycastHit checkRayCastCollision()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit, 200))
        {
            _lineRenderer.material.color = Color.red;
        }

        return hit;
    }

  
}