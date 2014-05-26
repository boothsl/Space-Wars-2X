using UnityEngine;
using System.Collections;

public class MultiplayerController : MonoBehaviour {

	public GameObject CameraObjectToAttachPlayer = null;
	public GameObject PlayerPrefab;

	public ServerEntryView view;
	public ScoreBoard scoreBoard;
	public NetworkGameOver gameOverScreen;

	public int StartPointStartCircleSize = 500;

	public int TargetScoreToWin = 200;

	public float TimeToWaitAtGameOver = 5.0f;

	public string SplashScreenLevelName = "SplashScreen";

	bool IsGameOver = false;
	string winner = string.Empty;

	bool Server 
	{
		get { return GlobalGameState.IsRunningServer; }
	}

	void Start()
	{
		// Entry point to the level
		if (Server)
		{
			Debug.Log ("Connecting to server: " + GlobalGameState.GameType + ":" + GlobalGameState.GameRoomName );
			// Start a server here
			Network.InitializeServer( 4, 36723, !Network.HavePublicAddress());
			MasterServer.RegisterHost (GlobalGameState.GameType,GlobalGameState.GameRoomName);
		} else {
			// Connect to server here.
			Debug.Log ("Connecting to server: " + GlobalGameState.ServerLocation.gameName + "(" + GlobalGameState.ServerLocation.ip + ":" + GlobalGameState.ServerLocation.port + ")" );
			Network.Connect(GlobalGameState.ServerLocation);
		}
	}

	void RegisterEvents()
	{
		view.MyPlayerIsReady += (object sender, System.EventArgs e) => networkView.RPC ("IsReady", RPCMode.Others, view.MyName);
		view.AllPlayersAreReady += (object sender, System.EventArgs e) => view.StartCountdown();
		view.GameShouldStart += (object sender, System.EventArgs e) => StartGame();

		// Global hit events
		GlobalGameState.HitObject += HandleHitObject;
		GlobalGameState.PlayerDied += (object sender, System.EventArgs e) => view.ResetToCountdown();
	}
	
	void HandleHitObject (object sender, GlobalGameState.HitObjectArgs e)
	{
		if (IsGameOver) return; // No more

		// This part should always only be run on the server.
		if (e.hitRaycast.transform.GetComponent<PlayerShield>() != null)
		{
			PlayerShield shield = e.hitRaycast.transform.GetComponent<PlayerShield>();
			if (shield.shieldLevel <= 0) return; // Probably destroy by another shot in the same frame
			shield.shieldLevel -= e.hitStrength;

			// Add to my score
			int scoreToAdd = +10;

			if (shield.shieldLevel <= 0)
			{
				// Player has died.
				Network.Destroy (e.hitRaycast.transform.GetComponent<NetworkView>().viewID);
				Quaternion rotation = Quaternion.FromToRotation(Vector3.up, e.hitRaycast.normal);
				Network.Instantiate(e.explosionEffect, e.hitRaycast.transform.position, rotation,0);

				scoreToAdd += +20;
			}
			// Now go back to the object itself and update the score.
			scoreBoard.AddToPlayerScoreByGuid( e.weaponObject.networkView.owner.guid, scoreToAdd );
			networkView.RPC ("SetPlayerScores", RPCMode.Others, scoreBoard.Serialize());

			if (scoreBoard.GetPlayerScoreByGui(e.weaponObject.networkView.owner.guid) >= TargetScoreToWin)
			{
				// We have a winner.
				winner = scoreBoard.GetPlayerNameByGuid(e.weaponObject.networkView.owner.guid);
				Debug.Log ("Winner is " + winner);
				GameOver();
			}
		}
	}
	
	void GameOver()
	{
		networkView.RPC ("ServerCalledGameOver", RPCMode.All, winner);
		OnGameOver();
	}

	void OnGameOver()
	{
		if (gameOverScreen != null)
		{
			gameOverScreen.enabled = true;
			gameOverScreen.winner = winner;
			StartCoroutine(WaitAndClose(TimeToWaitAtGameOver));
		}
	}

	IEnumerator WaitAndClose(float delay)
	{
		yield return new WaitForSeconds(delay);
		Network.Disconnect();
		Application.LoadLevel (SplashScreenLevelName);
	}

	void OnServerInitialized()
	{
		Debug.Log("Server created");
		//SpawnPlayer();

		// I am the server so add type
		view.MyName = System.Environment.MachineName.Split('.')[0]+"-svr";
		view.AddPlayer(view.MyName, ServerEntryView.PlayerState.kConnected);

		RegisterEvents();

		StartEntry ();
	}
	
	void OnConnectedToServer()
	{
		Debug.Log ("Connected to server");
		//SpawnPlayer();

		// TODO: Get the game state and if game has started they might be able
		// to observe until the next round...
		view.MyName = System.Environment.MachineName.Split('.')[0];
		view.AddPlayer(view.MyName, ServerEntryView.PlayerState.kConnected);

		networkView.RPC("JoinPlayer", RPCMode.Others, view.MyName);

		RegisterEvents();

		StartEntry();
	}

	void StartEntry()
	{
		Debug.Log ("Starting entry mode");
		view.enabled = true;
		view.ClearStates();

		scoreBoard.enabled = false;
		scoreBoard.ClearPlayers ();
	}

	void StartGame()
	{
		Debug.Log ("Starting the game");
		view.enabled = false;
		GameObject player = SpawnPlayer();

		scoreBoard.enabled = true;
		scoreBoard.AddPlayer(view.MyName,player.networkView.owner.guid);

		string serializedData = scoreBoard.Serialize();
		//Debug.Log ("Sending data : " + serializedData);
		networkView.RPC ("SetPlayerScores", RPCMode.Others, serializedData);
	}

	GameObject SpawnPlayer()
	{
		// Initiate the player
		Vector3 r = Random.insideUnitSphere * StartPointStartCircleSize;
		Quaternion q = Quaternion.identity;
		q.SetLookRotation(-r);
		GameObject player = (GameObject)Network.Instantiate(
			PlayerPrefab,
			r,
			q,
			0);

		// Attach the camera to the new player.

		// TODO: Need to make this more generic.
		CameraObjectToAttachPlayer.GetComponent<SU_CameraFollow>().target = player.transform;
		CameraObjectToAttachPlayer.GetComponent<RotateCamera>().lookAtObj = player.transform;
		return player;
	}

	[RPC]
	void ServerCalledGameOver(string winnerIs)
	{
		winner = winnerIs;
		OnGameOver ();
	}

	[RPC]
	void JoinPlayer(string playerName)
	{
		Debug.Log ("Player joined: " + playerName);
		view.AddPlayer( playerName, ServerEntryView.PlayerState.kConnected);
		networkView.RPC ("SetPlayerData", RPCMode.Others, view.Serialize());
	}

	[RPC]
	void IsReady(string playerName)
	{
		Debug.Log ("Player is ready: " + playerName );
		view.SetPlayerState( playerName, ServerEntryView.PlayerState.kReady);
		networkView.RPC ("SetPlayerData", RPCMode.Others, view.Serialize());
	}

	[RPC]
	void SetPlayerData(string data)
	{
		Debug.Log ("Deserializing data: " + data);
		view.Deserialize(data);
	}

	[RPC]
	void SetPlayerScores(string data)
	{
		Debug.Log ("SettingScores: " + data );
		scoreBoard.Deserialize(data);
	}
}
