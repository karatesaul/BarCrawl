using UnityEngine;
using System.Collections;

public class MeleeEnemy : MeleeEntity, IEnemy {

	public GameObject player { get; set; }
	private Move currMove;
	public bool isExecuting { get; set; }
	private int timer;
	private int moveCount;

	// Use this for initialization
	protected override void Start () {
		health = 20;
		timer = 0;
		player = GameObject.Find ("Player");
		currMove = Move.None;
		isExecuting = false;

		foeTag = "Player";

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
				if((Mathf.Abs(GridManager.getX(player.transform.position) - GridManager.getX(transform.position)) == 1 &&
				    Mathf.Abs(GridManager.getY(player.transform.position) - GridManager.getY(transform.position)) == 0) ||
				   (Mathf.Abs(GridManager.getX(player.transform.position) - GridManager.getX(transform.position)) == 0 &&
				 	Mathf.Abs(GridManager.getY(player.transform.position) - GridManager.getY(transform.position)) == 1)){
					currMove = Move.Fight;
					moveCount++;
				}
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
}
