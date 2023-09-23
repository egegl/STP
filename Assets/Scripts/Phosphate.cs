using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phosphate : MonoBehaviour
{
	private void Start() {
		// hide
		transform.localScale = new Vector3(0, 0, 0);
	}

	public void MoveInTo(Vector3 newPos, float time) {
		transform.LeanMove(newPos, time).setEaseInQuart();
	}

	public void MoveOutTo(Vector3 newPos, float time) {
		transform.LeanMove(newPos, time).setEaseOutQuad();
	}

	public void Initialize(Vector3 startPos) {
		transform.position = startPos;
		transform.LeanScale(new Vector3(1, 1, 1), 0.3f).setEaseInQuart();
	}

	public void Hide() {
		transform.LeanScale(new Vector3(0, 0, 0), 0.3f).setEaseInQuart();
	}
}
