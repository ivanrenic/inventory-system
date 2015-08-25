using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HandleCanvas : MonoBehaviour {

	void Start () {
		GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
	}

}
