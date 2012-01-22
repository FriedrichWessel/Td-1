using UnityEngine;
using System.Collections;

public class MasterSliderBehaviour : MonoBehaviour {

	public Slider LeftSlider;
	public Slider RightSlider;
	private Slider masterSlider;
	
	#region Init
	void Awake(){	
	}
	
	
	
	// Use this for initialization
	void Start () {
		masterSlider = gameObject.GetComponent<Slider>() as Slider;
		if(masterSlider == null){
			EditorDebug.LogError("Element is no Slider " + gameObject.name);
			return;
		}
		
		masterSlider.SliderValueChanged += OnMasterSliderValueChanged;	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	#endregion
	void OnMasterSliderValueChanged (object sender, SliderEventArgs e){
		if(masterSlider == null || RightSlider == null || LeftSlider == null){
			EditorDebug.LogError("Cannot found all Slider - Cancel!!");
			return;
		}
		var difference = e.SliderDifference;
		
		RightSlider.SliderValue += difference;
		LeftSlider.SliderValue += difference;
		EditorDebug.Log("SliderChange Value: " + difference + " Object: " + gameObject.name);
	}
}
