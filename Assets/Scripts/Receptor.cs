using System.Collections;
using UnityEngine;

public class Receptor : MonoBehaviour{
	private bool _available;
	
	private void Start(){
		_available = true;
	}
	
	public void OnTriggerEnter2D(Collider2D other){
		if (!_available) return;
		if (!other.CompareTag("Ligand")) return;
		other.GetComponent<Ligand>().BindToReceptor(transform);
		_available = false;
		//StartCoroutine(WaitCellResponse());
	}
	
	private IEnumerator WaitCellResponse(){
		yield return new WaitForSeconds(3f);
		_available = true;
	}

	public void ResetReceptor(){
		_available = true;
	}
}
