using UnityEngine;
using System.Collections;
using System;


public class HTTPBackend : MonoBehaviour {
	public int id;
	private const string ADDRESS = "http://localhost:8080/set/{0}/{1}";
	private const int MIN_VAL = 38;
	private const int MAX_VAL = 216;
	private Slider controledSlider;

	private static byte[] StringToByteArray(string str) {
		System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
		return enc.GetBytes(str);
	}

	private static IEnumerator callREST(string data) {
		WWW www = new WWW(data);
		yield return www;
	}

		// Use this for initialization
	void Start () {
		controledSlider = gameObject.GetComponent<Slider>() as Slider;
		if(controledSlider == null){
			EditorDebug.LogWarning("No Slider to Control on: " + gameObject.name);
		}
		controledSlider.SliderValueChanged += (sender, e) => {
			var call = String.Format(ADDRESS, id, (int)(e.NewSliderValue));
			EditorDebug.Log(call);
			StartCoroutine(callREST(call));
		};
	}
}
