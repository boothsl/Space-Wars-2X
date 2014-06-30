using UnityEngine;
using System.Collections;

// This class displays which players are currently in the game
// and their state.
// The controller class must send the right messages and this
// will render them.
// This is sort of like a Model/View.
public class ServerEntryView : MonoBehaviour {

	public enum PlayerState
	{
		kConnected = 0,
		kReady = 1,
	}

	public int Separation = 32;
	public int OffsetFromTop = 100;
	public int OffsetFromLeft = 200;
	
	public GUIStyle ReadyGuiStyle;
	public GUIStyle OwnReadyStyle;
	public GUIStyle LargeTestGuiStyle;

	public string MyName = null;
	private PlayerState myState = PlayerState.kConnected;

	private string lastAction = null;

	private bool countdownStarted = false;
	private bool allPlayersAreReady = false;

	class Player
	{
		public string name;
		public PlayerState state;
	}

	private ArrayList players = new ArrayList();

	public event System.EventHandler MyPlayerIsReady;
	protected void OnMyPlayerIsReady(System.EventArgs e)
	{
		if (MyPlayerIsReady != null)
			MyPlayerIsReady(this, e);
	}

	public event System.EventHandler AllPlayersAreReady;
	protected void OnAllPlayersAreReady(System.EventArgs e)
	{
		if (AllPlayersAreReady != null)
			AllPlayersAreReady(this, e);
	}
	
	public event System.EventHandler GameShouldStart;
	protected void OnGameShouldStart(System.EventArgs e)
	{
		if (GameShouldStart != null)
			GameShouldStart(this, new System.EventArgs());
	}

	public void AddPlayer(string playerName, PlayerState state)
	{
		if (setPlayerState(playerName, state))
			return;
		players.Add ( new Player() { name = playerName, state = state } );
		this.lastAction = playerName + " just joined the game.";
	}

	public void ClearStates()
	{
		for(int i=0; i<players.Count; i++)
		{
			((Player)players[i]).state = PlayerState.kConnected;
		}
	}

	private void testForAllPlayersReady()
	{
		for(int i=0; i < players.Count; i++)
		{
			if (((Player)players[i]).state != PlayerState.kReady)
				return;
		}
		if (players.Count < GlobalGameState.MinimumNumberOfPlayers) return; // Need 2 players to start
		OnAllPlayersAreReady(new System.EventArgs());
	}

	private bool setPlayerState( string playerName, PlayerState state )
	{
		for(int i = 0; i < players.Count; i++)
		{
			if (((Player)players[i]).name == playerName)
			{
				((Player)players[i]).state = state;
				if (playerName == MyName)
					myState = state;

				if (state == PlayerState.kReady)
				{
					if (playerName == MyName)
					{
						this.lastAction = "You are now Ready";
					} else
						this.lastAction = playerName + " is now Ready";
				}
				testForAllPlayersReady();
				return true;
			}
		}
		return false;
	}

	public void SetPlayerState( string playerName, PlayerState state )
	{
		setPlayerState(playerName, state);
	}

	public bool AreAllPlayersReady()
	{
		for(int i = 0; i < players.Count; i++)
		{
			if (((Player)players[i]).state != PlayerState.kReady)
				return false;
		}
		return true;
	}

	public int GetPlayerCount()
	{
		return players.Count;
	}

	public string Serialize()
	{
		System.IO.StringWriter sw = new System.IO.StringWriter();

		for(int i = 0; i < players.Count; i++)
		{
			sw.Write (((Player)players[i]).name);
			sw.Write (":");
			sw.Write (((Player)players[i]).state.ToString());
			sw.Write (":");
		}

		return sw.ToString();
	}

	System.Enum PlayerStateEnumFromName(string name)
	{
		System.Type t = typeof(PlayerState);
		return (System.Enum)System.Enum.Parse(t, name);
	}

	public void Deserialize(string data)
	{
		string[] items = data.Split(':');
		for(int i = 0; i < items.Length-1; i+=2)
		{
			AddPlayer(items[i], (PlayerState)PlayerStateEnumFromName(items[i+1]));
		}
	}
	
	private float startCountTime;

	public void StartCountdown()
	{
		allPlayersAreReady = true;
		startCountTime = Time.time;
	}

	public void ResetToCountdown()
	{
		enabled = true;
		countdownStarted = true;
		startCountTime = Time.time;
	}

	void OnGUI()
	{
		int width = Screen.width;
		int height = Screen.height;

		if (countdownStarted)
		{
			// 0..1 seconds = 
			int count = (int)(Time.time - startCountTime);
			if (count < 3) // Get Ready message
			{
				GUI.TextArea( 
				             new Rect( width * 0.3f, height * 0.35f, width * 0.4f, height * 0.1f ),
				             (3-count).ToString(),
				             LargeTestGuiStyle); 
			} else if (count >= 3)
			{
				OnGameShouldStart(new System.EventArgs());
				countdownStarted = false;
			}
			return;
		}

		if (allPlayersAreReady)
		{
			// For 2 seconds display a message
			int count = (int)(Time.time - startCountTime);
			if (count < 2) // Get Ready message
			{
				lastAction = "All Players are Ready to begin...";
			} else {
				countdownStarted = true;
				startCountTime = Time.time;
			}
		}

		// Draw the overall state of the game...
		if (players.Count < GlobalGameState.MinimumNumberOfPlayers) // No one entered yet
		{
			GUI.TextArea( 
			    new Rect( width * 0.3f, height * 0.35f, width * 0.4f, height * 0.1f ),
			    "Waiting for others to join...", 
			    LargeTestGuiStyle); 
			return;
		}

		// Draw the individual states
		int x = OffsetFromLeft;
		int y = OffsetFromTop;

		foreach( Player player in players )
		{
			string text = player.name + "...";
			if (player.state == PlayerState.kConnected)
				text += "Not ready";
			else
				text += "Ready";
			GUI.TextArea (new Rect( x, y, width * 0.5f, this.Separation), 
			              text, player.name == MyName?OwnReadyStyle:ReadyGuiStyle);
			y += this.Separation;
		}

		if (lastAction != null)
		{
			GUI.TextArea (new Rect( width*0.25f, height * 0.8f, width * 0.5f, height * 0.1f),
			              this.lastAction,
			              this.LargeTestGuiStyle);
		}
	}

	void Update()
	{
		if ((Input.GetKey(KeyCode.Space) || Input.GetKey (KeyCode.Return)) && myState != PlayerState.kReady)
		{
			SetPlayerState( MyName, PlayerState.kReady );
			OnMyPlayerIsReady(new System.EventArgs());
		}
	}
}
