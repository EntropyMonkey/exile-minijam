using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour
{

	public GameState Instance
	{
		get
		{
			if (instance == null)
				instance = this;
			return instance;
		}
	}
	private GameState instance;

	public int[] PlayerPoints = new int[4];

	// Use this for initialization
	void Start()
	{
		for (int i = 0; i < 4; ++i)
		{

		}
	}

	// Update is called once per frame
	void Update()
	{

	}
}
