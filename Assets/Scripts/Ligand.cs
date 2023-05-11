using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ligand : MonoBehaviour{
	private Vector3 _offset;
	private Vector3 _startPos;
	private Rigidbody2D _rb;
	private CapsuleCollider2D _collider;
	private Dragger _dragger;
	private bool _isBinded;
	
	private float _speed = 2.2f;
	private float _minSpeed = 2f;
	private float _maxSpeed = 2.6f;
	
	[HideInInspector] public Receptor BindedReceptor;

	private void Start(){
		_rb = GetComponent<Rigidbody2D>();
		_dragger = GetComponent<Dragger>();
		_collider = GetComponent<CapsuleCollider2D>();
		_startPos = transform.position;
		_rb.bodyType = RigidbodyType2D.Dynamic;
		BindedReceptor = null;
		_isBinded = false;
		KickStart();
	}
	
	private void FixedUpdate(){
		if (_isBinded) return;
		Vector2 velocity = _rb.velocity;
		velocity.x = Mathf.Clamp(velocity.x, -_maxSpeed, _maxSpeed);
		if (velocity.x > 0 && velocity.x < _minSpeed){
			velocity.x = _minSpeed;
		}
		else if (velocity.x < 0 && velocity.x > -_minSpeed){
			velocity.x = -_minSpeed;
		}
		_rb.velocity = velocity;
	}
	
	private Quaternion RandomAngle(){
		Quaternion angle =  Quaternion.Euler(0, 0, Random.Range(0, 180));
		if (Random.Range(0, 2) == 0) angle.z *= -1;
		return angle;
	}

	public void KickStart(){
		Quaternion angle = RandomAngle();
		_rb.velocity = angle * Vector2.right * _speed;
		_rb.AddTorque(_speed, ForceMode2D.Force);
	}

	public void BindToReceptor(Transform receptor){
		BindedReceptor = receptor.GetComponent<Receptor>();
		_rb.bodyType = RigidbodyType2D.Static;
		transform.rotation = receptor.rotation;
		_collider.enabled = false;
		gameObject.LeanMove(receptor.GetChild(0).position, 0.3f).setEaseInQuart();
		_dragger.IsEnabled = false;
		_isBinded = true;
	}

	public void OnDestroy(){
		if (!_isBinded) return;
		BindedReceptor.ResetReceptor();
	}

	public void ResetLigand(){
		BindedReceptor = null;
		_isBinded = false;
		transform.position = _startPos;
		_dragger.IsEnabled = true;
		_collider.enabled = true;
		_rb.bodyType = RigidbodyType2D.Dynamic;
		KickStart();
	}
}
