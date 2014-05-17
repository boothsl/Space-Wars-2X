using UnityEngine;
using System.Collections;

/// <summary>
/// Game manager
/// </summary>
public class GameController : MonoBehaviour {

	public enum GameState
	{
		Active,
		GameOver,
	}

	private static GameController _Instance;

	public static GameController Instance {
		get { return _Instance; }
	}

	// Game over object(s) to enable on end game
	public GameObject[] gameOver;

	public GameState state = GameState.Active;

	void Awake() {
		_Instance = this;
	}

	public void GameOver()
	{
		if (state != GameState.GameOver)
		{
			state = GameState.GameOver;
			foreach (GameObject obj in gameOver)
			{
				if (obj != null)
					obj.SetActive(true);
			}
		}
	}
}
