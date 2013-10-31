using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour
{
	public static GameState Instance
	{
		get
		{
			if (instance == null)
				instance = FindObjectOfType(typeof(GameState)) as GameState;
			return instance;
		}
	}
	private static GameState instance;

	[SerializeField]
	private float _sumoRingRadius = 10;
	public float SumoRingRadius { get { return _sumoRingRadius; } }

	private ChainJam.PLAYER lastInitiatedPlayer = 0;
	public ChainJam.PLAYER NextPlayerId
	{
		get
		{
			lastInitiatedPlayer++;
			return lastInitiatedPlayer - 1;
		}
	}

	// Use this for initialization
	void Start()
	{
	}

	public void PlayerPushedOut(SumoWrestler defendant, SumoWrestler aggressor)
	{
		defendant.Restart();
		if (aggressor)
		{
			ChainJam.AddPoints(aggressor.ID, 1);
		}
	}
}
