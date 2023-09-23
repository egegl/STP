using Cinemachine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CamManager : MonoBehaviour{
	private CinemachineBrain _vCamBrain;
	private bool _zoomed;
	private bool _zoomCD;
	private float _minCamY;

	[SerializeField] private Scrollbar _scrollbar;
	[SerializeField] private Transform _zoomCam;
	[SerializeField] private Transform _mainCam;
	// singleton
	public static CamManager Instance { get; private set; }
	private void Awake(){
		Instance = this;
	}

	private void Start(){
		_vCamBrain = GetComponent<CinemachineBrain>();
		_vCamBrain.m_DefaultBlend.m_Time = 0.5f;
		_zoomed = false;
		_zoomCD = false;
		_minCamY = -30;
	}
	
	private void Update(){
		// move camera up and down
		if (Input.GetAxis("Mouse ScrollWheel") > 0 || Input.GetKey(KeyCode.UpArrow)) {
			if (_mainCam.position.y < 0) {
				_mainCam.position += new Vector3(0, .3f, 0);
				FixScroll(); 
			}
		}
		if (Input.GetAxis("Mouse ScrollWheel") < 0 || Input.GetKey(KeyCode.DownArrow)) {
			if (_mainCam.position.y > _minCamY) {
				_mainCam.position -= new Vector3(0, .3f, 0);
				FixScroll();
			}
		}
		if (Input.GetMouseButtonDown(1) && !_zoomCD) {
			if (!_zoomed) {
				Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				mousePos.z = 0;
				if (mousePos.x < 3.5) mousePos.x -= 1.5f;
				else mousePos.x -= 2;
				_zoomCam.position = mousePos;
				_zoomCam.gameObject.SetActive(true);
				_zoomed = true;
			}
			else {
				_zoomCam.gameObject.SetActive(false);
				_zoomed = false;
			}
			StartCoroutine(ZoomCooldown());
		}
	}

	private IEnumerator ZoomCooldown() {
		_zoomCD = true;
		yield return new WaitForSecondsRealtime(0.5f);
		_zoomCD = false;
	}

	private void FixScroll() {
		_scrollbar.value = _mainCam.position.y / _minCamY;
	}

	public void Scroll() {
		float y = Mathf.Lerp(0, _minCamY, _scrollbar.value);
		Vector3 to = new Vector3(_mainCam.position.x, y, _mainCam.position.z);
		_mainCam.position = to;
	}
}
