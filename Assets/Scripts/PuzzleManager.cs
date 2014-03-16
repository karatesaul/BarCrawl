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

	/// <summary>
	/// The refill step.
	/// 0 - matching algorithm
	/// 1 - fade tiles out
	/// 2 - refill tiles
	/// 3 - drop tiles down
	/// 4 - waiting
	/// </summary>
	private int refillStep;
	
	private Token[,] puzzleGrid;
	private int[] refillCount;
	
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
		puzzleGrid = new Token[6, 10];
		for (int i=0; i<6; i++) {
			for (int j=0; j<5; j++){
				
				//get a random token type here
				int type = Random.Range(1, 6);
				puzzleGrid[i,j] = new Token(i, j, type);
			}
		}
		
		refillCount = new int[6];
		refillStep = 4;
		
		//matches = new List<Match> ();
		
		//List of moves to pass to the Game Board
		setOfMoves = new List<TokenType> ();
		Debug.Log (puzzleGrid [0, 0].tokenVal);
	}
	
	// Update is called once per frame
	void Update () {
		switch (refillStep) {
		case 0:
			//if the matching algorithm returns matches, go to the next steps.  Otherwise, await anomther move.
			if (queueMove()){
				refillStep = 1;
			} else {
				refillStep = 4;
			}
			break;
		case 1:
			//if we are done fading, go to the next step.  Otherwise, keep fading.
			if (FadeMatches()){
				refillStep = 2;
			}
			break;
		case 2:
			//refill the tokens and then proceed
			RefillTokens();
			refillStep = 3;
			break;
		case 3:
			//while(ShiftTokensDown()){Debug.Log ("loop");};//added by Cody
			for(int i = 0; i < 20; i++){
				bool moved = ShiftTokensDown ();
				if(moved == false){
					break;
				}
			}
			//if we have shifted tokens, try again.  Otherwise, proceed back to the matching algorithm
			if (!ShiftTokensDown()){
				refillStep = 0;
			}
			break;
		case 4:
			break;
		default:
			break;
		}
	}
	
	public void makeMove (){
		//		for (int i=0; i<20; i++){ //Maybe this works, maybe it doesn't, nobody knows!
		//			//Pass along the matches queue, but don't organize it at this time.
		//			
		//		}
		queueMove ();
		
		RefillTokens ();
		
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

	/// <summary>
	/// Fades the matches, one by one.
	/// Returns true when all are faded.
	/// </summary>
	/// <returns><c>true</c>, if matches are done fading, <c>false</c> otherwise.</returns>
	private bool FadeMatches(){
		//fill this in
		return true;
	}

	/// <summary>
	/// Creates new tokens to fall and refill the grid.  
	/// Iterates over all of the rows, counting the gaps and then adding that many tokens above.
	/// </summary>
	private void RefillTokens(){
		//iterate over all of the columns
		for (int i=0; i<6; i++) {
			//first reset the refillcount for the column
			refillCount[i] = 0;
			//next count the empty spots in the column
			for (int j=0; j<5; j++){
				if (puzzleGrid[i,j].tokenVal == TokenType.Empty){
					refillCount[i]++;
				}
			}
			//now add that number of tokens above to refill the lower rows
			for (int j=5; j<5+refillCount[i]; j++){
				//get a random token type here
				int type = Random.Range(1, 6);
				puzzleGrid[i,j] = new Token(i, j, type);
			}
		}
	}

	/// <summary>
	/// Shifts the tokens down.
	/// </summary>
	/// <returns><c>true</c>, if token were shifted down, <c>false</c> if done and no shifts were made.</returns>
	private bool ShiftTokensDown(){
		bool shifts = false;
		//Move tiles down after matches.
		for (int j=1; j<5; j++){
			for (int i=0; i<6; i++){
				if(puzzleGrid[i,j-1].tokenVal == TokenType.Empty){
					puzzleGrid[i, j-1].tokenVal = puzzleGrid[i, j].tokenVal;
					puzzleGrid[i, j].tokenVal = TokenType.Empty;
					puzzleGrid[i, j-1].resetSprite ();
					puzzleGrid[i, j].resetSprite ();
					shifts = true;
				}
			}
		}
		return shifts;
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
				
				//keep the token within the bounds
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
//					makeMove ();
					refillStep = 0;
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
//				makeMove ();
				refillStep = 0;
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

public enum TokenType : int { Empty, Up, Down, Left, Right, Attack };
