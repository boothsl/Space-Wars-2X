using UnityEngine;
using System.Collections;

public class StartClientGUI : StartCancelMenuBase {

	private const int RefreshHostsButtonId = -1000; // Should not be positive, those are used to index the hosts list.
	public string StartSceneName;
	
	public GameObject PreviousMenuContainer;

	private int selectedHost = -1;

	private HostData[] hostList = null;

	private void RefreshHostsList()
	{
		MasterServer.RequestHostList(GlobalGameState.GameType);
	}

	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if (msEvent == MasterServerEvent.HostListReceived)
			hostList = MasterServer.PollHostList();
	}

	protected override void OnStart ()
	{
		if (GlobalGameState.ServerLocation != null)
		{

			GlobalGameState.IsRunningServer = false;

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

		switch(buttonId)
		{
		case RefreshHostsButtonId:
			RefreshHostsList();
			break;
		}
	}
	
	override protected void OnGUI() {
		float width = Screen.width;
		float height = Screen.height;

		CreateButton( width * 0.3f, height * 0.2f, ButtonWidth*1.5f, ButtonHeight, "Refresh Hosts", MenuButtonGuiStyle, RefreshHostsButtonId);

		if (hostList != null && hostList.Length > 0)
		{
			string[] content = new string[hostList.Length];
			for(int i = 0; i < hostList.Length; i++)
			{
				content[i] = hostList[i].gameName;
			}

			selectedHost = GUI.SelectionGrid(
				new Rect( width * 0.2f, height * 0.3f, width * 0.6f, ButtonHeight * content.Length ),
				selectedHost,
				content,
				1);

			if (selectedHost >= 0 && selectedHost < content.Length)
			{
				Debug.Log ("Selected game: " + hostList[selectedHost].gameName );
				GlobalGameState.ServerLocation = hostList[selectedHost];
			}
		}

		if (hostList == null)
		{
			error = "Hit 'Refresh Hosts' to get the list of hosts";
		} else if (hostList.Length == 0)
		{
			error = "No hosts found, please start a server";
		} else {
			error = null;
		}

		base.OnGUI();
	}

}