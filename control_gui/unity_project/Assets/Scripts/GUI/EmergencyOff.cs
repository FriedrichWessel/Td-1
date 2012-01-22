using UnityEngine;
using System.Collections;

public class EmergencyOff : InteractionBehaviour {

	public Slider MasterSlider;
	public Slider LeftSlider;
	public Slider RightSlider;
	
	private 
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public override void Click(MouseEventArgs e){
		MasterSlider.SliderValue = 0;
		RightSlider.SliderValue = 0;
		LeftSlider.SliderValue = 0;
	}
}
