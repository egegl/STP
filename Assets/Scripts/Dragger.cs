using UnityEngine;

public class Dragger : MonoBehaviour{
	private Ligand ligand;
	private Vector3 dragOffset;
	private Camera cam;

	public bool IsEnabled;

	private void Start() {
		cam = Camera.main;
		if (GetComponent<Ligand>() != null) {
			ligand = GetComponent<Ligand>();
		}
	}

	private void OnMouseDown() {
		if (!IsEnabled) return;
		dragOffset = transform.position - GetMousePos();
	}
	
	private void OnMouseDrag() {
		if(!IsEnabled) return;
		transform.position = GetMousePos() + dragOffset;
	}

	private void OnMouseUp() {	
		if(!IsEnabled) return;
		if (ligand != null){
			ligand.KickStart();
		}
	}
	
	private Vector3 GetMousePos() {
		Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = 0;
		return mousePos;
	}
}
