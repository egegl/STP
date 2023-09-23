using UnityEngine;

public class Level1Manager : MonoBehaviour
{
	private void Update() {
		if (Receptor.Instance.CAMPCount < 10) return;
		InterfaceManager.Instance.WinLevel();
	}
}
