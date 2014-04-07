using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurnManager : MonoBehaviour {

	public PlayerCharacter player;
	public int enemyCount;
	public List<IEnemy> enemies;

	//0 = none
	//1 = player turn
	//2 = enemy turn
	public int turn;

	// Use this for initialization
	void Start () {
		GameObject[] enemyObjs; 

		player = GameObject.Find("Player").GetComponent<PlayerCharacter>();

		enemyObjs = GameObject.FindGameObjectsWithTag("enemy");
		enemies = new List<IEnemy>();
		for (int i = 0; i < enemyObjs.Length; i++) {
			enemies.Add((IEnemy)enemyObjs[i].GetComponent(typeof(IEnemy)));
		}

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
			foreach(IEnemy enemy in enemies)
			{
				enemy.isExecuting = true;
			}
			turn = 1;
		}
	}
}
