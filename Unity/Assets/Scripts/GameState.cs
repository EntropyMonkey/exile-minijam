using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour
{

	public static GameState Instance
	{
		get
		{
			if (instance == null)
				instance = this;
			return instance;
		}
	}
	private static GameState instance;

	private PLAYER lastInitiatedPlayer = 0;
	public PLAYER NextPlayerId
	{
		get
		{
			lastInitiatedPlayer++;
			return lastInitiatedPlayer - 1;
		}
	}

	public int[] PlayerPoints = new int[4];

	// Use this for initialization
	void Start()
	{
		for (int i = 0; i < 4; ++i)
		{
			PlayerPoints[i] = 0;
		}
	}
}
