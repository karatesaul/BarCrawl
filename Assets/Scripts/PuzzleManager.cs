using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// The script to manage the puzzle, drawing the puzzle, and running the puzzle
/// </summary>
public class PuzzleManager : MonoBehaviour {
	
	//this is off by default so the menu can appear.
	//when start is pressed on the menu, it turns the puzzle on
	public bool puzzleActive;
	
	public Texture tokenUp;
	public Texture tokenDown;
	public Texture tokenLeft;
	public Texture tokenRight;
	public Texture tokenAttack;
	public Texture tokenEmpty;
	
	public PlayerCharacter pc;
	
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
	public int fallSpeed = 4;
	
	private Token[,] puzzleGrid;
	private int[] refillCount;
	private bool readyToShift;

	public List<TokenType> setOfMoves;
	private List<List<Token>> setOfTokens;
	
	// Use this for initialization
	void Start () {
		puzzleActive = false;
		
		currTime = 0;		
		refillCount = new int[6];
		refillStep = 4;
		
		//List of moves to pass to the Game Board
		setOfMoves = new List<TokenType> ();
		setOfTokens = new List<List<Token>> ();
		//Debug.Log (puzzleGrid [0, 0].tokenVal);
		
		//initialize the tokens
		puzzleGrid = new Token[6, 10];
		for (int i=0; i<6; i++) {
			for (int j=0; j<5; j++){
				
				//get a random token type here
				int type = Random.Range(1, 6);
				puzzleGrid[i,j] = new Token(i, j, type);
			}
			//fill the unseen rows with empty tokens
			for (int j=5; j<10; j++){
				puzzleGrid[i,j] = new Token(i,j, 0);
			}
		}
		
		//clear out the initial matches
		while (QueueMove()) {
			ClearInitialMatches();
			RefillTokens();
			while (ShiftDownAtOnce());
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		//no need to update while still on the menu
		if(!puzzleActive) return;
		
		if (Input.GetKey (KeyCode.I)) {
			Debug.Log(refillStep.ToString());
		}
		
		switch (refillStep) {
		case 0:
			//if the matching algorithm returns matches, go to the next steps.  Otherwise, await anomther move.
			bool matchFound = QueueMove();
			if (matchFound){
				refillStep = 1;
			} else {
				//Debug.Log ("sent " + setOfMoves.Count+ " commands");
				refillStep = 4;
				pc.moveInput = setOfMoves.ToArray ();
				setOfMoves.Clear ();
				pc.fillUp = true;
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
			if (!readyToShift){
				if (!ShiftTokensInData()){
					readyToShift = true;
				}
			} else {
				//if we have shifted tokens, try again.  Otherwise, proceed back to the matching algorithm
				if (!ShiftTokensDownVisually()){
					refillStep = 0;
				}
			}
			break;
		case 4:
			//we are waiting for input
			break;
		default:
			break;
		}
	}

	public bool QueueMove (){
		//Debug.Log ("Running Algorithm");
		int slotNum = 0;
		bool foundMove = false;
		for (int i = 0; i < 6; i++){
			if (puzzleGrid[i,2].tokenVal.Equals(puzzleGrid[i,1].tokenVal)){
				if (puzzleGrid[i,1].tokenVal.Equals(puzzleGrid[i,0].tokenVal)){
					puzzleGrid[i,2].used = true;
					puzzleGrid[i,1].used = true;
					puzzleGrid[i,0].used = true;
					foundMove = true;
					setOfMoves.Add(puzzleGrid[i,2].tokenVal);
					List<Token> newMove = new List<Token>();
					newMove.Add(puzzleGrid[i,0]);
					newMove.Add(puzzleGrid[i,1]);
					newMove.Add(puzzleGrid[i,2]);
					setOfTokens.Add(newMove);
					slotNum++;
				}
				if (puzzleGrid[i,2].tokenVal.Equals(puzzleGrid[i,3].tokenVal)){
					puzzleGrid[i,3].used = true;
					puzzleGrid[i,2].used = true;
					puzzleGrid[i,1].used = true;
					setOfMoves.Add(puzzleGrid[i,2].tokenVal);
					foundMove = true;
					List<Token> newMove = new List<Token>();
					newMove.Add(puzzleGrid[i,1]);
					newMove.Add(puzzleGrid[i,2]);
					newMove.Add(puzzleGrid[i,3]);
					setOfTokens.Add(newMove);
					slotNum++;
				}
			}
			if (puzzleGrid[i,2].tokenVal.Equals(puzzleGrid[i,3].tokenVal)){
				if (puzzleGrid[i,3].tokenVal.Equals(puzzleGrid[i,4].tokenVal)){
					puzzleGrid[i,4].used = true;
					puzzleGrid[i,3].used = true;
					puzzleGrid[i,2].used = true;
					setOfMoves.Add(puzzleGrid[i,2].tokenVal);
					foundMove = true;
					List<Token> newMove = new List<Token>();
					newMove.Add(puzzleGrid[i,2]);
					newMove.Add(puzzleGrid[i,3]);
					newMove.Add(puzzleGrid[i,4]);
					setOfTokens.Add(newMove);
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
					setOfMoves.Add(puzzleGrid[2,j].tokenVal);
					foundMove = true;
					List<Token> newMove = new List<Token>();
					newMove.Add(puzzleGrid[0,j]);
					newMove.Add(puzzleGrid[1,j]);
					newMove.Add(puzzleGrid[2,j]);
					setOfTokens.Add(newMove);
					slotNum++;
				}
				if (puzzleGrid[2,j].tokenVal.Equals(puzzleGrid[3,j].tokenVal)) {
					puzzleGrid[3,j].used = true;
					puzzleGrid[2,j].used = true;
					puzzleGrid[1,j].used = true;
					setOfMoves.Add(puzzleGrid[2,j].tokenVal);
					foundMove = true;
					List<Token> newMove = new List<Token>();
					newMove.Add(puzzleGrid[1,j]);
					newMove.Add(puzzleGrid[2,j]);
					newMove.Add(puzzleGrid[3,j]);
					setOfTokens.Add(newMove);
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
					setOfMoves.Add(puzzleGrid[3,j].tokenVal);
					foundMove = true;
					List<Token> newMove = new List<Token>();
					newMove.Add(puzzleGrid[3,j]);
					newMove.Add(puzzleGrid[4,j]);
					newMove.Add(puzzleGrid[5,j]);
					setOfTokens.Add(newMove);
					slotNum++;
				}
				if (puzzleGrid[3,j].tokenVal.Equals(puzzleGrid[2,j].tokenVal)) {
					puzzleGrid[4,j].used = true;
					puzzleGrid[3,j].used = true;
					puzzleGrid[2,j].used = true;
					setOfMoves.Add(puzzleGrid[3,j].tokenVal);
					foundMove = true;
					List<Token> newMove = new List<Token>();
					newMove.Add(puzzleGrid[4,j]);
					newMove.Add(puzzleGrid[3,j]);
					newMove.Add(puzzleGrid[2,j]);
					setOfTokens.Add(newMove);
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
		bool fadeIsDone = false;
		bool setDone = false;
		//fade only one move at a time
		foreach (Token t in setOfTokens[0]) {
			//fade the move
			if (t.tokenVal != TokenType.Empty){
				t.drawAlpha -= 0.05f;
				if (t.drawAlpha <= 0.0f){
					t.drawAlpha = 1.0f;
					t.tokenVal = TokenType.Empty;
					t.ResetSprite();
					setDone = true;
				}
			}
		}
		if (setDone) {
			setOfTokens.RemoveAt(0);
			setOfTokens.TrimExcess();
			if (setOfTokens.Count == 0){
				fadeIsDone = true;
			}
		}
		return fadeIsDone;
	}
	
	private void ClearInitialMatches(){
		foreach (List<Token> l in setOfTokens) {
			foreach (Token t in l){
				t.tokenVal = TokenType.Empty;
			}
		}
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
				puzzleGrid[i,j].tokenVal = (TokenType)type;
				puzzleGrid[i,j].ResetSprite();
			}
		}
	}
	
	private bool ShiftDownAtOnce(){
		bool shifts = false;
		for (int j=1; j<10; j++){
			for (int i=0; i<6; i++){
				if(puzzleGrid[i,j-1] != null && puzzleGrid[i,j] != null && puzzleGrid[i,j-1].tokenVal == TokenType.Empty){
					puzzleGrid[i, j-1].tokenVal = puzzleGrid[i, j].tokenVal;
					puzzleGrid[i, j].tokenVal = TokenType.Empty;
					if (j < 5){
						shifts = true;
						//Debug.Log ("Shifts Happened");
					}
					puzzleGrid[i, j-1].ResetSprite ();
					puzzleGrid[i, j].ResetSprite ();
				}
			}
		}
		
		return shifts;
	}

	private bool ShiftTokensInData(){
		bool shifts = false;
		//shift the tokens in data alone.
		for (int i=0; i<6; i++) {
			for (int j=1; j<10; j++) {
				if (puzzleGrid [i, j - 1].tokenVal == TokenType.Empty) {
					Token temp = puzzleGrid [i, j - 1];
					puzzleGrid [i, j - 1] = puzzleGrid [i, j];
					puzzleGrid [i, j] = temp;
					if (j < 5) {
						shifts = true;
					}
				}
			}
		}
		return shifts;
	}

	/// <summary>
	/// Shifts the tokens down with a visual effect.
	/// </summary>
	/// <returns><c>true</c>, if token were shifted down, <c>false</c> if done and no shifts were made.</returns>
	private bool ShiftTokensDownVisually(){
		bool shifts = false;
		//shift the tokens in data alone.
		for (int i=0; i<6; i++) {
			for (int j=0; j<10; j++) {
				//get the height the tokens should be at
				Vector2 properHeight = Token.GetCoordsOfPosition(i,j);
				if (puzzleGrid[i,j].Origin.y < properHeight.y){
					puzzleGrid[i,j].location.y += fallSpeed;
					shifts = true;
				}
			}
		}
		return shifts;
	}
	
	public void OnGUI(){
		//no need to draw this while menu is active
		if(!puzzleActive) return;
		
		for (int i=0; i<6; i++) {
			for (int j=0; j<5; j++){
				if (puzzleGrid[i,j].tokenVal == TokenType.Empty) continue;
				if (puzzleGrid[i, j] != activeToken){
					GUI.color = new Color(1.0f, 1.0f, 1.0f, puzzleGrid[i,j].drawAlpha);
					GUI.DrawTexture(puzzleGrid[i,j].location, puzzleGrid[i,j].sprite);
					//GUI.Box(puzzleGrid[i,j].location, "i: " + i.ToString() + "j: " + j.ToString());
				}
			}
			if (refillStep == 3){
				if (puzzleGrid[i,6].tokenVal == TokenType.Empty) continue;
				if (puzzleGrid[i, 6] != activeToken){
					GUI.color = new Color(1.0f, 1.0f, 1.0f, puzzleGrid[i,6].drawAlpha);
					GUI.DrawTexture(puzzleGrid[i,6].location, puzzleGrid[i,6].sprite);
					//GUI.Box(puzzleGrid[i,j].location, "i: " + i.ToString() + "j: " + j.ToString());
				}
			}
		}
		if (activeToken != null){
			GUI.color = new Color(1.0f, 1.0f, 1.0f, activeToken.drawAlpha);
			GUI.DrawTexture(activeToken.location, activeToken.sprite);
		}
		
		if (refillStep == 4 && Input.GetMouseButton (0)) { //or if there is a touch present
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
					refillStep = 0;
				}
				currTime--;
			} else if (activeToken == null && Input.mousePosition.y < 5.0 / 6.0 * Screen.width) {
				//get the token that the mouse is over, and pick it up
				int x = Mathf.FloorToInt (Input.mousePosition.x / (Screen.width * 1.0f / 6.0f));
				int y = Mathf.FloorToInt (Input.mousePosition.y / (Screen.width * 1.0f / 6.0f));
				
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
	public float drawAlpha;
	
	public Vector2 Origin {
		get { return new Vector2(location.x, location.y); }
	}
	
	public static Vector2 GetCoordsOfPosition(int i, int j){
		return new Vector2 (Screen.width * (i / 6.0f), Screen.height - Screen.width / 6.0f * (1 + j));
	}
	
	public static Vector2 GetPositionOfCoords(Vector2 coords){
		return new Vector2 (Mathf.FloorToInt (coords.x * 6.0f / Screen.width), Mathf.FloorToInt ((Screen.height - coords.y) * 6.0f / Screen.width - 1));
	}
	
	public Token(int xLoc, int yLoc, int type){
		this.seen = false;
		this.used = false;
		this.tokenVal = (TokenType)type;
		this.drawAlpha = 1.0f;
		
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
	
	public void ResetSprite(){
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
