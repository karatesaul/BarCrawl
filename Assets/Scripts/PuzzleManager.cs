using UnityEngine;
using System.Collections;

/// <summary>
/// The script to manage the puzzle, drawing the puzzle, and running the puzzle
/// </summary>
public class PuzzleManager : MonoBehaviour {
	
	private Token[,] puzzleGrid;
	
	// Use this for initialization
	void Start () {
		puzzleGrid = new Token[5, 6];
		for (int i=0; i<5; i++) {
			for (int j=0; j<6; j++){
				puzzleGrid[i,j] = new Token();
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

public class Token{

	public bool seen;
	
	public Token(){
		this.seen = false;
	}

}