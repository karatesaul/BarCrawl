using UnityEngine;
using System.Collections;

public class PlayerCharacter : Entity {

	public Move[] movelist;
	private int currCombo;
	private bool executeMode;
	private bool exit;
	private int timer;
	// Use this for initialization
	protected override void Start () {
		exit = false;
		timer = 30;
		executeMode = false;
		movelist = new Move[10];
		currCombo = 0;
		for(int i = 0; i < 10; i++){
			movelist[i] = Move.None;
		}
	
		base.Start();
	}



	// Update is called once per frame
	void Update () {

		if(!executeMode){
			if (Input.GetKeyDown (KeyCode.LeftArrow)) {
				if(currCombo < 10){
					movelist [currCombo] = Move.Left;
					currCombo++;
					Debug.Log ("Left move queued.");
				}
				else
					Debug.Log ("Queue full!");
				//AttemptMove (Move.Left);
			}
			if (Input.GetKeyDown (KeyCode.RightArrow)){
				if(currCombo < 10){
				movelist [currCombo] = Move.Right;
				currCombo++;
				Debug.Log ("Right move queued.");
				}
				else
					Debug.Log ("Queue full!");
				//AttemptMove (Move.Right);
			}
			if (Input.GetKeyDown (KeyCode.UpArrow)) {
				if(currCombo < 10){
					movelist [currCombo] = Move.Up;
					currCombo++;
					Debug.Log ("Up move queued.");
				}
				else
					Debug.Log ("Queue full!");
				//AttemptMove (Move.Up);
			}
			if (Input.GetKeyDown (KeyCode.DownArrow)) {
				if(currCombo < 10){
					movelist [currCombo] = Move.Down;
					currCombo++;
					Debug.Log ("Down move queued.");
				}
				else
					Debug.Log ("Queue full!");
				//AttemptMove (Move.Down);
			}
			if(Input.GetKeyDown (KeyCode.Space)){
				currCombo = 0;
				executeMode = true;
				Debug.Log ("Execute mode entered.");
			}
		}
		else{
			if(currCombo > 9 || currCombo < 0){
				currCombo = 0;
				exit = true;
			}
			if(exit || movelist[currCombo] == Move.None){
				for(int i = 0; i < 10; i++){
					movelist[i] = Move.None;
				}
				Debug.Log ("Execute mode exited.");
				exit = false;
				currCombo = 0;
				timer = 30;
				executeMode = false;
				return;
			}
			timer++;
			if(timer > 30){
				AttemptMove(movelist[currCombo]);
				currCombo++;
				timer = 0;
			}
		}
	}
}
