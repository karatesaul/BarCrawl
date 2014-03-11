using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// The script to manage the puzzle, drawing the puzzle, and running the puzzle
/// </summary>
public class PuzzleManager : MonoBehaviour {
	int scalingFactor;
	
	private Token[,] puzzleGrid;

	/// <summary>
	/// A queue of the matches found to be sent to the player movement.
	/// </summary>
	public List<Match> matches;
	
	// Use this for initialization
	void Start () {
		//initialize the tokens
		puzzleGrid = new Token[6, 5];
		for (int i=0; i<6; i++) {
			for (int j=0; j<5; j++){
				puzzleGrid[i,j] = new Token(i, j, TokenType.Up);
			}
		}
		matches = new List<Match> ();
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
					puzzleGrid[i,2].used = true;
					puzzleGrid[i,1].used = true;
					puzzleGrid[i,0].used = true;
					slotNum++;
			}
				else if (puzzleGrid[i,2].tokenVal.Equals(puzzleGrid[i,3])){
					matches[slotNum].firstNum = i+6;
					matches[slotNum].secondNum = i+12;
					matches[slotNum].thirdNum = i+18;
					puzzleGrid[i,3].used = true;
					puzzleGrid[i,2].used = true;
					puzzleGrid[i,1].used = true;
					slotNum++;
				}
		}
			if (puzzleGrid[i,2].tokenVal.Equals(puzzleGrid[i,3])){
				if (puzzleGrid[i,3].tokenVal.Equals(puzzleGrid[i,4])){
					matches[slotNum].firstNum = i+12;
					matches[slotNum].secondNum = i+18;
					matches[slotNum].thirdNum = i+24;
					puzzleGrid[i,4].used = true;
					puzzleGrid[i,3].used = true;
					puzzleGrid[i,2].used = true;
					slotNum++;
				}
			}
	}
		for (int j = 0; j < 5; j++){
			if (puzzleGrid[2,j].tokenVal.Equals(puzzleGrid[1,j])){
				if (puzzleGrid[1,j].Equals(puzzleGrid[0,j])){
					matches[slotNum].firstNum = 0+(6*j);
					matches[slotNum].secondNum = 1+(6*j);
					matches[slotNum].thirdNum = 2+(6*j);
					puzzleGrid[2,j].used = true;
					puzzleGrid[1,j].used = true;
					puzzleGrid[0,j].used = true;
					slotNum++;
			}
				else if (puzzleGrid[2,j].Equals(puzzleGrid[3,j])) {
					matches[slotNum].firstNum = 1+(6*j);
					matches[slotNum].secondNum = 2+(6*j);
					matches[slotNum].thirdNum = 3+(6*j);
					puzzleGrid[3,j].used = true;
					puzzleGrid[2,j].used = true;
					puzzleGrid[1,j].used = true;
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
					puzzleGrid[5,j].used = true;
					puzzleGrid[4,j].used = true;
					puzzleGrid[3,j].used = true;
					slotNum++;
				}
				else if (puzzleGrid[3,j].Equals(puzzleGrid[2,j])) {
					matches[slotNum].firstNum = 2+(6*j);
					matches[slotNum].secondNum = 3+(6*j);
					matches[slotNum].thirdNum = 4+(6*j);
					puzzleGrid[4,j].used = true;
					puzzleGrid[3,j].used = true;
					puzzleGrid[2,j].used = true;
					slotNum++;
				}
			}
		}
	}

	public void makeMove (Match[] matches, Token[,] puzzleGrid){
		for (int i=0; i<20; i++){ //Maybe this works, maybe it doesn't, nobody knows!
			//Pass along the matches queue, but don't organize it at this time.

	}
		//Move tiles down after matches.
		for (int i=0; i<6; i++){
			for (int j=0; j<4; j++){
				if (puzzleGrid[i,j].used == true){
					puzzleGrid[i,j].tokenVal = puzzleGrid[i,j+1].tokenVal;
					puzzleGrid[i,5].tokenVal = 0;
				}
			}
		}
		//Pass matches queue to GridMovement here.
	}
}

public class Token{
	public static GameObject tokenUp;
	public static GameObject tokenDown;
	public static GameObject tokenLeft;
	public static GameObject tokenRight;
	public static GameObject tokenAttack;

	public bool seen;
	public bool used;

	//remove these - use GameObject
	public int xLocation;
	public int yLocation;

	public TokenType tokenVal;
	public GameObject gameObject;
	
	public Token(int xLoc, int yLoc, TokenType type){
		this.seen = false;
		this.used = false;

		switch (type) {
		case TokenType.Attack:
			gameObject = GameObject.Instantiate(tokenAttack, new Vector3 (Screen.width * xLoc / 6, 5 / 6 * Screen.width * yLoc / 5, 0), Quaternion.identity) as GameObject;
			break;
		case TokenType.Up:
			gameObject = GameObject.Instantiate(tokenUp, new Vector3 (Screen.width * xLoc / 6, 5 / 6 * Screen.width * yLoc / 5, 0), Quaternion.identity) as GameObject;
			break;
		case TokenType.Down:
			gameObject = GameObject.Instantiate(tokenDown, new Vector3 (Screen.width * xLoc / 6, 5 / 6 * Screen.width * yLoc / 5, 0), Quaternion.identity) as GameObject;
			break;
		case TokenType.Left:
			gameObject = GameObject.Instantiate(tokenLeft, new Vector3 (Screen.width * xLoc / 6, 5 / 6 * Screen.width * yLoc / 5, 0), Quaternion.identity) as GameObject;
			break;
		case TokenType.Right:
			gameObject = GameObject.Instantiate(tokenRight, new Vector3 (Screen.width * xLoc / 6, 5 / 6 * Screen.width * yLoc / 5, 0), Quaternion.identity) as GameObject;
			break;
		default:
			break;
		}
	}
}

public class Match{

	public int firstNum;
	public int secondNum;
	public int thirdNum;

	public Match(){
		firstNum = 0;
		secondNum = 0;
		thirdNum = 0;
	}
}

public enum TokenType : int { Empty, Up, Down, Left, Right, Attack };
