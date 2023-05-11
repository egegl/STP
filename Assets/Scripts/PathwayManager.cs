using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PathwayManager : MonoBehaviour
{
	private bool moveDown = true;
	private Vector3 camMovePos;
	
	[SerializeField] private Button camButton;
	[SerializeField] private Transform membrane;
	[SerializeField] private GameObject ligandPrefab;
	[SerializeField] private Transform ligands;
	[SerializeField] private Transform receptors;
	[SerializeField] private TextMeshProUGUI pathwayText;
	[SerializeField] private TextMeshProUGUI ligandText;
	[SerializeField] private TextMeshProUGUI receptorText;
	[SerializeField] private TextMeshProUGUI responseText;

	// Singleton
	public static PathwayManager Instance { get; private set; }
	
	private void Start(){
		camMovePos = new Vector3(membrane.position.x, membrane.position.y + 2, -10);
		Application.targetFrameRate = 60;
		Instance = this;

		SetLigandText(ligands.childCount);
		SetReceptorText(receptors.childCount);
	}

	public void SpawnLigand(){
		SetLigandText(ligands.childCount + 1);
		Instantiate(ligandPrefab, ligands);
	}
	public void RemoveLigand(){
		if (ligands.childCount == 0) return;
		SetLigandText(ligands.childCount - 1);
		Destroy(ligands.GetChild(0).gameObject);
	}
	
	public void ResetPathway(){
		foreach (Transform ligand in ligands){ 
			Destroy(ligand.gameObject);
		}
		foreach(Transform receptor in receptors){
			receptor.GetComponent<Receptor>().ResetReceptor();
		}
		SetLigandText(0);
	}

	public void SetLigandText(int ligandCount){
		ligandText.text = "#Ligands: " + ligandCount;
	}
	public void SetReceptorText(int receptorCount){
		receptorText.text = "#Receptors: " + receptorCount;
	}

	public void ChangeCam(){
		StartCoroutine(ButtonCooldown(camButton, .7f));
		if (moveDown){
			CamManager.Instance.Move(camMovePos, .6f);
			CamManager.Instance.ChangeSize(19.5f);
		}
		else{
			CamManager.Instance.ResetCam();
		}
		moveDown = !moveDown;
	}

	private IEnumerator ButtonCooldown(Button button, float time){
		button.interactable = false;
		yield return new WaitForSeconds(time);
		button.interactable = true;
	}
}
