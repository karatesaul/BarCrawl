using UnityEngine;
using System.Collections;

public class PlayerCharacter : Entity {



	// Use this for initialization
	void Start () {
	
	}



	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.LeftArrow))
			AttemptMove (Move.Left);
		if (Input.GetKeyDown (KeyCode.RightArrow))
			AttemptMove (Move.Right);
		if (Input.GetKeyDown (KeyCode.UpArrow))
			AttemptMove (Move.Up);
		if (Input.GetKeyDown (KeyCode.DownArrow))
			AttemptMove (Move.Down);
	}
}
