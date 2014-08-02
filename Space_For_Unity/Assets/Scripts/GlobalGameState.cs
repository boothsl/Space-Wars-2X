using UnityEngine;
using System.Collections;

public class GlobalGameState : MonoBehaviour {

	static public HostData ServerLocation = null;
	static public bool IsRunningServer = true; // Default to True to allow easy debugging.
	static public string GameRoomName = System.Environment.MachineName;
	static public int MinimumNumberOfPlayers = 1; // Default to 1 to allow easy debugging.
	static public RadarSystem MainRadar;

	// This is the callback that all weapons call when it makes a hit on a
	// player (this event happens only when we're the server).
	public class HitObjectArgs : System.EventArgs {
		public int hitStrength;
		public RaycastHit hitRaycast;
		public Transform weaponObject;
		public Transform explosionEffect;
	}

	public delegate void HitObjectHandler(object sender, HitObjectArgs e);
	public static event HitObjectHandler HitObject;
	public static void OnHitObject(object sender, HitObjectArgs e)
	{
		if (IsRunningServer && HitObject != null)
			HitObject(sender, e);
	}

	// This callback is related to the player being destroyed.
	// We should wait 3 seconds and then respawn.
	public static event System.EventHandler PlayerDied;
	public static void OnPlayerDied(object sender, System.EventArgs e)
	{
		if (PlayerDied != null)
			PlayerDied(sender, e);
	}

	public const string GameType = "Space-Wars-2X-GameLab";
}
