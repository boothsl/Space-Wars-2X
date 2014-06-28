using UnityEngine;
using System.Collections;

public class StartServerGUI : StartCancelMenuBase {

	public string StartSceneName;

	public GameObject PreviousMenuContainer;

	protected override void OnStart ()
	{
		if (GlobalGameState.GameRoomName != null && GlobalGameState.GameRoomName != string.Empty)
		{
			GlobalGameState.IsRunningServer = true;
			Application.LoadLevel(StartSceneName);
		}
	}

	protected override void OnCancel ()
	{
		gameObject.SetActive(false);
		// TODO : Refactor this part as it's not clean. We shouldn't have
		// to disable a component of a game object, just the game object itself.
		PreviousMenuContainer.GetComponent<MainMenu>().enabled = true;
	}

	protected override void OnButtonPress (int buttonId)
	{
		base.OnButtonPress (buttonId);
	}
	
	override protected void OnGUI() {
		float width = Screen.width;
		float height = Screen.height;
		
		GUI.Label ( new Rect(width * 0.35f, height * 0.4f, ButtonWidth, ButtonHeight), "Name your server:");
		GlobalGameState.GameRoomName = GUI.TextField(
			new Rect(width * 0.5f, height * 0.4f, ButtonWidth, ButtonHeight), 
			GlobalGameState.GameRoomName);

		if (GlobalGameState.GameRoomName == null || GlobalGameState.GameRoomName == string.Empty)
		{
			error = "!!! Please enter a unique server name !!!";
		} else {
			error = null;
		}

		base.OnGUI();
	}
}
