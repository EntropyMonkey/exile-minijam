using UnityEngine;
using System.Collections;

public class SumoInput : MonoBehaviour
{
	private static KeyCode GetKeycode(PLAYER player, BUTTON button)
	{
		KeyCode key = KeyCode.Space;
		switch (player)
		{
			case PLAYER.PLAYER1:
				switch (button)
				{
					case BUTTON.A:
						key = KeyCode.Z;
						break;
					case BUTTON.B:
						key = KeyCode.X;
						break;
					case BUTTON.LEFT:
						key = KeyCode.LeftArrow;
						break;
					case BUTTON.RIGHT:
						key = KeyCode.RightArrow;
						break;
					case BUTTON.UP:
						key = KeyCode.UpArrow;
						break;
					case BUTTON.DOWN:
						key = KeyCode.DownArrow;
						break;
				}
				break;
			case PLAYER.PLAYER2:
				switch (button)
				{
					case BUTTON.A:
						key = KeyCode.N;
						break;
					case BUTTON.B:
						key = KeyCode.M;
						break;
					case BUTTON.LEFT:
						key = KeyCode.J;
						break;
					case BUTTON.RIGHT:
						key = KeyCode.L;
						break;
					case BUTTON.UP:
						key = KeyCode.I;
						break;
					case BUTTON.DOWN:
						key = KeyCode.K;
						break;
				}
				break;
			case PLAYER.PLAYER3:
				switch (button)
				{
					case BUTTON.A:
						key = KeyCode.Q;
						break;
					case BUTTON.B:
						key = KeyCode.E;
						break;
					case BUTTON.LEFT:
						key = KeyCode.A;
						break;
					case BUTTON.RIGHT:
						key = KeyCode.D;
						break;
					case BUTTON.UP:
						key = KeyCode.W;
						break;
					case BUTTON.DOWN:
						key = KeyCode.S;
						break;
				}
				break;
			case PLAYER.PLAYER4:
				switch (button)
				{
					case BUTTON.A:
						key = KeyCode.R;
						break;
					case BUTTON.B:
						key = KeyCode.Y;
						break;
					case BUTTON.LEFT:
						key = KeyCode.F;
						break;
					case BUTTON.RIGHT:
						key = KeyCode.H;
						break;
					case BUTTON.UP:
						key = KeyCode.T;
						break;
					case BUTTON.DOWN:
						key = KeyCode.G;
						break;
				}
				break;
		}
		return key;
	}
}
