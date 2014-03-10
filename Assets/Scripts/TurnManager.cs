using UnityEngine;
using System.Collections;

public class TurnManager : MonoBehaviour {

	public PlayerCharacter player;
	public int enemyCount;
	public GameObject[] enemyObjs; 
	public Enemy[] enemies; 

	//0 = none
	//1 = player turn
	//2 = enemy turn
	public int turn;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("lamePC").GetComponent<PlayerCharacter>();
		enemyObjs = GameObject.FindGameObjectsWithTag("enemy");
		enemies = new Enemy[enemyObjs.Length];
		for (int i = 0; i < enemyObjs.Length; i++) {
			enemies[i] = enemyObjs[i].GetComponent<Enemy>();
		}

		turn = 1;
		enemyCount = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (turn == 1) {
			Debug.Log ("PLAYER TURN");
			//turn is set within PlayerCharacter.cs after exiting executeMode and attempting to move
		} else if (turn == 2) {
			Debug.Log ("ENEMY TURN");
			for (int i = 0; i < enemies.Length; i++) {
				enemies[i].isExecute = true;
			}
			turn = 1;
		}
	}
}
