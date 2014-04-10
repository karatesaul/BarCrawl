using UnityEngine;
using System.Collections;


//Hmm.  At the moment the only difference between ranged & melee enemies is values - we could make them just prefabs and not need seperate classes.
//But, I'm not going to do that.  Not yet, anyway.  We might decide that we do want more distinct behaviour.
//And anyway, this makes it easier to tweak values when we get multiple prefabs (for different bars).
public class Enemy : FightingEntity {

	protected GameObject player;
	public bool isExecuting;

	protected int maxMoves;
	protected Move currMove;
	private int timer;
	private int moveCount;

	// Use this for initialization
	protected override void Start () {
		base.Start();

		timer = 0;

		player = GameObject.Find ("Player");
		foeTag = "Player";

		currMove = Move.None;
		isExecuting = false;

		maxMoves = 2;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();


		if (health < 1) {
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
		//if in range, fight
		if((Mathf.Abs(GridManager.getX(player.transform.position) - GridManager.getX(transform.position)) <= range &&
		    Mathf.Abs(GridManager.getY(player.transform.position) - GridManager.getY(transform.position)) == 0) ||
		   (Mathf.Abs(GridManager.getX(player.transform.position) - GridManager.getX(transform.position)) == 0 &&
			Mathf.Abs(GridManager.getY(player.transform.position) - GridManager.getY(transform.position)) <= range)){
			currMove = Move.Fight;
			moveCount++;
		}
		//else, move
		else if(GridManager.getX(player.transform.position) > GridManager.getX(transform.position)){
			currMove = Move.Right;
		}
		else if(GridManager.getX(player.transform.position) < GridManager.getX(transform.position)){
			currMove = Move.Left;
		}
		else if(GridManager.getY(player.transform.position) > GridManager.getY(transform.position)){
			currMove = Move.Up;
		}
		else if(GridManager.getY(player.transform.position) < GridManager.getY(transform.position)){
			currMove = Move.Down;
		}

		bool successful = AttemptMove(currMove);


	}

	public override void Die(){
		base.Die ();
		player.GetComponent<PlayerCharacter> ().score += 100;
		player.GetComponent<TurnManager>().shortenEnemyList();
	}
}
