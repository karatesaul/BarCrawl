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
	public Texture tokenEmpty;
	
	private Token activeToken;
	private int activeX, activeY;
	public int timeLimit = 900;
	private int currTime;
	private Vector2 mouseTokenRelativeLocation;
	
	private Token[,] puzzleGrid;
	
	/// <summary>
	/// A queue of the matches found to be sent to the player movement.
	/// </summary>
	///
	//public List<Match> matches;
	
	public List<TokenType> setOfMoves;
	
	// Use this for initialization
	void Start () {
		
		currTime = 0;
		
		//initialize the tokens
		puzzleGrid = new Token[6, 5];
		for (int i=0; i<6; i++) {
			for (int j=0; j<5; j++){
				
				//get a random token type here
				int type = Random.Range(1, 6);
				puzzleGrid[i,j] = new Token(i, j, type);
			}
		}
		//matches = new List<Match> ();
		
		//List of moves to pass to the Game Board
		setOfMoves = new List<TokenType> ();
		Debug.Log (puzzleGrid [0, 0].tokenVal);
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	public void makeMove (/*Token[,] puzzleGrid*/){
		for (int i=0; i<20; i++){ //Maybe this works, maybe it doesn't, nobody knows!
			//Pass along the matches queue, but don't organize it at this time.
			
		}
		queueMove ();
		//Move tiles down after matches.
		for (int j=1; j<5; j++){
			for (int i=0; i<6; i++){
				if(puzzleGrid[i,j-1].tokenVal == TokenType.Empty){
					puzzleGrid[i, j-1].tokenVal = puzzleGrid[i, j].tokenVal;
					puzzleGrid[i, j].tokenVal = TokenType.Empty;
					puzzleGrid[i, j-1].resetSprite ();
					puzzleGrid[i, j].resetSprite ();
				}
				/*if (puzzleGrid[i,j].used == true){
					if (j<4){
						puzzleGrid[i,j].tokenVal = puzzleGrid[i,j+1].tokenVal;
						puzzleGrid[i,j+1].used = true;
						puzzleGrid[i,j].resetSprite ();
					}
					else{
						puzzleGrid[i,j].tokenVal = TokenType.Empty;
						puzzleGrid[i,j].resetSprite ();
					}
					//puzzleGrid[i,5].tokenVal = 0;
				}*/
			}
		}
		Debug.Log (setOfMoves.Count);
		for (int i=0; i<setOfMoves.Count; i++) {
			Debug.Log (setOfMoves[i].ToString ());
		}
		setOfMoves.Clear ();
		//Pass matches queue to GridMovement here.
	}
	
	public bool queueMove (){
		int slotNum = 0;
		bool foundMove = false;
		for (int i = 0; i < 6; i++){
			if (puzzleGrid[i,2].tokenVal.Equals(puzzleGrid[i,1].tokenVal)){
				if (puzzleGrid[i,1].tokenVal.Equals(puzzleGrid[i,0].tokenVal)){
					puzzleGrid[i,2].used = true;
					puzzleGrid[i,1].used = true;
					puzzleGrid[i,0].used = true;
					puzzleGrid[i,2].tokenVal = TokenType.Empty;
					puzzleGrid[i,1].tokenVal = TokenType.Empty;
					puzzleGrid[i,0].tokenVal = TokenType.Empty;
					foundMove = true;
					setOfMoves.Add(puzzleGrid[i,2].tokenVal);
					slotNum++;
				}
				if (puzzleGrid[i,2].tokenVal.Equals(puzzleGrid[i,3].tokenVal)){
					puzzleGrid[i,3].used = true;
					puzzleGrid[i,2].used = true;
					puzzleGrid[i,1].used = true;
					puzzleGrid[i,3].tokenVal = TokenType.Empty;
					puzzleGrid[i,2].tokenVal = TokenType.Empty;
					puzzleGrid[i,1].tokenVal = TokenType.Empty;
					setOfMoves.Add(puzzleGrid[i,2].tokenVal);
					foundMove = true;
					slotNum++;
				}
			}
			if (puzzleGrid[i,2].tokenVal.Equals(puzzleGrid[i,3].tokenVal)){
				if (puzzleGrid[i,3].tokenVal.Equals(puzzleGrid[i,4].tokenVal)){
					puzzleGrid[i,4].used = true;
					puzzleGrid[i,3].used = true;
					puzzleGrid[i,2].used = true;
					puzzleGrid[i,4].tokenVal = TokenType.Empty;
					puzzleGrid[i,3].tokenVal = TokenType.Empty;
					puzzleGrid[i,2].tokenVal = TokenType.Empty;
					setOfMoves.Add(puzzleGrid[i,2].tokenVal);
					foundMove = true;
					slotNum++;
				}
			}
		}
		for (int j = 0; j < 5; j++){
			if (puzzleGrid[2,j].tokenVal.Equals(puzzleGrid[1,j].tokenVal)){
				if (puzzleGrid[1,j].tokenVal.Equals(puzzleGrid[0,j].tokenVal)){
					puzzleGrid[2,j].used = true;
					puzzleGrid[1,j].used = true;
					puzzleGrid[0,j].used = true;
					puzzleGrid[2,j].tokenVal = TokenType.Empty;
					puzzleGrid[1,j].tokenVal = TokenType.Empty;
					puzzleGrid[0,j].tokenVal = TokenType.Empty;
					setOfMoves.Add(puzzleGrid[2,j].tokenVal);
					foundMove = true;
					slotNum++;
				}
				if (puzzleGrid[2,j].tokenVal.Equals(puzzleGrid[3,j].tokenVal)) {
					puzzleGrid[3,j].used = true;
					puzzleGrid[2,j].used = true;
					puzzleGrid[1,j].used = true;
					puzzleGrid[3,j].tokenVal = TokenType.Empty;
					puzzleGrid[2,j].tokenVal = TokenType.Empty;
					puzzleGrid[1,j].tokenVal = TokenType.Empty;
					setOfMoves.Add(puzzleGrid[2,j].tokenVal);
					foundMove = true;
					slotNum++;
				}
			}
		}
		for (int j = 0; j < 5; j++){
			if (puzzleGrid[3,j].tokenVal.Equals(puzzleGrid[4,j].tokenVal)){
				if (puzzleGrid[4,j].tokenVal.Equals(puzzleGrid[5,j].tokenVal)){
					puzzleGrid[5,j].used = true;
					puzzleGrid[4,j].used = true;
					puzzleGrid[3,j].used = true;
					puzzleGrid[5,j].tokenVal = TokenType.Empty;
					puzzleGrid[4,j].tokenVal = TokenType.Empty;
					puzzleGrid[3,j].tokenVal = TokenType.Empty;
					setOfMoves.Add(puzzleGrid[3,j].tokenVal);
					foundMove = true;
					slotNum++;
				}
				if (puzzleGrid[3,j].tokenVal.Equals(puzzleGrid[2,j].tokenVal)) {
					puzzleGrid[4,j].used = true;
					puzzleGrid[3,j].used = true;
					puzzleGrid[2,j].used = true;
					puzzleGrid[4,j].tokenVal = TokenType.Empty;
					puzzleGrid[5,j].tokenVal = TokenType.Empty;
					puzzleGrid[2,j].tokenVal = TokenType.Empty;
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
				if (puzzleGrid[i, j] != activeToken){
					GUI.DrawTexture(puzzleGrid[i,j].location, puzzleGrid[i,j].sprite);
					//GUI.Box(puzzleGrid[i,j].location, "i: " + i.ToString() + "j: " + j.ToString());
				}
			}
		}
		if (activeToken != null){
			GUI.DrawTexture(activeToken.location, activeToken.sprite);
		}

		if (Input.GetMouseButton (0)) { //or if there is a touch present
			if (activeToken != null) {
				//drag around the currently selected token
				activeToken.location.x = Input.mousePosition.x + mouseTokenRelativeLocation.x;
				activeToken.location.y = Screen.height - (Input.mousePosition.y + mouseTokenRelativeLocation.y);

				//swap around the tiles
				int x = Mathf.FloorToInt (Input.mousePosition.x / (Screen.width * 1.0f / 6.0f));
				int y = Mathf.FloorToInt (Input.mousePosition.y / (Screen.width * 1.0f / 6.0f));
				if (puzzleGrid[x, y] != activeToken){
					puzzleGrid[activeX, activeY] = puzzleGrid[x, y];
					puzzleGrid[activeX, activeY].Reposition(activeX, activeY);
					puzzleGrid [x, y] = activeToken;
					activeX = x;
					activeY = y;
				}

				//keep the token within the boundsa
				if (activeToken.location.x < 0)
					activeToken.location.x = 0;
				if (activeToken.location.x > Screen.width - activeToken.location.width)
					activeToken.location.x = Screen.width - activeToken.location.width;
				if (activeToken.location.y < Screen.height - 5.0f/6.0f*Screen.width) 
					activeToken.location.y = Screen.height - 5.0f/6.0f*Screen.width;
				if (activeToken.location.y > Screen.height - activeToken.location.height)
					activeToken.location.y = Screen.height - activeToken.location.height;

				if (currTime <= 0) {
					activeToken.Reposition(activeX, activeY);
					activeToken = null;
					makeMove ();
				}
				currTime--;
			} else if (activeToken == null && Input.mousePosition.y < 5.0 / 6.0 * Screen.width) {
				//get the token that the mouse is over, and pick it up
				int x = Mathf.FloorToInt (Input.mousePosition.x / (Screen.width * 1.0f / 6.0f));
				int y = Mathf.FloorToInt (Input.mousePosition.y / (Screen.width * 1.0f / 6.0f));
				//Debug.Log("X: " + x.ToString() + " Y: " + y.ToString());
				
				activeToken = puzzleGrid [x, y];
				activeX = x;
				activeY = y;
				//start the movement timer
				currTime = timeLimit;
				//get the difference between the mouse position and the token's origin
				mouseTokenRelativeLocation = new Vector2 (activeToken.location.x, Screen.height - activeToken.location.y) - new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
			}

		} else {
			if (activeToken != null){
				activeToken.Reposition(activeX, activeY);
				activeToken = null;
				makeMove ();
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
	
	public Token(int xLoc, int yLoc, int type){
		this.seen = false;
		this.used = false;
		this.tokenVal = (TokenType)type;
		
		location = new Rect (Screen.width * (xLoc / 6.0f), Screen.height - Screen.width / 6.0f * (1 + yLoc), Screen.width * 1.0f / 6.0f, Screen.width * 1.0f / 6.0f);
		
		switch ((TokenType)type) {
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
		case TokenType.Empty:
			sprite = GameObject.Find("PuzzleManager").GetComponent<PuzzleManager>().tokenEmpty;
			break;
		default:
			break;
		}
	}
	public void resetSprite(){
		switch (tokenVal) {
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
		case TokenType.Empty:
			sprite = GameObject.Find("PuzzleManager").GetComponent<PuzzleManager>().tokenEmpty;
			break;
		default:
			break;
		}
	}

	public void Reposition(int xLoc, int yLoc){
		location = new Rect (Screen.width * (xLoc / 6.0f), Screen.height - Screen.width / 6.0f * (1 + yLoc), Screen.width * 1.0f / 6.0f, Screen.width * 1.0f / 6.0f);
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
