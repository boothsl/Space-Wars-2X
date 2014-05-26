using UnityEngine;
using System.Collections;

public class ScoreBoard : MonoBehaviour {

	public GUIStyle PlayerGuiStyle;
	public GUIStyle OwnScoreGuiStyle;

	public int OffsetFromLeft = 10;
	public int OffsetFromTop = 10;

	public int Seperator = 15;
	public int PlayerNameSize = 120;

	Hashtable playersTable = new Hashtable(); // name -> PlayerDetails.

	class PlayerDetails
	{
		public string guid;
		public int score;
	}

	public void setPlayerScore(string playerName, string guid, int score)
	{
		lock(playersTable)
		{
			if (playersTable.Contains(playerName))
			{
				PlayerDetails details = (PlayerDetails)playersTable[playerName];
				details.score = score;
			} else if (guid != null)
			{
				Debug.Log ( "Adding new player " + playerName + " guid is " + guid );
				playersTable.Add( playerName, new PlayerDetails() { guid = guid, score = score });
			}
		}
	}

	public int GetPlayerScoreByGui(string guid)
	{
		lock(playersTable)
		{
			foreach(string name in playersTable.Keys)
			{
				PlayerDetails details = (PlayerDetails)playersTable[name];
				if (guid == details.guid)
				{
					return details.score;
				}
			}
		}
		return 0;
	}

	public string GetPlayerNameByGuid(string guid)
	{
		lock(playersTable)
		{
			foreach(string name in playersTable.Keys)
			{
				PlayerDetails details = (PlayerDetails)playersTable[name];
				if (guid == details.guid)
				{
					return name;
				}
			}
		}
		return null;
	}

	public void AddToPlayerScoreByGuid(string guid, int scoreToAdd)
	{
		Debug.Log ("Adding to " + guid + " score of " + scoreToAdd );
		if (scoreToAdd > 0)
		{
			lock(playersTable)
			{
				foreach(string name in playersTable.Keys)
				{
					PlayerDetails details = (PlayerDetails)playersTable[name];
					if (guid == details.guid)
					{
						details.score += scoreToAdd;
					}
				}
			}
		}
	}
	
	string myName = null;
	public void AddPlayer(string playerName, string guid)
	{
		// Only use this to add the player on this server, the other players
		// are retrieved via the Deserialize() function.
		if (!playersTable.Contains(playerName))
		{
			Debug.Log ("Adding player : " + playerName );
			setPlayerScore(playerName,guid,0);
			myName = playerName;
		}
	}
	
	public string Serialize()
	{
		System.IO.StringWriter sw = new System.IO.StringWriter();
		lock(playersTable)
		{
			foreach(string name in playersTable.Keys)
			{
				PlayerDetails details = (PlayerDetails)playersTable[name];
				sw.Write (name);
				sw.Write (";");
				sw.Write (details.guid);
				sw.Write (";");
				sw.Write (details.score.ToString ());
				sw.Write (";");
			}
		}
		return sw.ToString ();
	}

	public void Deserialize(string data)
	{
		string[] items = data.Split (';');
		for(int i = 0; i < items.Length-1; i+=3)
		{
			setPlayerScore(items[i], items[i+1], int.Parse (items[i+2]));
		}
	}

	public void ClearPlayers()
	{
		playersTable.Clear();
	}
	
	void OnGUI()
	{
		lock(playersTable)
		{
			int x = OffsetFromLeft;
			int y = OffsetFromTop;
			foreach(string name in playersTable.Keys)
			{
				PlayerDetails details = (PlayerDetails)playersTable[name];
				if (name == myName)
				{
					GUI.Label (new Rect(x, y, PlayerNameSize, Seperator), myName, OwnScoreGuiStyle );
					GUI.Label (new Rect(x+PlayerNameSize, y, PlayerNameSize, Seperator), details.score.ToString(), OwnScoreGuiStyle );
				}
				else
				{
					GUI.Label (new Rect(x, y, x+PlayerNameSize, Seperator), name, PlayerGuiStyle );
					GUI.Label (new Rect(x+PlayerNameSize, y, PlayerNameSize, Seperator), details.score.ToString(), PlayerGuiStyle );
				}
				y += Seperator;
			}
		}
	}
}
