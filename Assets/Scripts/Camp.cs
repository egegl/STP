using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camp : MonoBehaviour
{
	private PolygonCollider2D _collider;
	private Rigidbody2D _rb;

	private void Start() {
		_collider = GetComponent<PolygonCollider2D>();
		_rb = GetComponent<Rigidbody2D>();
		_rb.AddForce(new Vector2(2.5f, -1), ForceMode2D.Impulse);
		_rb.AddTorque(Random.Range(-0.6f, 0.6f), ForceMode2D.Impulse);
		StartCoroutine(Initiate());
	}

	private IEnumerator Initiate() {
		yield return new WaitForSeconds(.4f);
		_collider.enabled = true;
	}
}
