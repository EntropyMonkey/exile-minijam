using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;

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

	private ChainJam.PLAYER nextPlayerId = ChainJam.PLAYER.PLAYER1;
	public ChainJam.PLAYER NextPlayerId
	{
		get
		{
			Debug.Log("player id: " + nextPlayerId);
			return nextPlayerId++;
		}
	}

	private SumoWrestler[] wrestlers;

	private int[] points = new int[4];
	private TextMesh[] pointMesh = new TextMesh[4];

	public static ChainJam.PLAYER GetPlayerFromController(PlayerIndex controller)
	{
		switch(controller)
		{
			case PlayerIndex.One: return ChainJam.PLAYER.PLAYER1;
			case PlayerIndex.Two: return ChainJam.PLAYER.PLAYER2;
			case PlayerIndex.Three: return ChainJam.PLAYER.PLAYER3;
			case PlayerIndex.Four: return ChainJam.PLAYER.PLAYER4;
			default: return ChainJam.PLAYER.PLAYER1;
		}
	}

	public static PlayerIndex GetControllerFromPlayerId(ChainJam.PLAYER player)
	{
		switch (player)
		{
			case ChainJam.PLAYER.PLAYER1: return PlayerIndex.One;
			case ChainJam.PLAYER.PLAYER2: return PlayerIndex.Two;
			case ChainJam.PLAYER.PLAYER3: return PlayerIndex.Three;
			case ChainJam.PLAYER.PLAYER4: return PlayerIndex.Four;
			default: return PlayerIndex.One;
		}
	}

	// Use this for initialization
	void Start()
	{
		//Transform cylinder = transform.FindChild("Cylinder");
		//cylinder.localScale
		//	= new Vector3(_sumoRingRadius * 2, 0, _sumoRingRadius * 2);

		wrestlers = FindObjectsOfType(typeof(SumoWrestler)) as SumoWrestler[];

		for (int i = 0; i < 4; i++)
		{
			Transform c = transform.FindChild("Score" + (i + 1));
			if (c)
			{
				pointMesh[i] = c.GetComponent<TextMesh>();
				pointMesh[i].color = GetColor(wrestlers[i].ID);
			}
		}
	}

	Color GetColor(ChainJam.PLAYER id)
	{
		switch(id)
		{
			case ChainJam.PLAYER.PLAYER1: return new Color(49.0f / 255, 50.0f / 255, 50.0f / 255);
			case ChainJam.PLAYER.PLAYER2: return new Color(39.0f / 255, 173.0f / 255, 227.0f / 255);
			case ChainJam.PLAYER.PLAYER3: return new Color(238.0f / 255, 54.0f / 255, 138.0f / 255);
			case ChainJam.PLAYER.PLAYER4: return new Color(176.0f / 255, 209.0f / 255, 54.0f / 255);
		}

		return Color.black;
	}

	public void PlayerPushedOut(SumoWrestler defendant, SumoWrestler aggressor)
	{
		defendant.Restart();
		if (aggressor)
		{
			ChainJam.AddPoints(aggressor.ID, 1);
			points[(int)aggressor.ID] += 1;
		}
	}

	void Update()
	{
		for (int i = 0; i < 4; i++)
		{
			if (pointMesh[(int)wrestlers[i].ID] != null)
			{
				pointMesh[(int)wrestlers[i].ID].text = points[i].ToString();
			}
		}
	}

	public IEnumerator ShakeLevel(float strength)
	{
		float timer = 0.3f;
		while (timer > 0)
		{
			timer -= Time.deltaTime;
			transform.Rotate(Vector3.forward * Random.Range(-strength, strength));

			yield return new WaitForEndOfFrame();
		}
	}
}
