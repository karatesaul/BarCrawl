using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// The script to manage the puzzle, drawing the puzzle, and running the puzzle
/// </summary>
public class PuzzleManager : MonoBehaviour {
	public Texture tokenUp;
	public Texture tokenDown;
	public Texture tokenLeft;
	public Texture tokenRight;
	public Texture tokenAttack;

	private Token[,] puzzleGrid;

	/// <summary>
	/// A queue of the matches found to be sent to the player movement.
	/// </summary>
	/// 
	//public List<Match> matches;

	public List<TokenType> setOfMoves;
	
	// Use this for initialization
	void Start () {
		//initialize the tokens
		puzzleGrid = new Token[6, 5];
		for (int i=0; i<6; i++) {
			for (int j=0; j<5; j++){

				//get a random token type here

				puzzleGrid[i,j] = new Token(i, j, TokenType.Up);
			}
		}
		//matches = new List<Match> ();

		//List of moves to pass to the Game Board
		setOfMoves = new List<TokenType> ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void makeMove (Token[,] puzzleGrid){
		for (int i=0; i<20; i++){ //Maybe this works, maybe it doesn't, nobody knows!
			//Pass along the matches queue, but don't organize it at this time.

	}
		//Move tiles down after matches.
		for (int i=0; i<6; i++){
			for (int j=0; j<5; j++){
				if (puzzleGrid[i,j].used == true){
					if (j<4){
						puzzleGrid[i,j].tokenVal = puzzleGrid[i,j+1].tokenVal;
					}
					puzzleGrid[i,5].tokenVal = 0;
				}
			}
		}
		//Pass matches queue to GridMovement here.
	}

	public bool queueMove (){
		int slotNum = 0;
		bool foundMove = false;
		for (int i = 0; i < 6; i++){
			if (puzzleGrid[i,2].tokenVal.Equals(puzzleGrid[i,1])){
				if (puzzleGrid[i,1].tokenVal.Equals(puzzleGrid[i,0])){
					puzzleGrid[i,2].used = true;
					puzzleGrid[i,1].used = true;
					puzzleGrid[i,0].used = true;
					foundMove = true;
					setOfMoves.Add(puzzleGrid[i,2].tokenVal);
					slotNum++;
				}
				if (puzzleGrid[i,2].tokenVal.Equals(puzzleGrid[i,3])){
					puzzleGrid[i,3].used = true;
					puzzleGrid[i,2].used = true;
					puzzleGrid[i,1].used = true;
					setOfMoves.Add(puzzleGrid[i,2].tokenVal);
					foundMove = true;
					slotNum++;
				}
			}
			if (puzzleGrid[i,2].tokenVal.Equals(puzzleGrid[i,3])){
				if (puzzleGrid[i,3].tokenVal.Equals(puzzleGrid[i,4])){
					puzzleGrid[i,4].used = true;
					puzzleGrid[i,3].used = true;
					puzzleGrid[i,2].used = true;
					setOfMoves.Add(puzzleGrid[i,2].tokenVal);
					foundMove = true;
					slotNum++;
				}
			}
		}
		for (int j = 0; j < 5; j++){
			if (puzzleGrid[2,j].tokenVal.Equals(puzzleGrid[1,j])){
				if (puzzleGrid[1,j].Equals(puzzleGrid[0,j])){
					puzzleGrid[2,j].used = true;
					puzzleGrid[1,j].used = true;
					puzzleGrid[0,j].used = true;
					setOfMoves.Add(puzzleGrid[2,j].tokenVal);
					foundMove = true;
					slotNum++;
				}
				if (puzzleGrid[2,j].Equals(puzzleGrid[3,j])) {
					puzzleGrid[3,j].used = true;
					puzzleGrid[2,j].used = true;
					puzzleGrid[1,j].used = true;
					setOfMoves.Add(puzzleGrid[2,j].tokenVal);
					foundMove = true;
					slotNum++;
				}
			}
		}
		for (int j = 0; j < 5; j++){
			if (puzzleGrid[3,j].tokenVal.Equals(puzzleGrid[4,j])){
				if (puzzleGrid[4,j].Equals(puzzleGrid[5,j])){
					puzzleGrid[5,j].used = true;
					puzzleGrid[4,j].used = true;
					puzzleGrid[3,j].used = true;
					setOfMoves.Add(puzzleGrid[3,j].tokenVal);
					foundMove = true;
					slotNum++;
				}
				if (puzzleGrid[3,j].Equals(puzzleGrid[2,j])) {
					puzzleGrid[4,j].used = true;
					puzzleGrid[3,j].used = true;
					puzzleGrid[2,j].used = true;
					setOfMoves.Add(puzzleGrid[3,j].tokenVal);
					foundMove = true;
					slotNum++;
				}
			}
		}
		return foundMove;
	}

	public void OnGUI(){
		for (int i=0; i<6; i++) {
			for (int j=0; j<5; j++){
				GUI.DrawTexture(puzzleGrid[i,j].location, puzzleGrid[i,j].sprite);
			}
		}
	}
}

public class Token{
	public bool seen;
	public bool used;

	public TokenType tokenVal;
	public Rect location;

	public Texture sprite;
	
	public Token(int xLoc, int yLoc, TokenType type){
		this.seen = false;
		this.used = false;
		this.tokenVal = type;

		location = new Rect (Screen.width * (xLoc / 6.0f), Screen.height - (5.0f / 6.0f * Screen.width * (yLoc / 5.0f)), Screen.width * 1.0f / 6.0f, 5.0f / 6.0f * Screen.width * 1.0f / 5.0f);

		switch (type) {
		case TokenType.Attack:
			sprite = GameObject.Find("PuzzleManager").GetComponent<PuzzleManager>().tokenAttack;
			break;
		case TokenType.Down:
			sprite = GameObject.Find("PuzzleManager").GetComponent<PuzzleManager>().tokenDown;
			break;
		case TokenType.Left:
			sprite = GameObject.Find("PuzzleManager").GetComponent<PuzzleManager>().tokenLeft;
			break;
		case TokenType.Right:
			sprite = GameObject.Find("PuzzleManager").GetComponent<PuzzleManager>().tokenRight;
			break;
		case TokenType.Up:
			sprite = GameObject.Find("PuzzleManager").GetComponent<PuzzleManager>().tokenUp;
			break;
		default:
			break;
		}
	}
}

/*
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
*/

public enum TokenType : int { Empty, Up, Down, Left, Right, Attack };

/* Example of format for Match Code!
 * matches[slotNum].firstNum = 1+(6*j);
 * matches[slotNum].secondNum = 2+(6*j);
 * matches[slotNum].thirdNum = 3+(6*j);
 */
