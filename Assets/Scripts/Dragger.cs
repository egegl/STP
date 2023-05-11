using Unity.VisualScripting;
using UnityEngine;

public class Dragger : MonoBehaviour{
	private Ligand _ligand;
	private Vector3 _dragOffset;
	private Camera _cam;
	
	public bool IsEnabled { get; set; }

	private void Start(){
		_cam = Camera.main;
		IsEnabled = true;
		if (GetComponent<Ligand>() != null) {
			_ligand = GetComponent<Ligand>();
		}
	}

	private void OnMouseDown(){
		if(!IsEnabled) return;
		_dragOffset = transform.position - GetMousePos();
	}
	
	private void OnMouseDrag(){
		if(!IsEnabled) return;
		transform.position = GetMousePos() + _dragOffset;
	}

	private void OnMouseUp(){	
		if(!IsEnabled) return;
		if (_ligand != null){
			_ligand.KickStart();
		}
	}
	
	private Vector3 GetMousePos(){
		Vector3 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = 0;
		return mousePos;
	}
}
