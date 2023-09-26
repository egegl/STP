using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
	private int index;
	private bool isPaused;
	private Transform winPanel;
	[SerializeField] private Transform levelPanel;
	[SerializeField] private TMP_Text missionText;
	[SerializeField] private Image pauseButton;
	[SerializeField] private Sprite pauseSprite;
	[SerializeField] private Sprite playSprite;

	// singleton
	public static InterfaceManager Instance { get; private set; }
	private void Awake() {
		Instance = this;
	}

	private void Start() {
		winPanel = levelPanel.GetChild(0);
		winPanel.localScale = Vector3.zero;
		isPaused = false;
		// set level mission text
		missionText.text = (PlayerPrefs.GetString("LevelMission"));
	}
	// DEBUG
	private void Update() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			if(index == 0) {
				WinLevel();
				index++;
			} else {
				LoseLevel();
				index--;
			}
		}
	}

	public void Pause() {
		isPaused = !isPaused;
		if (isPaused) {
			Time.timeScale = 0;
			pauseButton.sprite = playSprite;
		} else {
			Time.timeScale = 1;
			pauseButton.sprite = pauseSprite;
		}
	}

	public void WinLevel() {
		Pause();
		StartCoroutine(WinSequence());
	}

	public void LoseLevel() {
		Pause();
		StartCoroutine(LoseSequence());
	}

	private IEnumerator WinSequence() {
		levelPanel.gameObject.SetActive(true);
		yield return new WaitForSecondsRealtime(0.2f);
		winPanel.LeanScale(Vector3.one, 1).setIgnoreTimeScale(true).setEaseOutBounce();
	}

	private IEnumerator LoseSequence() {
		winPanel.LeanScale(Vector3.zero, 0.3f).setEaseInBounce();
		yield return new WaitForSeconds(0.5f);
		levelPanel.gameObject.SetActive(false);
	}
}
