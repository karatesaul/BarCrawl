using UnityEngine;
using System.Collections;

public class RangedEnemy : Enemy {

	bool hasBottle;

	// Use this for initialization
	protected override void Start () {

		health = 10;
		damageDealt = 5;
		range = 2;
		hasBottle = true;

		base.Start();
	}


	protected override void makeMove ()
	{
		if (!hasBottle)
		{
			currMove = Move.None;
			hasBottle = true;
			moveCount++;
			return;
		}

		base.makeMove();
	}

	protected override bool AttemptMove (Move move)
	{
		bool success = base.AttemptMove(move);
		if (move == Move.Fight && success)
		{
			hasBottle = false;
		}

		return success;
	}
}
