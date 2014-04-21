using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// The script to manage the puzzle, drawing the puzzle, and running the puzzle
/// </summary>
public class PuzzleManager : MonoBehaviour {

	#region Globals

	//this is off by default so the menu can appear.
	//when start is pressed on the menu, it turns the puzzle on
	public bool puzzleActive;
	
	public Texture tokenUp;
	public Texture tokenDown;
	public Texture tokenLeft;
	public Texture tokenRight;
	public Texture tokenAttack;
	public Texture tokenEmpty;

	public int upVal;
	public int downVal;
	public int leftVal;
	public int rightVal;
	public int attackVal;
	
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
	public int refillStep;
	public int fallSpeed = 4;
	
	private Token[,] puzzleGrid;
	private int[] refillCount;
	private bool readyToShift;

	public List<TokenType> setOfMoves;
	private List<List<Token>> setOfTokens;
	
	private GameObject cLabel2;
	private GameObject cLabel3;
	private GameObject cLabel4;
	private GameObject cLabel5;
	private GameObject ccLabel;

	#endregion 

	//coroutine for the text popup
	//author: Krishna Velury
	IEnumerator Wait(GameObject label) {
		Debug.Log("WAITING");
		yield return new WaitForSeconds(3.0f); // waits 3 seconds
		label.SetActive(false);
	}
	
	// Use this for initialization
	void Start () {
		puzzleActive = false;
		
		currTime = 0;		
		refillCount = new int[6];
		refillStep = 4;

		upVal = 200;
		downVal = 200;
		leftVal = 200;
		rightVal = 200;
		attackVal = 200;

		//List of moves to pass to the Game Board
		setOfMoves = new List<TokenType> ();
		setOfTokens = new List<List<Token>> ();
		//Debug.Log (puzzleGrid [0, 0].tokenVal);
		
		//initialize the tokens
		puzzleGrid = new Token[6, 10];
		for (int i=0; i<6; i++) {
			for (int j=0; j<5; j++){
				
				//get a random token type here
				int roll = Random.Range(1,1000);
				int type = 0;
				if(roll < upVal){
					type = 1;
					upVal -= 4;
					downVal += 1;
					leftVal += 1;
					rightVal += 1;
					attackVal +=1;
				}
				else if(roll < (downVal+upVal)){
					type = 2;
					upVal += 1;
					downVal -= 4;
					leftVal += 1;
					rightVal += 1;
					attackVal += 1;
				}
				else if (roll < (leftVal+downVal+upVal)){
					type = 3;
					upVal += 1;
					downVal += 1;
					leftVal -= 4;
					rightVal += 1;
					attackVal += 1;
				}
				else if (roll < (rightVal+leftVal+downVal+upVal)){
					type = 4;
					upVal += 1;
					downVal += 1;
					leftVal += 1;
					rightVal -= 4;
					attackVal += 1;
				}
				else if (roll < (attackVal+rightVal+leftVal+downVal+upVal)){
					type = 5;
					upVal += 1;
					downVal += 1;
					leftVal += 1;
					rightVal += 1;
					attackVal -= 4;
				}
				else {
					type = 6;
				}
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
		setOfMoves.Clear ();
		endTurn ();

		//get scoring labels
		cLabel2 = GameObject.Find("combo2");
		cLabel2.SetActive(false);
		cLabel3 = GameObject.Find("combo3");
		cLabel3.SetActive(false);
		cLabel4 = GameObject.Find("combo4");
		cLabel4.SetActive(false);
		cLabel5 = GameObject.Find("combo5");
		cLabel5.SetActive(false);
	}

	public void endTurn(){
		refillStep = 5;
	}

	// Update is called once per frame
	void Update () {
		//no need to update while still on the menu
		if(!puzzleActive) return;
		
		if (Input.GetKey (KeyCode.I)) {
			Debug.Log(refillStep.ToString());
		}

		if (Input.GetKey (KeyCode.R)) {
			ResetPuzzle();
		}

		switch (refillStep) {
		case 0:
			//if the matching algorithm returns matches, go to the next steps.  Otherwise, await anomther move.
			bool matchFound = QueueMove();
			if (matchFound){
				refillStep = 1;
			} else {

				//assign score based on number of moves in queue and display accordingly
				//added by Krishna
				SetScore (setOfMoves.Count);

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
					readyToShift = false;
				}
			}
			break;
		case 4:
			//executing moves
			break;
		case 5:
			//we are waiting for input
			break;
		default:
			break;
		}
	}

	#region QueueMove

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
					setOfMoves.Add(puzzleGrid[i,2].tokenVal);
					foundMove = true;
					List<Token> newMove = new List<Token>();
					newMove.Add(puzzleGrid[i,0]);
					newMove.Add(puzzleGrid[i,1]);
					newMove.Add(puzzleGrid[i,2]);
					AddMatchToFadeQueue(newMove);
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
					AddMatchToFadeQueue(newMove);
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
					AddMatchToFadeQueue(newMove);
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
					AddMatchToFadeQueue(newMove);
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
					AddMatchToFadeQueue(newMove);
					slotNum++;
				}
			}
			if (puzzleGrid[3,j].tokenVal.Equals(puzzleGrid[4,j].tokenVal)){
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
					AddMatchToFadeQueue(newMove);
					slotNum++;
				}
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
					AddMatchToFadeQueue(newMove);
					slotNum++;
				}
			}
		}
		return foundMove;
	}

	/// <summary>
	/// Takes in a match and attempts to merge it with the match on the end of the queue.  
	/// This happens if the matches have overlapping tiles.
	/// </summary>
	/// <param name="match">Match.</param>
	private void AddMatchToFadeQueue(List<Token> newMatch){
		bool matchFound = false;

		//if there are no prior matches, just add the new move to the queue
		if (setOfTokens.Count == 0) {
			setOfTokens.Add (newMatch);
		} else {
			List<Token> duplicates = new List<Token> ();

			foreach(List<Token> l in setOfTokens){
				//first check to see if there are duplicate tokens
				foreach (Token t in l) {
					foreach(Token u in newMatch){
						if (t == u){
							duplicates.Add(t);
						}
					}
				}
				//if there are duplicates, we have found another portion of the same match.
				if (duplicates.Count > 0){
					matchFound = true;

					//Add the non-duplicates to the match.
					while (duplicates.Count > 0){
						Token t = duplicates[0];
						newMatch.Remove(t);
						duplicates.Remove(t);
					}
					setOfTokens[setOfTokens.Count - 1].AddRange(newMatch);
					break;
				}
			}

			//if we didn't find another portion, add as a new match.
			if (!matchFound){
				setOfTokens.Add(newMatch);
			}
//			//first check to see if there are duplicate tokens
//			foreach (Token t in setOfTokens[setOfTokens.Count-1]) {
//				foreach(Token u in newMatch){
//					if (t == u){
//						duplicates.Add(t);
//					}
//				}
//			}
//
//			//if there are no duplicates, add the new move to the queue
//			if (duplicates.Count == 0){
//				setOfTokens.Add(newMatch);
//			} else {
//				//else add the non-duplicates to the last match.
//				while (duplicates.Count > 0){
//					Token t = duplicates[0];
//					newMatch.Remove(t);
//					duplicates.Remove(t);
//				}
//				
//				setOfTokens[setOfTokens.Count - 1].AddRange(newMatch);
//			}
		}
	}

	#endregion

	#region Fade Methods

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
				t.ResetSprite();
			}
		}
		setOfTokens.Clear ();
	}

	#endregion

	#region Refill

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
				int roll = Random.Range(1,1000);
				int type = 0;
				if(roll < upVal){
					type = 1;
					upVal -= 4;
					downVal += 1;
					leftVal += 1;
					rightVal += 1;
					attackVal +=1;
				}
				else if(roll < (downVal+upVal)){
					type = 2;
					upVal += 1;
					downVal -= 4;
					leftVal += 1;
					rightVal += 1;
					attackVal += 1;
				}
				else if (roll < (leftVal+downVal+upVal)){
					type = 3;
					upVal += 1;
					downVal += 1;
					leftVal -= 4;
					rightVal += 1;
					attackVal += 1;
				}
				else if (roll < (rightVal+leftVal+downVal+upVal)){
					type = 4;
					upVal += 1;
					downVal += 1;
					leftVal += 1;
					rightVal -= 4;
					attackVal += 1;
				}
				else if (roll < (attackVal+rightVal+leftVal+downVal+upVal)){
					type = 5;
					upVal += 1;
					downVal += 1;
					leftVal += 1;
					rightVal += 1;
					attackVal -= 4;
				}
				else {
					type = 6;
				}
				puzzleGrid[i,j].tokenVal = (TokenType)type;
				puzzleGrid[i,j].ResetSprite();
			}
		}
	}

	#endregion

	#region ShiftDown Methods

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
				} else if (puzzleGrid[i,j].Origin.y > properHeight.y){
					puzzleGrid[i,j].Reposition(i,j);
				}
			}
		}
		return shifts;
	}

	#endregion

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
		
		if (refillStep == 5 && Input.GetMouseButton (0)) { //or if there is a touch present
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

	//function to set the score and display combo popups
	//author: Krishna Velury
	private void SetScore(int moves) {
		if (moves == 1) pc.score = pc.score + 25;
		else if (moves == 2) {
			//Debug.Log("2x combo!");
			pc.score = pc.score + 50;
			cLabel2.SetActive(true);
			StartCoroutine(Wait(cLabel2));
		} else if (moves == 3) {
			//Debug.Log("3x combo!");
			pc.score = pc.score + 75;
			cLabel3.SetActive(true);
			StartCoroutine(Wait(cLabel3));
		} else if (moves == 4) {
			//Debug.Log("4x combo!");
			pc.score = pc.score + 100;
			cLabel4.SetActive(true);
			StartCoroutine(Wait(cLabel4));
		} else if (moves >= 5) {
			//Debug.Log("Crazy combo!");
			pc.score = pc.score + 125;
			cLabel5.SetActive(true);
			StartCoroutine(Wait(cLabel5));
		}
	}

	public void ResetPuzzle(){
		setOfTokens.Clear ();
		setOfTokens.Add (new List<Token> ());
		foreach (Token t in puzzleGrid) {
			setOfTokens[0].Add(t);
		}
		refillStep = 1;
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
