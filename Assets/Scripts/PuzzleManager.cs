using UnityEngine;
using System.Collections;

/// <summary>
/// The script to manage the puzzle, drawing the puzzle, and running the puzzle
/// </summary>
public class PuzzleManager : MonoBehaviour {
	int scalingFactor;
	
	private Token[,] puzzleGrid;

	public Match[] matches;
	
	// Use this for initialization
	void Start () {
		puzzleGrid = new Token[6, 5];
		for (int i=0; i<6; i++) {
			for (int j=0; j<5; j++){
				puzzleGrid[i,j] = new Token();
				puzzleGrid[i,j].xLocation = 0; //Add code here once we figure out scaling and screen dimensions
				puzzleGrid[i,j].yLocation = 0; //Add code here once we figure out scaling and screen dimensions
				puzzleGrid[i,j].tokenVal = 0;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		int slotNum = 0;
		for (int i = 0; i < 6; i++){
			if (puzzleGrid[i,2].tokenVal.Equals(puzzleGrid[i,1])){
				if (puzzleGrid[i,1].tokenVal.Equals(puzzleGrid[i,0])){
					matches[slotNum].firstNum = i;
					matches[slotNum].secondNum = i+6;
					matches[slotNum].thirdNum = i+12;
					slotNum++;
			}
		}
			if (puzzleGrid[i,2].tokenVal.Equals(puzzleGrid[i,3])){
				if (puzzleGrid[i,1].tokenVal.Equals(puzzleGrid[i,4])){
					matches[slotNum].firstNum = i+12;
					matches[slotNum].secondNum = i+18;
					matches[slotNum].thirdNum = i+24;
					slotNum++;
				}
			}
	}
		for (int j = 0; j < 5; j++){
			if (puzzleGrid[0,j].tokenVal.Equals(puzzleGrid[1,j])){
				if (puzzleGrid[1,j].Equals(puzzleGrid[2,j])){
					matches[slotNum].firstNum = 0+(6*j);
					matches[slotNum].secondNum = 1+(6*j);
					matches[slotNum].thirdNum = 2+(6*j);
					slotNum++;
			}
		}
	}
		for (int j = 0; j < 5; j++){
			if (puzzleGrid[3,j].tokenVal.Equals(puzzleGrid[4,j])){
				if (puzzleGrid[4,j].Equals(puzzleGrid[5,j])){
					matches[slotNum].firstNum = 3+(6*j);
					matches[slotNum].secondNum = 4+(6*j);
					matches[slotNum].thirdNum = 5+(6*j);
					slotNum++;
				}
			}
		}
	}
}

public class Token{

	public bool seen;

	public int xLocation;
	public int yLocation;

	public int tokenVal;
	
	public Token(){
		this.seen = false;
	}

}

public class Match{

	public int firstNum;
	public int secondNum;
	public int thirdNum;
}