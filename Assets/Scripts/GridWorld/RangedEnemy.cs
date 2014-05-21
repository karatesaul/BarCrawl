using UnityEngine;
using System.Collections;

public class RangedEnemy : Enemy {

	bool hasBottle;

	public Bottle prefab;
	public Vector3 bottleOffset = new Vector3(.25f, .1f, 0);

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
			Bottle proj = (Bottle)Instantiate(prefab, gameObject.transform.position + bottleOffset, Quaternion.identity);

			//this is kind of unwise, but it works
			proj.target_x = player.x;
			proj.target_y = player.y;

			hasBottle = false;
		}

		return success;
	}
}
