using UnityEngine;
using System.Collections;

// Right is the POSITIVE X
// Up is the POSITIVE Y
//not that you need to remember that, because we have a conversion function.
public enum Move {None, Left, Right, Up, Down, Fight};

public static class moveExtensions
{

	/// <summary>
	/// Gets the direction of the move, with a magnitude of 1.  Since there are no diagonals, this should always result in integer values.
	/// </summary>
	/// <returns>The direction.</returns>
	/// <param name="move">The move to convert to a direction.</param>
	public static Vector2 getDirection(this Move move)
	{
		switch (move) 
		{
		case Move.Down:
			return new Vector2(0, -1);
		case Move.Up:
			return new Vector2(0, 1);
		case Move.Right:
			return new Vector2(1, 0);
		case Move.Left:
			return new Vector2(-1, 0);
		case Move.Fight:
			Debug.Log("Attack!");
			return new Vector2(0, 0);
		case Move.None:
			return new Vector2(0, 0);
		default:
			return new Vector2(760, 3589);
		}
	}
}