using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RightHandMenuController : MonoBehaviour {


    private LineRenderer _lineRenderer;


    void Start()
    {

        _lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (_lineRenderer == null)
            return;

        _lineRenderer.SetPosition(0, transform.position);

        RaycastHit hit = checkRayCastCollision();

        if (hit.collider == null)
        {
            _lineRenderer.material.color = Color.red;
            _lineRenderer.SetPosition(1, transform.position + transform.forward * 100);
            return;
        }

        if (hit.collider.gameObject.tag.Equals(Tag.button))
        {
            _lineRenderer.material.color = Color.green;

            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
            {
                Button button = hit.collider.gameObject.GetComponent<Button>();
                button.onClick.Invoke();
            }
        }

        _lineRenderer.SetPosition(1, hit.point);
    }


    public RaycastHit checkRayCastCollision()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            _lineRenderer.material.color = Color.blue;
        }

        return hit;
    }
}
