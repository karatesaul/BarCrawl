using UnityEngine;
using System.Collections;

// Right is the POSITIVE X
// Up is the POSITIVE Y
//not that you need to remember that, because we have a conversion function.
public enum Move {None, Left, Right, Up, Down, Fight, Heal};

//This class allows us to make methods that can be called using Move.whatever
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
		case Move.None:
		case Move.Heal:
			return new Vector2(0, 0);
		default:
			return new Vector2(760, 3589);
		}
	}

	public static void getMove(this Move move, Vector2 direction){
		if (direction.x == 0 && direction.y == -1) {
			move = Move.Down;
			return;
		}
		if (direction.x == 0 && direction.y == 1) {
			move = Move.Up;
			return;
		}
		if (direction.x == 1 && direction.y == 0) {
			move = Move.Right;
			return;
		}
		if (direction.x == -1 && direction.y == 0) {
			move = Move.Left;
			return;
		}
		move = Move.None;
		return;

	}

	public static Move getMove(Vector2 direction){
		if (direction.x == 0 && direction.y == -1) {
			return Move.Down;
		}
		if (direction.x == 0 && direction.y == 1) {
			return Move.Up;
		}
		if (direction.x == 1 && direction.y == 0) {
			return Move.Right;
		}
		if (direction.x == -1 && direction.y == 0) {
			return Move.Left;
		}
		return Move.None;
	}

	public static Vector2[] attackOrder(this Move move){
		Vector2[] ret = new Vector2[4];
		switch (move) {
		case Move.Down:
			ret[0] = new Vector2(0, -1);
			ret[1] = new Vector2(1, 0);
			ret[2] = new Vector2(-1, 0);
			ret[3] = new Vector2(0, 1);
			break;
		case Move.Up:
			ret[0] = new Vector2(0, 1);
			ret[1] = new Vector2(-1, 0);
			ret[2] = new Vector2(1, 0);
			ret[3] = new Vector2(0, -1);
			break;
		case Move.Left:
			ret[0] = new Vector2(-1, 0);
			ret[1] = new Vector2(0, 1);
			ret[2] = new Vector2(0, -1);
			ret[3] = new Vector2(1, 0);
			break;
		case Move.Right:
			ret[0] = new Vector2(1, 0);
			ret[1] = new Vector2(0, -1);
			ret[2] = new Vector2(0, 1);
			ret[3] = new Vector2(-1, 0);
			break;
		default:
			ret[0] = new Vector2(0, -1);
			ret[1] = new Vector2(1, 0);
			ret[2] = new Vector2(-1, 0);
			ret[3] = new Vector2(0, 1);
			break;
		}
		return ret;
	}

	public static Move[] getPerpendicular(this Move move)
	{
		switch(move) {
		case Move.Down:
		case Move.Up:
			return new Move[2] {Move.Left, Move.Right};
		case Move.Left:
		case Move.Right:
			return new Move[2] {Move.Up, Move.Down};
		default:
			return null;
		}
	}

	public static bool isDirectional(this Move move)
	{
		switch (move)
		{
		case Move.Down:
		case Move.Left:
		case Move.Right:
		case Move.Up:
			return true;
		case Move.None:
		case Move.Fight:
		default:
			return false;
		}
	}
}