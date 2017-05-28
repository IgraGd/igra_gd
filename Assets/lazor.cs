using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lazor : MonoBehaviour {
	public Camera cam;
	private RaycastHit hit;
	private Ray ray;
	private LineRenderer lineRenderer;

	void Start () {
		lineRenderer = GetComponent<LineRenderer>();
	}

	void Update () {
		if (Input.GetMouseButton(0)) {
			//ray = cam.ScreenPointToRay(Input.mousePosition);
			ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
			if (Physics.Raycast(ray, out hit)) {
				lineRenderer.SetPosition(0, transform.position);
				lineRenderer.SetPosition(1, hit.point);
			} else {
				lineRenderer.SetPosition(0, transform.position);
				lineRenderer.SetPosition(1, transform.position);
			}
		} else {
			lineRenderer.SetPosition(0, transform.position);
			lineRenderer.SetPosition(1, transform.position);
		}
	}
}
