using System.Collections;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
	private Animator _anim;
	[HideInInspector] public string SceneName;
	private AsyncOperation asyncLoad;

	// singleton
	public static SceneLoader Instance { get; private set; }
	private void Awake() {
		Instance = this;
		SceneName = SceneManager.GetActiveScene().name;
	}

	private void Start() {
		Application.targetFrameRate = 60;
		_anim = GetComponent<Animator>();
	}
	
	public void LoadScene(string name) {
		SceneName = name;
		_anim.SetTrigger("StartFade");
		// start loading the scene asynchronously
		StartCoroutine(LoadSceneAsync());
	}

	public void SceneMission(string mission) {
		PlayerPrefs.SetString("LevelMission", mission);
	}

	public void OnFadeComplete() {
		// activate the loaded scene
		asyncLoad.allowSceneActivation = true;
	}

	private IEnumerator LoadSceneAsync() {
		// Load the scene in the background & prevent it from displaying when ready
		asyncLoad = SceneManager.LoadSceneAsync(SceneName);
		asyncLoad.allowSceneActivation = false;
		// Wait until the scene is loaded
		while (!asyncLoad.isDone) {
			yield return null;
		}
	}
}
