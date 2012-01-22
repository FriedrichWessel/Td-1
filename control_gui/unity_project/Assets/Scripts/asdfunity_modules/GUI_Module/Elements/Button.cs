using UnityEngine;
using System.Collections;

public class Button : Control {

	public Rect HoverUV;
	public Rect ActiveUV;
	
	public bool ConstantHover;
	public bool ConstantActive;
	

	
	public override void OnClick(object sender, MouseEventArgs e){
		base.OnClick(sender,e);
		
	}
	
	public override void OnDown(object sender, MouseEventArgs e){
			base.OnDown(sender,e);
			if(!ConstantHover){
				plane.UV = ActiveUV;	
			}
			
			
		
		
	}
	
	public override void OnUp(object sender, MouseEventArgs e){
		base.OnUp(sender,e);
		if(down){	
			if(!ConstantHover && !ConstantActive){
				plane.UV = Uv;
			} else if(ConstantActive && Label != null){
				Label.IsActive = true;
			}
		}
	}
	
	public override void OnHover(object sender, MouseEventArgs e){
		base.OnHover(sender,e);
		if(!down && !ConstantActive && plane != null){
			if(!InputEvents.Instance.GetIsDown(0))
				plane.UV = HoverUV;
		}
			
		
		
	}
	
	public override void resetElement(){
		base.resetElement();
		if(!down && plane != null){
			plane.UV = Uv;
		}
		if(ConstantHover && plane != null)
				plane.UV = HoverUV;
		if(plane != null && ConstantActive)
				plane.UV = ActiveUV;
		else if(ConstantActive && Label != null){
			Label.IsActive = true;
		}
	}
	
	
	
	
}
