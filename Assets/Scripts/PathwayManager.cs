using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PathwayManager : MonoBehaviour
{
	[SerializeField] private GameObject ligandPrefab;
	[SerializeField] private Transform ligands;
	[SerializeField] private int maxLigands;
	[SerializeField] private TextMeshProUGUI pathwayText;
	[SerializeField] private TextMeshProUGUI ligandText;
	[SerializeField] private TextMeshProUGUI receptorText;
	[SerializeField] private TextMeshProUGUI activationText;
	[SerializeField] private GraphWindow[] graphs;

	[HideInInspector] public int LigandCount;
	[HideInInspector] public int ActivationCount;
	// singleton
	public static PathwayManager Instance { get; private set; }
	private void Awake() {
		Instance = this;
	}

	private void Start(){
		// spawn the initial ligand
		SpawnLigand();
		// set ligand text
		SetLigandText(ligands.childCount);
	}

	public void SpawnLigand(){
		if (ligands.childCount == maxLigands) return;
		SetLigandText(ligands.childCount + 1);
		Instantiate(ligandPrefab, ligands);
	}
	public void RemoveLigand(){
		if (ligands.childCount == 0) return;
		SetLigandText(ligands.childCount - 1);
		Destroy(ligands.GetChild(0).gameObject);
	}
	
	public void ResetPathway(){
		// reset ligands
		foreach (Transform ligand in ligands){ 
			Destroy(ligand.gameObject);
		}
		// reset counts
		SetLigandText(0);
		SetActivationText(0);
		// reset graphs
		foreach (GraphWindow graph in graphs) {
			graph.ResetGraph();
		}
		// reset receptor
		Receptor.Instance.ResetReceptor();
	}

	public void SetLigandText(int ligandCt){
		LigandCount = ligandCt;
		ligandText.text = LigandCount.ToString();
	}
	public void SetActivationText(int activationCt) {
		ActivationCount = activationCt;
		activationText.text = ActivationCount.ToString();
	}
}
