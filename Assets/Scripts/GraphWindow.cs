using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GraphWindow : MonoBehaviour
{
	private RectTransform _rectTransform;
	private float _height;
	private float _width;
	private float _yMax;
	private float _xSize;
	private int _maxXSize;
	private List<int> _xValues = new List<int>() { 0 };
	private List<int> _yValues = new List<int>() { 0 };
	private RectTransform _xLabels;
	private RectTransform _yLabels;
	private RectTransform _titles;
	private RectTransform _circles;
	private Vector2 _labelXPos;
	private Vector2 _labelYPos;
	private TMP_Text _labelXText;
	private TMP_Text _labelYText;
	private TMP_Text _titleText;
	private enum _independentVar { LigandCount, CAMPCount, ResponseCount };
	// filled in by user
	[SerializeField] private RectTransform _labelPrefab;
	[SerializeField] private Sprite _circleSprite;
	[SerializeField] private string _labelXTextString;
	[SerializeField] private string _labelYTextString;
	[SerializeField] private string _titleTextString;
	[SerializeField] private int _refreshTime;
	[SerializeField] private _independentVar _independentVariable;

	private void Start() {
		// get graph info
		_rectTransform = GetComponent<RectTransform>();
		_width = _rectTransform.sizeDelta.x;
		_height = _rectTransform.sizeDelta.y;
		// get labels, title, circles
		_xLabels = transform.GetChild(0).GetComponent<RectTransform>();
		_yLabels = transform.GetChild(1).GetComponent<RectTransform>();
		_titles = transform.GetChild(2).GetComponent<RectTransform>();
		_circles = transform.GetChild(3).GetComponent<RectTransform>();
		// get titles' text
		_labelXText = _titles.GetChild(0).GetComponent<TMP_Text>();
		_labelYText = _titles.GetChild(1).GetComponent<TMP_Text>();
		_titleText = _titles.GetChild(2).GetComponent<TMP_Text>();
		// set label positions
		_labelXPos =  new Vector2(-_width/2, -_height/2 - 15);
		_labelYPos = new Vector2(-_width/2 - 15, -_height/2);

		_yMax = 0;
		_maxXSize = 16;

		SetText();
		InvokeRepeating(nameof(UpdateGraph), _refreshTime, _refreshTime);
	}

	private void SetText() {
		// set titles' text
		_labelXText.text = _labelXTextString;
		_labelYText.text = _labelYTextString;
		_titleText.text = _titleTextString;
	}

	private void UpdateGraph() {
		// clear graph
		ClearGraph();
		// update values according to independent variable
		switch (_independentVariable) {
			case _independentVar.LigandCount:
				UpdateValues(PathwayManager.Instance.LigandCount);
				break;
			case _independentVar.CAMPCount:
				UpdateValues(Receptor.Instance.CAMPCount);
				break;
			case _independentVar.ResponseCount:
				UpdateValues(Receptor.Instance.ResponseCount);
				break;
		}
	}

	public void ResetGraph() {
		_xValues = new List<int>() { 0 };
		_yValues = new List<int>() { 0 };
		_yMax = 0;
		ClearGraph();
	}

	private void ClearGraph() {
		// destroy circles & lines
		for (int i = 0; i < _circles.childCount; i++) {
			Destroy(_circles.GetChild(i).gameObject);
		}
		// destroy labels
		for (int i = 0; i < _xLabels.childCount; i++) {
			Destroy(_xLabels.GetChild(i).gameObject);
		}
	}

	private void UpdateValues(int yValue) {
		// update x values
		int nextNum = _xValues[_xValues.Count - 1] + _refreshTime;
		_xValues.Add(nextNum);
		if (_xValues.Count > _maxXSize) {
			_xValues.RemoveAt(0);
		}
		// update y values
		CheckYMax(yValue);
		_yValues.Add(yValue);
		if(_yValues.Count > _maxXSize) {
			_yValues.RemoveAt(0);
		}
		// update values on graph
		ShowGraph(_yValues);
	}

	private void CheckYMax(int value) {
		if (value >= _yMax/1.1f) {
			_yMax = value * 1.1f;
			// update y labels
			CreateYLabel(_yMax, value);
		}
	}

	private void CreateYLabel(float yMax, int yValue) {
		// destroy old labels
		for (int i = 0; i < _yLabels.childCount; i++) {
			Destroy(_yLabels.GetChild(i).gameObject);
		}
		for (int i = 1; i <= yValue; i++) {
			// create label & set position
			RectTransform labelY = Instantiate(_labelPrefab, _yLabels);
			labelY.anchoredPosition = new Vector2(_labelYPos.x, i * (_height / yMax) + _labelYPos.y);
			// set label text
			labelY.GetComponent<TMP_Text>().text = i.ToString();
		}
	}

	private void CreateXLabel(int circleNum, float xPosition) {
		// create label & set position
		RectTransform labelX = Instantiate(_labelPrefab, _xLabels);
		labelX.anchoredPosition = new Vector2(xPosition + _labelXPos.x, _labelXPos.y);
		// set label text
		labelX.GetComponent<TMP_Text>().text = _xValues[circleNum].ToString();
	}

	private void ShowGraph(List<int> values) {
		RectTransform prevCircle = null;
		int numValues = values.Count - 1;
		_xSize = _width / numValues;

		for(int i = 0; i < numValues; i++) {
			int thisValue = values[i];
			float xPosition = i * _xSize;
			float yPosition = thisValue;
			if(thisValue != 0) {
				yPosition = yPosition / _yMax * _height;
			}
			RectTransform thisCircle = CreateCircle(new Vector2(xPosition, yPosition));
			CreateXLabel(i, xPosition);
			thisCircle.SetSiblingIndex(i);
			if (prevCircle != null) {
				ConnectPoints(prevCircle.anchoredPosition, thisCircle.anchoredPosition);
			}
			prevCircle = thisCircle;
		}		
	}

	private RectTransform CreateCircle(Vector2 AnchoredPos) {
		GameObject circle = new GameObject("Circle", typeof(Image));
		circle.transform.SetParent(_circles, false);
		circle.GetComponent<Image>().sprite = _circleSprite;
		RectTransform circTransform = circle.GetComponent<RectTransform>();
		circTransform.anchoredPosition = AnchoredPos;
		circTransform.sizeDelta = new Vector2(14, 14);
		circTransform.anchorMin = new Vector2(0, 0);
		circTransform.anchorMax = new Vector2(0, 0);
		return circTransform;
	}

	private void ConnectPoints(Vector2 dot1, Vector2 dot2) {
		GameObject line = new GameObject("Line", typeof(Image));
		Image lineImage = line.GetComponent<Image>();
		RectTransform lineTransform = line.GetComponent<RectTransform>();
		line.transform.SetParent(_circles, false);
		lineImage.color = Color.gray;
		Vector2 dir = (dot2 - dot1).normalized;
		float distance = Vector2.Distance(dot1, dot2);
		lineTransform.anchorMin = new Vector2(0, 0);
		lineTransform.anchorMax = new Vector2(0, 0);
		lineTransform.sizeDelta = new Vector2(distance, 3f);
		lineTransform.anchoredPosition = dot1 + 0.5f * distance * dir;
		lineTransform.localEulerAngles = new Vector3(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
	}
}
