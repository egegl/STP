using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Receptor : MonoBehaviour
{
	private bool _available;
	private Ligand _ligand;
	private GameObject _gp1;
	private Transform _gp1Transform;
	private Vector3 _g1Start;
	private GameObject _gp2;

	private GameObject _gdp;
	private Rigidbody2D _gdpRb;
	private Vector3 _gdpStartPos;
	private Vector3 _gdpStartRot;
	private GameObject _gtp;
	private Vector3 _gtpStart;

	private string _levelName;
	private bool _cascade = true;

	private Vector2 _armStart;
	private Vector2 _armActive = new Vector2(-0.38f, -1.55f);
	[SerializeField] private Transform _arm;

	[SerializeField] private ADTP _adtp;
	[SerializeField] private Phosphate _Phosphate;

	[SerializeField] private Transform[] _atpPositions;
	[SerializeField] private Transform _camps;
	[SerializeField] private GameObject _campPrefab;

	[HideInInspector] public int CAMPCount;
	[HideInInspector] public int ResponseCount;
	// singleton
	public static Receptor Instance { get; private set; }
	private void Awake() {
		Instance = this;
	}

	private void Start() {
		// G protein values
		_gp1 = transform.GetChild(2).gameObject;
		_gp1Transform = _gp1.transform;
		_g1Start = _gp1Transform.position;
		_gp2 = transform.GetChild(3).gameObject;
		// ARM, GDP & GTP values
		_armStart = _arm.transform.position;
		_gdp = _gp1Transform.GetChild(0).gameObject;
		_gdpRb = _gdp.GetComponent<Rigidbody2D>();
		_gtp = _gp1Transform.GetChild(1).gameObject;
		_gdpStartPos = _gdp.transform.position;
		_gdpStartRot = _gdp.transform.eulerAngles;
		_gtpStart = _gtp.transform.position;
		// level-dependent values
		_levelName = SceneLoader.Instance.SceneName;
		if(_levelName == "Level1") _cascade = false;
		// reset receptor values
		ResetReceptor();
	}

	public void ResetReceptor() {
		CAMPCount = 0;
		ResponseCount = 0;
		_ligand = null;
		_available = true;
	}

	public void OnTriggerEnter2D(Collider2D other) {
		if (!_available) return;
		if (!other.CompareTag("Ligand")) {
			return;
		}
		_ligand = other.GetComponent<Ligand>();
		_ligand.BindToReceptor(transform);
		_available = false;
		StartCoroutine(CellResponse());
	}

	private IEnumerator CellResponse() {
		// activate G protein by switching GDP to GTP
		StartCoroutine(Gdtp(true));
		yield return new WaitForSeconds(3.5f);
		// move G protein to adenylate cyclase	
		_gp1.LeanMoveX(_gp1Transform.position.x + 4.85f, 1).setEaseInQuart();
		yield return new WaitForSeconds(1.1f);
		// activate adenylate cyclase
		_adtp.Initialize(_atpPositions[0].position);
		yield return new WaitForSeconds(0.1f);
		_adtp.MoveInTo(_atpPositions[1].position, 1.3f);
		yield return new WaitForSeconds(1.2f);
		_adtp.Shrink();
		yield return new WaitForSeconds(0.5f);
		// inactivate G protein by switching GTP to GDP
		StartCoroutine(Gdtp(false));
		yield return new WaitForSeconds(2);
		// produce cAMP	
		GameObject camp = Instantiate(_campPrefab, _camps);
		CAMPCount++;
		// destroy cAMP if level 1
		if (!_cascade) {
			// win level 1 condition
			if (CAMPCount == 5) {
				InterfaceManager.Instance.WinLevel();
			}
			Destroy(camp, 2.5f);
			yield return new WaitForSeconds(1.5f);
		}
		// initiate phosphorylation cascade if NOT level 1
		else {
			Cascade();
			yield return new WaitForSeconds(0);
		}
		// move G protein back & fix GTP position
		_gp1.LeanMoveX(_g1Start.x, 1).setEaseInQuart();
		yield return new WaitForSeconds(1.1f);
		_gtp.transform.position = _gtpStart;
		// fade in gamma and beta structures
		_gp2.LeanAlpha(1, 0.3f).setEaseInQuart();
		// remove ligand
		yield return new WaitForSeconds(1.3f);
		RemoveLigand();
		// become available again
		yield return new WaitForSeconds(1f);
		_available = true;
	}

	private IEnumerator Gdtp(bool forward) {
		if(forward) {
			// fade out gamma and beta structures
			_gp2.LeanAlpha(0.5f, 0.3f).setEaseInQuart();
			// start ARM
			_arm.LeanMoveLocal(_armActive, 0.5f).setEaseInQuart();
			yield return new WaitForSeconds(0.5f);
			// rotate ARM with GDP
			_gdp.transform.SetParent(_arm);
			_arm.LeanRotateZ(-90, 0.4f).setEaseOutBounce();
			yield return new WaitForSeconds(0.6f);
			// make GDP the child of GP1
			_gdp.transform.SetParent(_gp1Transform);
			// give GDP force to move left & fade out
			_gdpRb.AddForce(new Vector2(-2.5f, 0), ForceMode2D.Impulse);
			_gdpRb.AddTorque(Random.Range(-0.5f, 0.5f), ForceMode2D.Impulse);
			yield return new WaitForSeconds(0.2f);
			_gdp.LeanAlpha(0, 0.4f).setEaseInQuart();
			yield return new WaitForSeconds(0.2f);
			// reset ARM
			_arm.LeanRotateZ(0, 0.4f).setEaseInQuart();
			_arm.LeanMove(_armStart, 0.5f).setEaseInQuart();
			yield return new WaitForSeconds(0.5f);
			// move and fade GTP in
			_gtp.LeanAlpha(1, 0.4f).setEaseInQuart();
			_gtp.LeanMoveX(_gdpStartPos.x, 0.7f).setEaseInQuart();
		}
		else {
			// move GDP in to GTP position
			Vector3 gtpPos = _gtp.transform.position;
			_gdpRb.velocity = Vector2.zero;
			_gdpRb.angularVelocity = 0;
			_gdp.transform.eulerAngles = _gdpStartRot;
			_gdp.transform.position = gtpPos;
			// fade GTP out and GDP in
			_gtp.LeanAlpha(0, 1);
			yield return new WaitForSeconds(0.2f);
			_gdp.LeanAlpha(1, 1);
			// Eject phosphate molecule
			_Phosphate.Initialize(gtpPos);
			yield return new WaitForSeconds(0.1f);
			_Phosphate.MoveOutTo(new Vector3(gtpPos.x, gtpPos.y - 1, gtpPos.z), 1.5f);
			yield return new WaitForSeconds(0.5f);
			// Fade phosphate out
			_Phosphate.Hide();
		}
	}
	
	private IEnumerator Cascade() {
		yield return new WaitForSeconds(0);
	}

	private void RemoveLigand() {
		if(_ligand != null) {
			_ligand.KickStart();
		}
	}
}
