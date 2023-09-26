using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class 
	MenuManager : MonoBehaviour
{
	[SerializeField] private Sprite[] _buttonSprites;
	[SerializeField] private RectTransform _hbPanel;
	[SerializeField] private RectTransform _infoPanel;
	[SerializeField] private float _moveTime = 1;
	private float _hbPanelTop;
	private float _hbPanelBottom;
	private Transform _infoHeader;
	private TextMeshProUGUI _infoTitle;
	private TextMeshProUGUI _infoText;
	private Sprite _infoSprite;
	private string[] _buttonTypes = {
		"mitochondria",
		"nucleus",
		"lysosome",
		"golgi apparatus"
	};
	private string[] _buttonInfos = {
		"Mitochondria are double-membraned organelles responsible for the process of cellular respiration, which converts glucose into ATP, the energy currency of the cell.\n\n" +
			"The process of cellular respiration involves 4 stages:\n" +
			"1. Glycolysis: \n" +
			"2. (or 1.5) Pyruvate Oxidation: \n" +
			"3. Citric Acid/Krebs Cycle: \n" +
			"4. Oxidative Phosphorylation: \n",
		"The nucleus is the control center of the cell. It stores the cell's DNA, organized into chromosomes. The nucleus is surrounded by a double membrane called the nuclear envelope, which contains pores that allow for the transport of molecules in and out.\n\n" +
			"The nucleus is responsible for the following functions:\n" +
			"1. DNA replication: \n" +
			"2. DNA transcription: \n" +
			"3. DNA translation: \n" +
			"4. Ribosome assembly: \n",
		"Lysosomes are membrane-bound organelles that contain digestive enzymes. They are responsible for breaking down macromolecules and cellular waste.\n\n",
		"Golgi apparatus are membrane-bound organelles that are responsible for the modification, packaging, and transport of proteins and lipids.\n\n"
	};

	private void Start() {
		Time.timeScale = 1;
		_hbPanelTop = (_hbPanel.rect.height - 1080) / -2;
		_hbPanelBottom = -_hbPanel.rect.height;
		_hbPanel.anchoredPosition = new Vector3(0, _hbPanelBottom, 0);
		_infoHeader = _infoPanel.GetChild(0);
		_infoTitle = _infoHeader.GetChild(0).GetComponent<TextMeshProUGUI>();
		_infoSprite = _infoHeader.GetChild(1).GetComponent<Image>().sprite;
		_infoText = _infoPanel.GetChild(1).GetComponent<TextMeshProUGUI>();
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
	
	public void OpenInfo(string buttonType) {
		int buttonIndex = Array.IndexOf(_buttonTypes, buttonType);
		_infoTitle.text = buttonType.ToUpper();
		_infoText.text = _buttonInfos[buttonIndex];
		_infoSprite = _buttonSprites[buttonIndex];
		_infoPanel.localScale = Vector3.zero;
		_infoPanel.gameObject.SetActive(true);
		_infoPanel.LeanScale(Vector3.one, _moveTime).setEaseOutQuart();
	}

	public void CloseInfo() {
		_infoPanel.LeanScale(Vector3.zero, _moveTime).setEaseInQuart().setOnComplete(() => {
			_infoPanel.gameObject.SetActive(false);
		});
	}
}
