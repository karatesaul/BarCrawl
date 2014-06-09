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
	private GameObject floor;
	private GameObject bar;
	public ParticleSystem aoeEffect;

	private int shakyCam = 0;
	private float shakeSpace = 0.3f;
	private int shakeTime = 30;

	public ParticleSystem healingEffect;

	public bool executeMode;
	private int timer;

	public int maxHealth = 100;
	public int startingHealth = 100;

	public int healAmount = 5;
	public int healIncrease = 5;

	public int blunderDamage = 5;

	private Move lastMove;
	private int combo;
	private int fullCombo;

	public Sprite deathsprite;

	private bool deathFadeIsHappening = false;
	private int deathFadeCount = 0;

	public Bottle bottlePrefab;
	public Vector3 bottleOffset = new Vector3(.25f, .1f, 0);

	
	// Use this for initialization
	protected override void Start () {
		tm = GameObject.Find("Player").GetComponent<TurnManager>();
		menu = GameObject.Find("MainMenu").GetComponent<MainMenu>();
		floor = GameObject.Find("Floor");
		bar = GameObject.Find("bar");

		fillUp = false;
		health = startingHealth;
		timer = 30;
		executeMode = false;
	
		Scores.clearScores (); //there's probably a better place to put this...

		moveQueue = new List<Move> ();

		foeTag = "enemy";

		cameraOffset = new Vector3(0, -1.5f, -9);

		base.Start();

		facing = Move.Right;
	}

	public void throwFalsePunch()
	{
		animator.Play ("AttackLeft");
	}

	// Update is called once per frame
	protected override void Update () 
	{
		base.Update ();

		//camera follows player!
		worldCamera.gameObject.transform.position = gameObject.transform.position + cameraOffset;
		Vector3 correction = new Vector3(0, 0, 0);
		Vector3 cameraMin = new Vector3(worldCamera.transform.position.x - worldCamera.orthographicSize * Screen.width/Screen.height, worldCamera.transform.position.y - worldCamera.orthographicSize, 0);
		Vector3 cameraMax = new Vector3(worldCamera.transform.position.x + worldCamera.orthographicSize * Screen.width/Screen.height, worldCamera.transform.position.y + worldCamera.orthographicSize, 0);
		Vector3 floorMin = new Vector3(floor.renderer.bounds.min.x, floor.renderer.bounds.min.y, 0);
		Vector3 floorMax = new Vector3(floor.renderer.bounds.max.x, floor.renderer.bounds.max.y, 0);
		if (cameraMin.x < floorMin.x){
			correction.x += floorMin.x - cameraMin.x;
		}
		if (cameraMax.x > floorMax.x){
			correction.x += floorMax.x - cameraMax.x;
		}
		if (worldCamera.transform.position.y < floorMin.y){
			correction.y += floorMin.y - worldCamera.transform.position.y;
		}
		if (cameraMax.y > bar.renderer.bounds.max.y){
			correction.y += bar.renderer.bounds.max.y - cameraMax.y;
		}
		if (shakyCam != 0) {
			shakyCam--;
			float shake = shakeSpace * (shakyCam * 1.0f / shakeTime);
			correction.x += Random.Range(-shake, shake);
			correction.y += Random.Range(-shake, shake);
		}
		worldCamera.transform.position += correction;

		if (tm.turn == 1) 
		{
			if (!executeMode) 
			{

				//this
				//is so friggin ugly
				if(fillUp)
				{


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


					//end player turn
					executeMode = false;

					//increase scores
					Scores.turnsSurvived++;
					if(fullCombo > Scores.maxCombo)
						Scores.maxCombo = fullCombo;

					//reset combo stuff
					lastMove = Move.None;
					fullCombo = 0;
					//Debug.Log("Cleared!");

					//prevent delay when player's next turn starts
					timer = 30;

					//signal enemies to take turn.
					tm.turn = 2;
					return;
				}
				else
				{

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

		if (deathFadeIsHappening) {
//			Debug.Log ("???");
			if (deathFadeCount > 60) {
				GridManager.instance.clearEntities ();
				Application.LoadLevel ("High_Scores");
			}
			deathFadeCount++;
		}
	}

	protected override bool AttemptMove(Move move)
	{
		if (move == lastMove)
		{
			combo++;
			//Debug.Log(combo + "x COMBO!");
		}
		else
		{
			lastMove = move;
			combo = fullCombo / 4;
			//Debug.Log("Combo reset to " + combo);
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
			if(move == Move.Fight && combo > 2)
			{
				//Debug.Log("AoE triggered!");

				//play an obscenity

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
				shakyCam = shakeTime;
				Instantiate(aoeEffect, transform.position, Quaternion.identity);
				audioSources[2].volume = PlayerPrefs.GetFloat(InGameMenu.effectVolKey);
				audioSources[2].Play();
				
				if (Random.Range(0, 5) == 0)
					SFXManager.PlayerVoice();
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

						target.takeDamage(blunderDamage);
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
				//Debug.Log("Ranged attempt triggered");


				//try again with range.
				//copy of the normal fight code, because we need to know our target.
				//this is bad practice
				Vector2[] fightOrder = facing.attackOrder ();
				for(int i = 0; i < 4; i++)
				{
					
					for(int j = 1; j <= 2; j++)
					{
						
						Vector2 fightDir = fightOrder[i];
						int destX = x + ((int)fightDir.x * j);
						int destY = y + ((int)fightDir.y * j);
						
						Entity target = GridManager.instance.getTarget(destX, destY);
						if(target != null){
							if(target.gameObject.tag == foeTag){
						
								//play an obscenity IFF a bottle is actually thrown.


								target.takeDamage(damageDealt);
								
								facing = moveExtensions.getMove(fightOrder[i]);
								animator.Play("AttackLeft");
								//changing this to an actual bottle throwing animation would be nice
								//but unlikely

								Bottle proj = (Bottle)Instantiate(bottlePrefab, gameObject.transform.position + bottleOffset, Quaternion.identity);
								proj.target_x = target.x;
								proj.target_y = target.y;
								if (Random.Range(0, 5) == 0)
									SFXManager.PlayerVoice();
								return true;
							}
						}
					}
				}


			}

			return false;
		}
	}

	public override void Die()
	{
		animator.Play ("Death");

		//play an obscenity

		deathFadeIsHappening = true;
		deathFadeCount = 0;
		Camera.main.SendMessage("fadeOut");
		
		//if (Random.Range(0, 3) == 0)
		//	SFXManager.PlayerVoice();
		//Application.LoadLevel("Main_Menu");
		//TOTALSCORE = score;
		//base.Die ();
	}

	public override void takeDamage (int damage)
	{
		
		base.takeDamage (damage);
		//play an obscenity
		if (Random.Range(0, 5) == 0)
			SFXManager.PlayerVoice();

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
