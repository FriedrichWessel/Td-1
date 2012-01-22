using UnityEngine;
using System.Collections;

public class Control : Panel {

	// Show Active Region is an EditorDebug Option that makes the active array visible
	public Text Label;
	public bool ShowActiveRegion = false;
	public Rect ActiveRegion;
	protected Rect realActiveRegion;
	protected bool down = false;
	
	
	void Awake(){
		AwakeOverride();
	}
	protected override void AwakeOverride(){
		base.AwakeOverride();
		
	}
	
	void Start () {
		
	}
	
	public override void createGUIElement(){
		base.createGUIElement();
	}
	
	void OnGUI(){
#if UNITY_EDITOR
		if(ShowActiveRegion){
			initActiveRegion();
			UnityEngine.GUI.Box(realActiveRegion, "");	
		}
#endif 
	}
	
	public override void resetElement (){
		base.resetElement();
		if(Label != null)
				Label.IsActive = false;
	}
	
	public override void CreateElement (){
		base.CreateElement ();
		initActiveRegion();
	}
	public override bool checkMouseOverElement(){
		if(ShowActiveRegion)
			initActiveRegion();
		if(!this.Visibility)
			return false;
		
	
		return CameraScreen.CursorInsidePhysicalRegion(realActiveRegion);
	}
	
	public override void UpdateElement(){
		base.UpdateElement();
		initActiveRegion();
		resetElement();
		
	}
	// Caclulate the Absolute Values on the physical screen - because ActiveRegion is virtual an relative to the Control Position

	private void initActiveRegion(){
		if(activeScreen == null){
			EditorDebug.LogWarning("ActiveScreen is not set on Object: " + gameObject.name);
			return;
		}
		var activeRegion = activeScreen.GetPhysicalRegionFromRect(ActiveRegion, KeepAspectRatio);
		realActiveRegion = new Rect(RealRegionOnScreen.x + activeRegion.x , RealRegionOnScreen.y + activeRegion.y, activeRegion.width, activeRegion.height);
	}
	
	public override void OnClick(object sender, MouseEventArgs e){
		base.OnClick(sender,e);
		
	}
	
	public override void OnHover (object sender, MouseEventArgs e){
		base.OnHover (sender, e);
		if(Label != null && ( !InputEvents.Instance.GetIsDown(0) || down) ){
			Label.IsActive = true;
		}
			
	}
	public override void OnDown(object sender, MouseEventArgs e){
		base.OnDown(sender,e);
		down = true;
		if(Label != null){
			Label.IsActive = true;	
		}
			
	}
	
	public override void OnUp(object sender, MouseEventArgs e){
		base.OnUp(sender,e);
		down = false;
		if(Label != null)
			Label.IsActive = false;
	}

}
