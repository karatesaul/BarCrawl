using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurnManager : MonoBehaviour {

	public PlayerCharacter player;
	public int enemyCount;
	public List<Enemy> enemies;
	public int spawnTimer;
	private GameObject enemyInstance;
	public GameObject enemyReference;
	public int maxEnemies;

	//0 = none
	//1 = player turn
	//2 = enemy turn
	public int turn;

	// Use this for initialization
	void Start () {
		maxEnemies = 4;
		GameObject[] enemyObjs; 

		player = GameObject.Find("Player").GetComponent<PlayerCharacter>();

		enemyObjs = GameObject.FindGameObjectsWithTag("enemy");
		enemies = new List<Enemy>();
		for (int i = 0; i < enemyObjs.Length; i++) {
			enemies.Add(enemyObjs[i].GetComponent<Enemy>());
		}
		spawnTimer = 0;
		turn = 1;
		enemyCount = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (turn == 1) {
			//Debug.Log ("PLAYER TURN");
			//turn is set within PlayerCharacter.cs after exiting executeMode and attempting to move
		} else if (turn == 2) {
			//Debug.Log ("ENEMY TURN");
			//player.score/100 = enemies defeated.

			if((spawnTimer > 5 && enemyCount < (player.score/100) && enemyCount < maxEnemies) || enemyCount < 0)
			{

				 //This should instead be called in Enemy, but I'll move it there later.
				enemyInstance = Instantiate (enemyReference, new Vector2(2, 0), new Quaternion(0,0,0,0)) as GameObject;
				enemyInstance.GetComponent<Entity>().x = 4;
				enemyInstance.GetComponent<Entity>().y = 0;

				enemies.Add(enemyInstance.GetComponent<Enemy>());

				spawnTimer = 0;
			}

			foreach(Enemy enemy in enemies)
			{
				if(enemy != null)
				enemy.isExecuting = true;
			}

			turn = 1;
			spawnTimer++;
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
