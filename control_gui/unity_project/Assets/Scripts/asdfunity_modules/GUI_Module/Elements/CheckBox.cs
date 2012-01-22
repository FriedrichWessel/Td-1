using UnityEngine;
using System.Collections;
using System;

public class CheckBox : Button {


	public event EventHandler<CheckBoxEventArgs> CheckboxStatusChanged;
		
	public bool Checked{
		get{
			return checkedFlag;
		}
		
		set{
			checkedFlag = value;
			if(!checkedFlag){
				ConstantActive = false;
				resetElement();
			}
				
			else{
				ConstantActive = true;
				if(Label != null)
					Label.IsActive = true;
				UpdateElement();
			}
		}
	}
	
	private bool checkedFlag;
	
	protected override void AwakeOverride (){
		base.AwakeOverride ();
		Checked = false;
	}
	
	
	public override void OnClick (object sender, MouseEventArgs e){
		if(InputEvents.Instance.IsActiveElement(this)){
			Checked = !Checked;	
			InvokeCheckboxStatusChanged();	
		}
		base.OnClick (sender, e);
		
		
	}
	
	
	public override void resetElement (){
		base.resetElement();
		if(checkedFlag && Label != null){
			Label.IsActive = true;
		}	
	}
	
	private void InvokeCheckboxStatusChanged(){
		var handler = CheckboxStatusChanged;
		if (handler == null) {
			return;
		}
		
		var e = new CheckBoxEventArgs(this);
		CheckboxStatusChanged(this, e);
	}
}