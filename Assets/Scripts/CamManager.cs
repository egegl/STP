using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CamManager : MonoBehaviour{
	private CinemachineVirtualCamera vCam;
	private Vector3 startPos;
	private float startSize;
	private float maxSize = 19.5f;
	private float minSize = 5f;
	public Transform boundaries;
	private float boundSize;
	
	public static CamManager Instance { get; private set; }
	private void Awake(){
		Instance = this;
	}

	private void Start(){
		vCam = GetComponent<CinemachineVirtualCamera>();
		//boundaries = transform.GetChild(0);
		startPos = transform.position;
		startSize = vCam.m_Lens.OrthographicSize;
	}
	
	private void Update(){
		if (Input.GetAxis("Mouse ScrollWheel") > 0f && vCam.m_Lens.OrthographicSize > minSize){
			vCam.m_Lens.OrthographicSize -= .8f;
		}
		else if (Input.GetAxis("Mouse ScrollWheel") < 0f && vCam.m_Lens.OrthographicSize < maxSize){
			vCam.m_Lens.OrthographicSize += .8f;
		}
		boundSize = vCam.m_Lens.OrthographicSize / 6;
		boundaries.localScale = new Vector3(boundSize, boundSize, 1);
		
		if (Input.GetKey(KeyCode.W)){
			transform.position += new Vector3(0, .2f, 0);
		}
		if (Input.GetKey(KeyCode.S)){
			transform.position += new Vector3(0, -.2f, 0);
		}
		if (Input.GetKey(KeyCode.A)){
			transform.position += new Vector3(-.2f, 0, 0);
		}
		if (Input.GetKey(KeyCode.D)){
			transform.position += new Vector3(.2f, 0, 0);
		}
	}
	
	private IEnumerator Size(float to){
		while (vCam.m_Lens.OrthographicSize < to - 1){
				vCam.m_Lens.OrthographicSize += .5f;
				yield return new WaitForSeconds(.01f);
		}
		while (vCam.m_Lens.OrthographicSize > to + 1){
			vCam.m_Lens.OrthographicSize -= .5f;
				yield return new WaitForSeconds(.01f);
		}
		vCam.m_Lens.OrthographicSize = to;
	}

	public void ChangeSize(float to) {
		StartCoroutine(Size(to));
	}
	
	public void Move(Vector3 to, float time){
		transform.LeanMove(to, time).setEaseInOutQuart();
	}
	
	public void ResetCam(){
		Move(startPos, .6f);
		StartCoroutine(Size(startSize));
	}
}
