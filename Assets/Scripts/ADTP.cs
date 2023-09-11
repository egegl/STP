using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADTP : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
	private Color _hidden = new Color(1, 1, 1, 0);

	[SerializeField] private Sprite _adpSprite;
    [SerializeField] private Sprite _atpSprite;

    private void Start() {
        _spriteRenderer = GetComponent<SpriteRenderer>();

		// hide
		_spriteRenderer.color = _hidden;
		transform.localScale = new Vector3(1, 1, 1);
    }

	public IEnumerator ChangeSprite() {
		transform.LeanRotate(new Vector3(0, 0, -180), 0.3f).setEaseInQuart();
		yield return new WaitForSeconds(0.29f);
		_spriteRenderer.sprite = _adpSprite;
		transform.LeanRotate(new Vector3(0, 0, 0), 0.3f).setEaseOutQuart();
	}

	public void MoveInTo(Vector3 newPos, float time) {
		transform.LeanMove(newPos, time).setEaseInQuart();
	}

	public void MoveOutTo(Vector3 newPos, float time) {
		transform.LeanMove(newPos, time).setEaseOutQuart();
	}

	public void Initialize(Vector3 startPos) {
		_spriteRenderer.sprite = _atpSprite;
		transform.position = startPos;
		_spriteRenderer.color = _hidden;
		transform.localScale = new Vector3(1, 1, 1);
		gameObject.LeanAlpha(1, 0.3f);
	}

	public void Hide() {
		gameObject.LeanAlpha(0, 0.3f);
	}

	public void Shrink() {
		transform.LeanScale(new Vector3(0, 0, 0), 0.4f).setEaseInQuart();
	}
}
