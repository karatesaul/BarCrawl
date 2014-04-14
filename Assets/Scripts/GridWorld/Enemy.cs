using UnityEngine;
using System.Collections;


//Hmm.  At the moment the only difference between ranged & melee enemies is values - we could make them just prefabs and not need seperate classes.
//But, I'm not going to do that.  Not yet, anyway.  We might decide that we do want more distinct behaviour.
//And anyway, this makes it easier to tweak values when we get multiple prefabs (for different bars).
public class Enemy : FightingEntity {

	protected PlayerCharacter player;
	public bool isExecuting;

	protected int maxMoves = 2;
	protected Move currMove;
	private int timer;
	private int moveCount;

	private bool playerDetected;
	protected int detectionRange = 5;

	// Use this for initialization
	protected override void Start () {
		base.Start();

		timer = 0;

		player = GameObject.Find ("Player").GetComponent<PlayerCharacter>();
		foeTag = "Player";

		currMove = Move.None;
		isExecuting = false;

		playerDetected = false;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();


		if (health <= 0) {
			Die ();
		}
		
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

	protected void makeMove()
	{
		//these keep coming up, so I'm going to let them be calculated once beforehand
		int diffX = Mathf.Abs (player.x - this.x);
		int diffY = Mathf.Abs (player.y - this.y);

		if (!playerDetected) 
		{
			int distance = diffX + diffY;

			if(distance <= 5)
				playerDetected = true;
		}
		if (!playerDetected)
		{
			//at some later point, might change this to distribute the bikers more

			int dir = Random.Range(0, 4);

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
			default:
				currMove = Move.None;
				break;
			}

			AttemptMove(currMove);
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
			//i should really get some actual pathfinding...
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

	public override void Die(){
		base.Die ();
		player.GetComponent<PlayerCharacter> ().score += 100;
		player.GetComponent<TurnManager>().shortenEnemyList();
	}
}
