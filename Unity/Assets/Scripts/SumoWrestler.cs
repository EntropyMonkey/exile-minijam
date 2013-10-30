using UnityEngine;
using System.Collections;

public class SumoWrestler : MonoBehaviour
{
	PLAYER id;

	private System.Action currentState;

	// Use this for initialization
	void Start()
	{
		id = GameState.Instance.NextPlayerId;
		currentState = Idle;
	}

	// Update is called once per frame
	void Update()
	{
		currentState();
	}

	void Idle()
	{

	}

	void Walk()
	{

	}

	void Charge()
	{

	}

	void Jump()
	{

	}
}
