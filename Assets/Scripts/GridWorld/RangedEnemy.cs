using UnityEngine;
using System.Collections;

public class RangedEnemy : Entity, IEnemy {

	public GameObject player { get; set;}
	private Move currMove;
	public bool isExecuting { get; set; }
	private int timer;
	private int moveCount;
	protected string foeTag = "Player";
	protected int damage = 5;
	protected int range = 3;
	
	// Use this for initialization
	protected override void Start () {
		health = 10;
		timer = 0;
		player = GameObject.Find ("Player");
		currMove = Move.None;
		isExecuting = false;
		
		base.Start();
	}
	
	// Update is called once per frame
	protected override void Update()
	{
		base.Update ();
		if (health < 1) {
			Die ();
		}
		
		if (!isExecuting) {
			if(Input.GetKeyDown (KeyCode.O)){
				Debug.Log ("Enemy takes a turn!");
				isExecuting = true;
			}
		}
		else{
			timer++;
			currMove = Move.None;
			if(moveCount > 1){
				Debug.Log ("Enemy's turn is over.");
				moveCount = 0;
				timer = 0;
				isExecuting = false;
				return;
			}
			if(timer > 30){
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
				AttemptMove(currMove);
				moveCount++;
				timer = 0;
			}
		}
	}

	protected override void AttemptMove(Move move)
	{
		if (move != Move.Fight)
		{
			base.AttemptMove(move);
			return;
		}

		Debug.Log (gameObject.name + "attempts to fight!");
		//handles fighting
		//this function orders the tiles so that the entity will attack those in front of it before ones to the side
		Vector2[] fightOrder = facing.attackOrder ();
		
		for(int i = 0; i < 4; i++)
		{

			for(int j = 1; j <= range; j++)
			{
			
				Vector2 fightDir = fightOrder[i];
				int destX = x + ((int)fightDir.x * j);
				int destY = y + ((int)fightDir.y * j);
				
				Entity target = GridManager.instance.getTarget(destX, destY);
				if(target != null){
					if(target.gameObject.tag == foeTag){
						//at this point, we have confirmed there is a fightable entity in this tile.
						//Attack animation code should go somewhere under this if.
						
						GridManager.instance.getTarget (destX, destY).health -= damage;
						//change color at the same time
						
						GridManager.instance.getTarget (destX, destY).currentRed = 100;
						
						
						facing = moveExtensions.getMove(fightOrder[i]);
						
						Debug.Log (gameObject.name + " attacks! " + GridManager.instance.getTarget (destX, destY).gameObject.name + " takes " + damage + " damage, and has " +
						           GridManager.instance.getTarget(destX, destY).health + " health remaining!");
						
						return;
					}
				}
			}
		}
	}
}
