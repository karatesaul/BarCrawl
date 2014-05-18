using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCharacter : FightingEntity {

	public List<Move> moveQueue;

	//oh my god, that is ugly.
	public TokenType[] moveInput; 
	public bool fillUp;

	public TurnManager tm;
	public MainMenu menu;
	public Vector3 cameraOffset;
	public Camera worldCamera;
	public ParticleSystem healingEffect;

	public bool executeMode;
	private int timer;
	
	public int score;

	public int maxHealth = 100;
	public int startingHealth = 100;

	public int healAmount = 5;
	public int healIncrease = 5;

	public int blunderDamage = 5;

	private Move lastMove;
	private int combo;
	private int fullCombo;

	
	// Use this for initialization
	protected override void Start () {
		tm = GameObject.Find("Player").GetComponent<TurnManager>();
		menu = GameObject.Find("MainMenu").GetComponent<MainMenu>();

		fillUp = false;
		score = 0;
		health = startingHealth;
		timer = 30;
		executeMode = false;
	

		moveQueue = new List<Move> ();

		foeTag = "enemy";

		cameraOffset = new Vector3(0, -1.5f, -9);

		base.Start();

	}



	// Update is called once per frame
	protected override void Update () 
	{
		base.Update ();

		//camera follows player!
		worldCamera.gameObject.transform.position = gameObject.transform.position + cameraOffset;

		if (tm.turn == 1) 
		{
			if (!executeMode) 
			{

				//this
				//is so friggin ugly
				if(fillUp)
				{
					lastMove = Move.None;
					fullCombo = 0;

					for(int i = 0; i < moveInput.Length; i++)
					{
						switch(moveInput[i]){
						case TokenType.Left:
							//Debug.Log ("Left move queued.");
							moveQueue.Add(Move.Left);
							//score = score + 25;
							break;
						case TokenType.Right:
							//Debug.Log ("Right move queued.");
							moveQueue.Add(Move.Right);
							//score = score + 25;
							break;
						case TokenType.Up:
							//Debug.Log ("Up move queued.");
							moveQueue.Add(Move.Up);
							//score = score + 25;
							break;
						case TokenType.Down:
							//Debug.Log ("Down move queued.");
							moveQueue.Add(Move.Down);
							//score = score + 25;
							break;
						case TokenType.Attack:
							//Debug.Log ("Fight move queued.");
							moveQueue.Add(Move.Fight);
							//score = score + 25;
							break;
						case TokenType.Heal:
							//Debug.Log ("Heal move queued.");
							moveQueue.Add(Move.Heal);
							break;
						default:
							Debug.Log ("Move.None queued.  Why did this happen?");
							moveQueue.Add(Move.None);
							break;

						}
					}
					fillUp = false;
					executeMode = true;
				}
			} 
			else 
			{
				//this section: executeMode == true;

				if (moveQueue.Count == 0) {

					Debug.Log ("Player queue emptied.");

					//end player turn
					executeMode = false;

					//prevent delay when player's next turn starts
					timer = 30;

					//signal enemies to take turn.
					tm.turn = 2;
					return;
				}

				//will probably want to change this to be controlled by the animations
				//you know, once we have walking animations in
				timer++;
				if (timer > 30) {

					AttemptMove(moveQueue[0]);
					moveQueue.RemoveAt(0);
					timer = 0;
				}
			}
		}

	}

	protected override bool AttemptMove(Move move)
	{
		if (move == lastMove)
		{
			combo++;
		}
		else
		{
			lastMove = move;
			combo = fullCombo / 4;
		}
		fullCombo++;

		if (move == Move.Heal) {
			health += healAmount + (combo * healIncrease);
			//1 and 2 are 5 less than before; 3 is the same; 4 and higher are more.
			if(health > maxHealth)
				health = maxHealth;
			Instantiate(healingEffect, transform.position + new Vector3(0, 0, .5f), Quaternion.identity);
			return true;
		}
		else 
		{
			bool success = false;
			if(move == Move.Fight && combo >= 3)
			{
				//i.e. the fourth or later punch in a row
				List<Entity> closeEntities = GridManager.instance.getEntitiesInRect(x - 2, x + 2, y + 2, y - 2);

				foreach(Entity entity in closeEntities)
				{
					if(entity != null)
					{
						if(entity.gameObject.tag == foeTag)
						{
							entity.takeDamage(10);

							success = true;
						}
					}
				}

				animator.Play("AttackDown");
				return success;
			}

			success = base.AttemptMove (move);
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
						animator.Play ("AttackLeft");

						//Debug.Log("Player blunders into " + target.gameObject.name + ", dealing " + blunderDamage + " damage!  " + target.health + " health remains.");

						target.currentRed = 100;

						return true;
					}
				}

				return false;
			}
			else if(move == Move.Fight && combo > 0)
			{
				//try again with range.

				range = 2;

				base.AttemptMove(move);

				range = 1;
			}

			return false;
		}
	}

	public override void Die()
	{
		GridManager.instance.clearEntities();

		//Application.LoadLevel("Main_Menu");
		Application.LoadLevel ("High_Scores");
	}

	public void OnGUI(){
		//draw the queue of moves
		//center-align the queue
//		int length = 0;
//		for (int i=0; i<20; i++) {
//			if (moveQueue[i] != Move.None) length++;
//		}
//
		float centerX = Screen.width/2 - (moveQueue.Count * Screen.width/16)/2;
		if (centerX < 0) centerX = 0f;
		float centerY = Screen.width * 5/6 - Screen.width/6;
		int index = 0;
		foreach (Move m in moveQueue) {
			Texture sprite = null;

			switch (m){
			case Move.Up:
				sprite = GameObject.Find("PuzzleManager").GetComponent<PuzzleManager>().tokenUp;
				break;
			case Move.Down:
				sprite = GameObject.Find("PuzzleManager").GetComponent<PuzzleManager>().tokenDown;
				break;
			case Move.Left:
				sprite = GameObject.Find("PuzzleManager").GetComponent<PuzzleManager>().tokenLeft;
				break;
			case Move.Right:
				sprite = GameObject.Find("PuzzleManager").GetComponent<PuzzleManager>().tokenRight;
				break;
			case Move.Fight:
				sprite = GameObject.Find("PuzzleManager").GetComponent<PuzzleManager>().tokenAttack;
				break;
			case Move.Heal:
				sprite = GameObject.Find("PuzzleManager").GetComponent<PuzzleManager>().tokenHeal;
				break;
			default:
				break;
			}

			GUI.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
			GUI.DrawTexture(new Rect(centerX + Screen.width/16 * index, centerY, Screen.width/16, Screen.width/16), sprite);
			index++;
		}

	}
}
