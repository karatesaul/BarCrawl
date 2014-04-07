using UnityEngine;
using System.Collections;

public class TurnManager : MonoBehaviour {

	public PlayerCharacter player;
	public int enemyCount;
	public int maxEnemies;
	public int spawnTimer;
	public GameObject[] enemyObjs; 
	public Enemy[] enemies;
	private GameObject enemyInstance;
	public GameObject enemyReference;

	//0 = none
	//1 = player turn
	//2 = enemy turn
	public int turn;

	// Use this for initialization
	void Start () {
		maxEnemies = 4;
		player = GameObject.Find("lamePC").GetComponent<PlayerCharacter>();
		enemyObjs = GameObject.FindGameObjectsWithTag("enemy");
		enemies = new Enemy[maxEnemies];
		for (int i = 0; i < enemyObjs.Length; i++) {
			enemies[i] = enemyObjs[i].GetComponent<Enemy>();
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
			if((spawnTimer > 5 && enemyCount < (player.score/100) && enemyCount < maxEnemies) || enemyCount < 0){
				shortenEnemyList(); //This should instead be called in Enemy, but I'll move it there later.
				enemyInstance = Instantiate (enemyReference, new Vector2(2, 0), new Quaternion(0,0,0,0)) as GameObject;
				enemyInstance.GetComponent<Enemy>().x = 4;
				enemyInstance.GetComponent<Enemy>().y = 0;
				for(int i = 0; i < enemies.Length; i++){
					if(enemies[i] == null){
						enemies[i] = enemyInstance.GetComponent<Enemy>();
						i = enemies.Length;
					}
				}
				spawnTimer = 0;
			}
			for (int i = 0; i < enemies.Length; i++) {
				if(enemies[i]!=null)
				enemies[i].isExecute = true;
			}
			turn = 1;
			spawnTimer++;
		}
	}

	void shortenEnemyList(){
		for(int i = 1; i < maxEnemies; i++){
			if(enemies[i-1] == null){
				enemies[i-1] = enemies[i];
				enemies[i] = null;
			}
		}
	}
}
