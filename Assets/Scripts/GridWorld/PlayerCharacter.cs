using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCharacter : FightingEntity {

	public Move[] movelist;
	public TokenType[] moveInput;
	public bool fillUp;
	public TurnManager tm;
	public MainMenu menu;
	private int currCombo;
	public bool executeMode;
	public int score;
	private bool exit;
	private int timer;
	public int startingHealth = 100;
	public Vector3 cameraOffset;
	public Camera worldCamera;
	public GameObject backdrop;
	public Vector3 backdropOffset;
	public int blunderDamage = 5;
	
	// Use this for initialization
	protected override void Start () {
		tm = GameObject.Find("Player").GetComponent<TurnManager>();
		menu = GameObject.Find("MainMenu").GetComponent<MainMenu>();
		backdrop = GameObject.Find ("WoodBackdrop");
		exit = false;
		fillUp = false;
		score = 0;
		health = startingHealth;
		timer = 30;
		executeMode = false;
		movelist = new Move[20];
		currCombo = 0;
		for(int i = 0; i < 20; i++){
			movelist[i] = Move.None;
		}

		foeTag = "enemy";

		cameraOffset = new Vector3(0, -1.5f, -9);
		backdropOffset = new Vector3 (0, -3, 0);
		base.Start();

	}



	// Update is called once per frame
	protected override void Update () 
	{
		base.Update ();
		worldCamera.gameObject.transform.position = gameObject.transform.position + cameraOffset;
		backdrop.gameObject.transform.position = gameObject.transform.position + backdropOffset;

		if (tm.turn == 1) 
		{
			if (!executeMode) 
			{
				if(fillUp){
			for(int i = 0; i < 20; i++){
				if(moveInput.Length == i){
					Debug.Log ("ran out of moves at " + i);
					break;
				}
				switch(moveInput[i]){
				case TokenType.Left:
					Debug.Log ("Left move queued.");
					movelist[i] = Move.Left;
					//score = score + 25;
					break;
				case TokenType.Right:
					Debug.Log ("Right move queued.");
					movelist[i] = Move.Right;
					//score = score + 25;
					break;
				case TokenType.Up:
					Debug.Log ("Up move queued.");
					movelist[i] = Move.Up;
					//score = score + 25;
					break;
				case TokenType.Down:
					Debug.Log ("Down move queued.");
					movelist[i] = Move.Down;
					//score = score + 25;
					break;
				case TokenType.Attack:
					Debug.Log ("Fight move queued.");
					movelist[i] = Move.Fight;
					score = score + 25;
					break;
				default:
					Debug.Log ("No move queued.");
					movelist[i] = Move.None;
					break;

				}
			}
			fillUp = false;
			executeMode = true;
						}
		/*
						if (Input.GetKeyDown (KeyCode.LeftArrow)) {
								if (currCombo < 10) {
										movelist [currCombo] = Move.Left;
										currCombo++;
										Debug.Log ("Left move queued.");
								} else
										Debug.Log ("Queue full!");
								//AttemptMove (Move.Left);
						}
						if (Input.GetKeyDown (KeyCode.RightArrow)) {
								if (currCombo < 10) {
										movelist [currCombo] = Move.Right;
										currCombo++;
										Debug.Log ("Right move queued.");
								} else
										Debug.Log ("Queue full!");
								//AttemptMove (Move.Right);
						}
						if (Input.GetKeyDown (KeyCode.UpArrow)) {
								if (currCombo < 10) {
										movelist [currCombo] = Move.Up;
										currCombo++;
										Debug.Log ("Up move queued.");
								} else
										Debug.Log ("Queue full!");
								//AttemptMove (Move.Up);
						}
						if (Input.GetKeyDown (KeyCode.DownArrow)) {
								if (currCombo < 10) {
										movelist [currCombo] = Move.Down;
										currCombo++;
										Debug.Log ("Down move queued.");
								} else
										Debug.Log ("Queue full!");
								//AttemptMove (Move.Down);
						}
						if (Input.GetKeyDown (KeyCode.F)) {
								if(currCombo < 10){
									movelist [currCombo] = Move.Fight;
									currCombo++;
									Debug.Log ("Fight move queued.");
								}
								else
									Debug.Log ("Queue full!");
								//AttemptMove (Move.Down);
						}
						if (Input.GetKeyDown (KeyCode.Space)) {
								currCombo = 0;
								executeMode = true;
								tm.turn = 1;
								Debug.Log ("Execute mode entered.");
						}
				*/
			} 
			else 
			{
				if (currCombo > 19 || currCombo < 0) {
					currCombo = 0;
					exit = true;
				}
				if (exit || movelist [currCombo] == Move.None) {
					for (int i = 0; i < 20; i++) {
							movelist [i] = Move.None;
					}
					//Debug.Log ("Execute mode exited.");
					exit = false;
					currCombo = 0;
					timer = 30;
					executeMode = false;
					tm.turn = 2;
					return;
				}
				timer++;
				if (timer > 30) {
					AttemptMove (movelist [currCombo]);
					currCombo++;
					timer = 0;
				}
			}
		}

			//make sure the sprite is the right color

	}

	protected override bool AttemptMove(Move move)
	{
		if (move == Move.Heal) {
			health += 5;
			return true;
		}
		else 
		{
			bool success = base.AttemptMove (move);
			if(success)
				return true;
			//else

			if(move.isDirectional())
			{
				facing = move;

				//find the destination tile
				Vector2 moveDir = move.getDirection();
				int destX = x + (int)moveDir.x;
				int destY = y + (int)moveDir.y;

				Entity target = GridManager.instance.getTarget(destX, destY);

				if(target != null)
				{
					if(target.gameObject.tag == foeTag)
					{
						//a fightable enemy!
						//deal it some damage.

						target.health -= blunderDamage;

						Debug.Log("Player blunders into " + target.gameObject.name + ", dealing " + blunderDamage + " damage!  " + target.health + " health remains.");

						target.currentRed = 100;

						return true;
					}
				}

				return false;
			}
			else if(move == Move.Fight)
			{
				//do nothing, yet, anyway.  We'll see what we're doing with this.
			}

			return false;
		}
	}

	public override void Die()
	{
		GridManager.instance.clearEntities();

		Application.LoadLevel("Main_Menu");
	}
}
