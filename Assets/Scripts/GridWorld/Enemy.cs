using UnityEngine;
using System.Collections;

public class Enemy : Entity {
	public GameObject player;
	private Move currMove;
	public bool isExecute;
	private int timer;
	private int moveCount;
	// Use this for initialization
	protected override void Start () {
		timer = 30;
		player = GameObject.Find ("lamePC");
		currMove = Move.None;
		isExecute = false;

		base.Start();
	}
	
	// Update is called once per frame
	void Update()
	{

		if (!isExecute) {
			if(Input.GetKeyDown (KeyCode.O)){
				Debug.Log ("Enemy takes a turn!");
				isExecute = true;
			}
		}
		else{
			timer++;
			currMove = Move.None;
			if(moveCount > 1){
				Debug.Log ("Enemy's turn is over.");
				moveCount = 0;
				timer = 30;
				isExecute = false;
				return;
			}
			if(timer > 30){
				if(Mathf.Abs(GridManager.getX(player.transform.position) - GridManager.getX(transform.position)) == 1 ||
				   Mathf.Abs(GridManager.getY(player.transform.position) - GridManager.getX(transform.position)) == 1){
					currMove = Move.Fight;
					Debug.Log ("Enemy attacks!");
					moveCount++;
				}
				else if(GridManager.getY(player.transform.position) > GridManager.getY(transform.position)){
					currMove = Move.Up;
				}
				else if(GridManager.getY(player.transform.position) < GridManager.getY(transform.position)){
					currMove = Move.Down;
				}
				else if(GridManager.getX(player.transform.position) > GridManager.getX(transform.position)){
					currMove = Move.Right;
				}
				else if(GridManager.getX(player.transform.position) < GridManager.getX(transform.position)){
					currMove = Move.Left;
				}
				AttemptMove (currMove);
				moveCount++;
				timer = 0;
			}
		}
	}
}
