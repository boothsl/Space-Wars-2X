using UnityEngine;
using System.Collections;

public class StartCancelMenuBase : MonoBehaviour {

	public GUIStyle MenuButtonGuiStyle;
	public int ButtonWidth;
	public int ButtonHeight;

	public string OkButtonText = "Start";
	public string CancelButtonText = "Cancel";

	protected string error = null;

	protected const int startButtonId = -1;
	protected const int cancelButtonId = -2;

	virtual protected void OnStart()
	{
	}

	virtual protected void OnCancel()
	{
	}

	virtual protected void OnButtonPress(int buttonId)
	{
		switch(buttonId)
		{
		case startButtonId:
			OnStart();
			break;
		case cancelButtonId:
			OnCancel();
			break;
		}
	}

	protected void CreateButton(float x, float y, float width, float height, string text, GUIStyle buttonStyle, int buttonId) {
		float hw = width / 2;
		float hh = height / 2;
		if (GUI.Button(new Rect(x - hw, y - hh, width, height), text, buttonStyle)) 
		{
			OnButtonPress(buttonId);
		}
	}	

	virtual protected void OnGUI() {
		float width = Screen.width;
		float height = Screen.height;
		
		CreateButton (width * 0.4f, height * 0.75f, ButtonWidth, ButtonHeight, OkButtonText, MenuButtonGuiStyle, startButtonId);
		CreateButton (width * 0.6f, height * 0.75f, ButtonWidth, ButtonHeight, CancelButtonText, MenuButtonGuiStyle, cancelButtonId);

		if (error != null)
		{
			GUI.Label ( new Rect(width*0.4f, height * 0.55f, width*0.4f, ButtonHeight), error);
		}
	}

}

