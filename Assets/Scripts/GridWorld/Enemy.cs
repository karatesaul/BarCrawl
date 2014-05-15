using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : FightingEntity {

	protected PlayerCharacter player;
	public bool isExecuting;

	protected int maxMoves = 2;
	protected Move currMove;
	private int timer;
	protected int moveCount;

	//lifespan = amount of turns enemy has been alive
	public int lifespan;

	private bool playerDetected;
	protected int detectionRange = 5;

	public Move toBar = Move.Up;

	// Use this for initialization
	protected override void Start () {
		base.Start();

		timer = 0;

		player = GameObject.Find ("Player").GetComponent<PlayerCharacter>();
		foeTag = "Player";

		currMove = Move.None;
		isExecuting = false;

		playerDetected = false;
		lifespan = 0;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();

		if (!isExecuting) {
			if(Input.GetKeyDown (KeyCode.O)){
				Debug.Log ("Enemy takes a turn!");
				isExecuting = true;
			}
		}
		else
		{
			timer++;
			currMove = Move.None;
			if(moveCount >= maxMoves)
			{
				Debug.Log ("Enemy's turn is over.");

				moveCount = 0;
				timer = 0;

				isExecuting = false;
				return;
			}
			if(timer > 30)
			{
				makeMove();

				moveCount++;
				timer = 0;
			}
		}
	}

	protected virtual void makeMove()
	{
		//these keep coming up, so I'm going to let them be calculated once beforehand
		int diffX = Mathf.Abs (player.x - this.x);
		int diffY = Mathf.Abs (player.y - this.y);

		if (!playerDetected) 
		{
			int distance = diffX + diffY;

			if(distance <= 5)
				playerDetected = true;


			if(lifespan >= 10)
				playerDetected = true;
			//*/
		}
		if (!playerDetected)
		{
			//at some later point, might change this to distribute the bikers more

			Debug.Log("Wandering");

			bool success = false; //to ensure they actually make a move
			int loop = 0;

			while (!success)
			{
				loop++;
				int dir = Random.Range(0, 6);
				//This is a temp fix so bikers move upward towards the bar by default.
				//dir = 2;
				switch(dir)
				{
				case 0:
					currMove = Move.Left;
					break;
				case 1:
					currMove = Move.Right;
					break;
				case 2:
					currMove = Move.Up;
					break;
				case 3:
					currMove = Move.Down;
					break;
				case 4:
				case 5:
					currMove = toBar;
					break;
				default:
					currMove = Move.None;
					break;
				}

				success = AttemptMove(currMove);
				if(loop > 10)
					return;
				//because infinite loops is bad.
			}
			return;
		}

		//reaching this point implies that the player has been detected

		//if in range, fight
		if ((diffX <= range && diffY == 0) ||
			(diffX == 0 && diffY <= range))
		{
			currMove = Move.Fight;
			moveCount++;
		}
		else 
		{
			//else, move

			//currMove = getMoveTowardTarget(x, y, player.x, player.y, range);

			//i should maybe get some actual pathfinding...
			//but for now, better derp movement

			if(diffX <= range && diffY <= range)
			{
				//if both are less than range, attempts to get in a line with the player
				//so that it can throw shit
				//this means moving on the axis it differs LESS

				if(diffX < diffY)
				{
					if (player.x > this.x) {
						currMove = Move.Right;
					}
					else if (player.x < this.x)
					{
						currMove = Move.Left;
					}
					else
					{
						Debug.Log("This shouldn't be reached! (Ranged attack should prevent it! (X))");
					}
				}
				else
				{
					if (player.y > this.y)
					{
						currMove = Move.Up;
					}
					else if (player.y < this.y)
					{
						currMove = Move.Down;
					}
					else
					{
						Debug.Log("This shouldn't be reached! (Ranged attack should prevent it! (Y))");
					}
				}
			}
			else
			{
				//if it's NOT in the range box, then it should attempt to close on the player
				//this means moving on the axis by which it differs MORE

				if(diffX > diffY)
				{
					if (player.x > this.x) {
						currMove = Move.Right;
					}
					else if (player.x < this.x)
					{
						currMove = Move.Left;
					}
					else
					{
						Debug.Log("This shouldn't be reached! (Any attack should prevent it! (X))");
					}
				}
				else
				{
					if (player.y > this.y)
					{
						currMove = Move.Up;
					}
					else if (player.y < this.y)
					{
						currMove = Move.Down;
					}
					else
					{
						Debug.Log("This shouldn't be reached! (Any attack should prevent it! (Y))");
					}
				}
			}


		}

		bool successful = AttemptMove (currMove);

		if (successful)
			return;


		//attempt to move around obstacles
		//this will likely want to be improved, but later, secretly.
		//(not actually secretly)
		//does this by attempting to turn either left or right.  If neither works, it WILL get stuck.
		Move[] moveOrder;

		if(currMove == Move.Down || currMove == Move.Up)
		{
			if (player.x > this.x) {
				moveOrder = new Move[] {Move.Right, Move.Left};
			}
			else
			{
				moveOrder = new Move[] {Move.Left, Move.Right};
			}
		}
		else
		{
			if (player.y > this.y)
			{
				moveOrder = new Move[] {Move.Up, Move.Down};
			}
			else
			{
				moveOrder = new Move[] {Move.Down, Move.Up};
			}
		}

		for (int i = 0; i < moveOrder.Length; i++) 
		{
			currMove = moveOrder [i];

			successful = AttemptMove (currMove);
			if (successful)
				return;
		}
	}

	private class AStarPoint : System.IComparable<AStarPoint>
	{
		public int cost;
		public int totalCost;
		public int x, y;

		public AStarPoint(int _cost, int _heur, int _x, int _y)
		{
			cost = _cost;
			totalCost = cost + _heur;
			x = _x;
			y = _y;
		}

		public int CompareTo(AStarPoint asp)
		{
			return totalCost - asp.totalCost;
		}
	}

	protected virtual Move getMoveTowardTarget(int x, int y, int targetX, int targetY, int range)
	{
		//herp derp how do i A*

		//apparently unity doesn't support sortedSets
		//this means I have to use a less efficient method
		// :<
		//fix plz
		List<AStarPoint> list = new List<AStarPoint> ();

		list.Add (new AStarPoint(0, heuristic (x, y, targetX, targetY, range), x, y));

		while (list.Count > 0)
		{
			list.Sort();
			//and thus efficiency dies

			AStarPoint from = list[0];
			list.RemoveAt(0);
		}

		return Move.None;
		//because compile
	}

	protected virtual int heuristic(int x, int y, int targetX, int targetY, int range)
	{
		int diffX = Mathf.Abs (x - targetX);
		int diffY = Mathf.Abs (y - targetY);

		if(diffX < range)
		{
			if(diffY < range)
			{
				if(diffX > diffY)
					return diffY;
				else
					return diffX;
			}
			return diffY;
		}
		if (diffY < range)
		{
			return diffX;
		}
		return (diffX + diffY - range);
	}

	//this override exists only for debug logging purposes.
	/*
	protected override bool AttemptMove(Move move)
	{
		Debug.Log(gameObject.name + " attempts move " + move);
		bool success = base.AttemptMove(move);
		if(success)
		{
			Debug.Log("Success!");
		}
		else
		{
			Debug.Log("Failed!");
		}
		return success;
	}
	//*/

	public override void Die(){
		base.Die ();
		player.GetComponent<PlayerCharacter> ().score += 100;
		player.GetComponent<TurnManager>().shortenEnemyList();
	}
}
