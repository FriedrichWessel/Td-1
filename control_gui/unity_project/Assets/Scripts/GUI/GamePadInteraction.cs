using UnityEngine;
using System.Collections;
using System;

public class GamePadInteraction : MonoBehaviour {

	public string AxisName;
	public bool Incrementer;
	public float InputMin = -1;
	public float InputMax = 1;
	public float Step;
	private Slider controledSlider;
	
	
	// Use this for initialization
	void Start () {
		if(AxisName == string.Empty){
			EditorDebug.LogError("No Joystick Axis set on : " + gameObject.name);
		}
		controledSlider = gameObject.GetComponent<Slider>() as Slider;
		if(controledSlider == null){
			EditorDebug.LogWarning("No Slider to Control on: " + gameObject.name);
		}
	}
	
	
	// Update is called once per frame
	void FixedUpdate () {
		
		var value = Input.GetAxis(AxisName);
		var sliderMin = controledSlider.MinValue;
		var sliderMax = controledSlider.MaxValue;
		
		//EditorDebug.Log("Remaped Value: " + sliderValue);
		if(Incrementer){
			//EditorDebug.Log("Axis Value: " + value + " on Object: " + gameObject.name);
			if(value >= InputMax/2){
				//EditorDebug.Log("Step up on Element: " + gameObject.name);
				controledSlider.SliderValue += Step;
			}
				
			else if(value <= InputMin/2){
				//EditorDebug.Log("Step down on Element: " + gameObject.name);
				controledSlider.SliderValue -= Step;
			}
				
		} else {
			var sliderValue = ( ( value - InputMin ) / (InputMax - InputMin) ) * (sliderMax - sliderMin) + sliderMin; 
			controledSlider.SliderValue = sliderValue;
		}
			
	}
}
