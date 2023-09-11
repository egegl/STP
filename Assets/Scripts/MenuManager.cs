using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
	[SerializeField] private RectTransform _hbPanel;
	[SerializeField] private float _moveTime = 0.6f;
	private float _hbPanelTop;
	private float _hbPanelBottom;

    private void Start() {
		Time.timeScale = 1;
		_hbPanelTop = (_hbPanel.rect.height - 1080) / -2;
		_hbPanelBottom = -_hbPanel.rect.height;
		_hbPanel.anchoredPosition = new Vector3(0, _hbPanelBottom, 0);
	}

	public void OpenHandbook() {
		_hbPanel.gameObject.SetActive(true);
		_hbPanel.LeanMoveY(_hbPanelTop, _moveTime).setEaseOutQuart();
	}

	public void CloseHandbook() {
		_hbPanel.LeanMoveLocalY(_hbPanelBottom, _moveTime).setEaseInQuart().setOnComplete(() => {
			_hbPanel.gameObject.SetActive(false);
		});
	}	
}
