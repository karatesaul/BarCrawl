using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurnManager : MonoBehaviour {

	public PlayerCharacter player;
	public PuzzleManager puzzle;
	public int enemyCount;
	public List<Enemy> enemies;
	public int spawnTimer;
	public int coolDownTimer;

	public Tile[] enemySpawnLocations;
	public MeleeEnemy meleeEnemyReference;
	public RangedEnemy rangedEnemyReference;
	public GameObject meleeprefab;
	public GameObject rangedprefab;

	public Texture gray;
	public int maxEnemies;



	//0 = none
	//1 = player turn
	//2 = enemy turn
	public int turn;
	//private bool canMove;

	// Use this for initialization
	void Start () {
		maxEnemies = 4;
		meleeEnemyReference = meleeprefab.GetComponent<MeleeEnemy> ();
		rangedEnemyReference = rangedprefab.GetComponent<RangedEnemy> ();
		GameObject[] enemyObjs; 
		//gray;// = GameObject.Find ("Grayout").GetComponent<SpriteRenderer>();

		player = GameObject.Find("Player").GetComponent<PlayerCharacter>();
		puzzle = GameObject.Find ("PuzzleManager").GetComponent<PuzzleManager> ();
		enemyObjs = GameObject.FindGameObjectsWithTag("enemy");
		enemies = new List<Enemy>();
		for (int i = 0; i < enemyObjs.Length; i++) {
			enemies.Add(enemyObjs[i].GetComponent<Enemy>());
		}
		//canMove = true;
		spawnTimer = 0;
		coolDownTimer = 5;
		turn = 1;
		enemyCount = 0;
	}

	/*
	 * NEW CODE in Update:
	 * || Scores.turnsSurvived > 50
	 * 
	 * that's literally it
	 */

	// Update is called once per frame
	void Update () {
			if (turn == 1) {
			//Debug.Log ("PLAYER TURN");
			//turn is set within PlayerCharacter.cs after exiting executeMode and attempting to move
			/*	canMove = true;
				foreach(Enemy enemy in enemies){
					if(enemy!=null){
						if(enemy.isExecuting==true){
							canMove = false;
						}
					}
				}
				if(canMove && puzzle.refillStep == 4){
				Debug.Log ("You can go now!");
					puzzle.endTurn ();
				}*/
			//Vector4 x = new Vector4(0,0,0,0);
			//gray.color = x;
		} else if (turn == 2) {
			//Vector4 x = new Vector4(0,0,0,5);
			//gray.color = x;
			//Debug.Log ("ENEMY TURN");
			//player.score/100 = enemies defeated.

			Enemy enemyInstance;

			//NEW CODE: || Scores.turnsSurvived > 50

			if(((spawnTimer > 2 && enemyCount < (Scores.total/100) && enemyCount < maxEnemies) || enemyCount < 0) || Scores.turnsSurvived > 50)
			{

				 //This should instead be called in Enemy, but I'll move it there later.
		
				for(int i=0; i<10; i++)
				{
					int rand = Random.Range(0, enemySpawnLocations.Length);
					Tile spawnLoc = enemySpawnLocations[rand];

					if(GridManager.instance.isPassable(spawnLoc.x, spawnLoc.y))
					{
						if(Random.Range(0,100) >= 15){
							enemyInstance = Instantiate (meleeEnemyReference, new Vector2(2, 0), new Quaternion(0,0,0,0)) as Enemy;
						}
						else{
							enemyInstance = Instantiate (rangedEnemyReference, new Vector2(2, 0), new Quaternion(0,0,0,0)) as Enemy;
						}
						enemyInstance.x = spawnLoc.x;
						enemyInstance.y = spawnLoc.y;
						enemyInstance.transform.position = GridManager.getTransformPosition(spawnLoc.x, spawnLoc.y);

						enemies.Add(enemyInstance.GetComponent<Enemy>());
						
						spawnTimer = 0;
						break;
					}
				}

			}

			foreach(Enemy enemy in enemies)
			{
				if(enemy != null){
					enemy.isExecuting = true;
					enemy.lifespan++;
				}
			}
			//canMove = false;

			turn = 3;
			spawnTimer++;
			coolDownTimer++;
		}
		else if(turn == 3){
			//Actual enemy execute turn
			turn = 1;
			foreach(Enemy enemy in enemies){
				if(enemy != null){
					if(enemy.isExecuting == true){
						turn = 3;
					}
				}
			}
			if(turn == 1)
				puzzle.endTurn ();

		}

	}

	public void shortenEnemyList(){
		//this needs to be rewritten, (or code with a similar function), to work now that enemies is a List.  But for now, it should work without it.

		for(int i = 1; i < enemies.Count; i++){
			if(enemies[i-1] == null){
				enemies[i-1] = enemies[i];
				enemies[i] = null;
			}
		}

	}
}
