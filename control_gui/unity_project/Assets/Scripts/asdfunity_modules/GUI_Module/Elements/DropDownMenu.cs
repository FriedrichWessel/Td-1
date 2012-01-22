using UnityEngine;
using System.Collections;

public class DropDownMenu : Frame {

	public RolloutPanel Content;
	public Button HeaderButton;
	public Rect CurrentElementRegion;
	public bool InitialOpen;
	public Frame InitalSelectedObject;
	
	private bool saveHeaderButtonConstantActive;
	public Frame Selected{
		get{
			return currentSelection;
		}
		set{
			oldSelection = currentSelection;
			currentSelection = value;
			Select(currentSelection);
		}
	}
	private Frame currentSelection;
	private Frame oldSelection;
	
	private void Select(Frame element){
		int count = 0;
		var children = Content.GetComponentsInChildren<DropDownElementBehaviour>() as DropDownElementBehaviour[];
		var elementAsDropDown = element.GetComponent<DropDownElementBehaviour>() as DropDownElementBehaviour;
		if(elementAsDropDown == null){
			EditorDebug.LogError("Object: " + element.name + " is no DropDownElementBehaviour!");
			return;
				
		}
		if(children != null){
			foreach(var dropDownElement in children){
				
				if(dropDownElement.Identifier == elementAsDropDown.Identifier){
					//EditorDebug.LogError("Found Object: " + dropDownElement.Identifier);
					break;
				}
					
				count++;
			}
			Select(count);	
		}
		
	}
	
	private void Select(int index){
		
		if(oldSelection != null){
			EditorDebug.LogError("Destroy: " + oldSelection.name);
			Destroy(oldSelection.gameObject);
			
		}
		
		//EditorDebug.LogError("Save Oldselection: " + oldSelection.name);
			
		
		var children = Content.GetComponentsInChildren<DropDownElementBehaviour>() as DropDownElementBehaviour[];
		if(index >= children.Length){
			EditorDebug.LogError("Index: " + index + " is out of bounds! While select DropDownElement");
			return;
		}
		var obj = Instantiate(children[index].gameObject) as GameObject;
		currentSelection = obj.GetComponent<Frame>() as Frame;
		//if(oldSelection == null)
		//	oldSelection = currentSelection;
		var dropDownBehaviour = currentSelection.gameObject.GetComponent<DropDownElementBehaviour>() as DropDownElementBehaviour;
		if(dropDownBehaviour != null)
			Destroy(dropDownBehaviour);
		EditorDebug.LogError("Duplicate: " + currentSelection.name + " Index: " + index);
		
		currentSelection.activeScreen = this.activeScreen;
		//currentSelection.UpdateActiveScreen();
		currentSelection.Visibility = true;
		currentSelection.gameObject.transform.parent = this.gameObject.transform;
		currentSelection.VirtualRegionOnScreen = CurrentElementRegion;
		currentSelection.CreateElement();
		currentSelection.UpdateElement();
	}
	
	protected override void firstUpdate (){
		base.firstUpdate ();
		if(!InitialOpen){
			Content.Hide(0);
		}
			

		if(InitalSelectedObject != null)
			Select(InitalSelectedObject);
		else{
			EditorDebug.LogWarning("DropDownMenu: " + gameObject.name + " has no InitiaElement Set - take Element zero");
			Select(0);
		}
		
			
	}
	
	public void ToggleContentVisibility(){
		if(Content.Visibility){
			Content.Hide();
			HeaderButton.ConstantActive = saveHeaderButtonConstantActive;
			HeaderButton.UpdateElement();
		} else{
			saveHeaderButtonConstantActive = HeaderButton.ConstantActive;
			Content.Show();
			HeaderButton.ConstantActive = true;
			HeaderButton.UpdateElement();
			
		}
			
	}
	
}
