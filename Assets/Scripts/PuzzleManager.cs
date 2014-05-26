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
	public Texture tokenHeal;
	public Texture tokenEmpty;
	public Texture gray;

	public Texture cursor;

	public int upVal;
	public int downVal;
	public int leftVal;
	public int rightVal;
	public int attackVal;
	public int healVal;
	public int maxVal;
	public int trackerVal;
	
	public PlayerCharacter pc;
	
	private Token activeToken;
	private int activeX, activeY;
	public int timeLimit = 600;
	public int currTime;
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
	private int[] maxValue;
	private int[] minValue;
	private bool readyToShift;

	public List<TokenType> setOfMoves;
	private List<List<Token>> setOfTokens;
	
	private GameObject cLabel2;
	private GameObject cLabel3;
	private GameObject cLabel4;
	private GameObject cLabel5;
	private GameObject ccLabel;

	public GameObject tLabel1;
	public GameObject tLabel2;
	public GameObject tLabel3;

	/// <summary>
	/// The state of the tutorial.
	/// 0 - not in tutorial
	/// 1 - part 1, before picking up
	/// 2 - part 1, picked up correct token, allow only a swap
	/// 3 - part 1, swap happened, ready to drop
	/// 4 - part 2, before picking up
	/// 5 - part 2, picked up correct token, allow only first swap
	/// 6 - part 2, swap happened, allow only second swap
	/// 7 - part 2, second swap happened, ready to drop
	/// 8 - part 3, before picking up
	/// 9 - part 3, picked up correct token, allow only first swap
	/// 10 - part 3, first swap happened, allow only second swap
	/// 11 - part 3, second swap happened, allow only third swap
	/// 12 - part 3, third swap happened, ready to drop
	/// </summary>
	public int tutorialState;
	private bool drawn; //set to true once the cursor is drawn to set off the animation
	private int m = 0;
	private int n = 0;
	private float x = 0;
	private float y = 0;
	private float locx = 0;
	private float locy = 0;
	private int swapCount = 0;

	private AudioSource audioSource;

	#endregion 

	//coroutine for the text popup
	//author: Krishna Velury
	IEnumerator Wait(GameObject label) {
//		Debug.Log("WAITING");
		yield return new WaitForSeconds(3.0f); // waits 3 seconds
		label.SetActive(false);
	}
	
	// Use this for initialization
	void Start () {
		puzzleActive = false;
		
		currTime = 0;		
		refillCount = new int[6];
		refillStep = 5;

		maxValue = new int[6];
		minValue = new int[6];

		upVal = 150;
		downVal = 150;
		leftVal = 150;
		rightVal = 150;
		attackVal = 350;
		healVal = 50;

		//List of moves to pass to the Game Board
		setOfMoves = new List<TokenType> ();
		setOfTokens = new List<List<Token>> ();
		//Debug.Log (puzzleGrid [0, 0].tokenVal);
		
		//initialize the tokens
		puzzleGrid = new Token[6, 10];
		for (int i=0; i<6; i++) {
			for (int j=0; j<5; j++){
				
				//get a random token type here
				int type = getTokenType();
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
		
		//set tutorial labels
		tLabel1.SetActive(false);
		tLabel2.SetActive(false);
		tLabel3.SetActive(false);
		
		//set tutorial bools and preset board
		if (PlayerPrefs.GetInt("ShowTutorial") == 1) {
			Debug.Log("Beginning with a tutorial");
			tutorialState = 1;
			tutorialBoard();
		}
		drawn = false;
		audioSource = GetComponentInChildren<AudioSource> ();
	}

	public void endTurn(){
		refillStep = 5;
		currTime = 900;
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
			swapCount = 0;
			//if the matching algorithm returns matches, go to the next steps.  Otherwise, await anomther move.
			bool matchFound = QueueMove();
			if (matchFound){
				refillStep = 1;
			} else {

				//assign score based on number of moves in queue and display accordingly
				//added by Krishna
				SetScore (setOfMoves.Count);

				//Debug.Log ("sent " + setOfMoves.Count+ " commands");
				refillStep = 5;
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
		bool setDone = true;
		//fade only one move at a time
		foreach (Token t in setOfTokens[0]) {
			//fade the move
			if (t.tokenVal != TokenType.Empty){
				t.drawAlpha -= 0.05f;
				if (t.drawAlpha <= 0.0f){
					t.drawAlpha = 1.0f;
					t.tokenVal = TokenType.Empty;
					t.ResetSprite();
//					setDone = true;
				} else {
					setDone = false;
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
				int type = getTokenType();
				puzzleGrid[i,j].tokenVal = (TokenType)type;
				puzzleGrid[i,j].ResetSprite();
			}
		}
	}

	private int getTokenType(){
		int roll = Random.Range(1,1000);
		int type = 0;
		if(roll < upVal){
			type = 1;
			upVal -= 5;
			downVal += 1;
			leftVal += 1;
			rightVal += 1;
			attackVal += 1;
			healVal += 1;
		}
		else if(roll < (downVal+upVal)){
			type = 2;
			upVal += 1;
			downVal -= 5;
			leftVal += 1;
			rightVal += 1;
			attackVal += 1;
			healVal += 1;
		}
		else if (roll < (leftVal+downVal+upVal)){
			type = 3;
			upVal += 1;
			downVal += 1;
			leftVal -= 5;
			rightVal += 1;
			attackVal += 1;
			healVal += 1;
		}
		else if (roll < (rightVal+leftVal+downVal+upVal)){
			type = 4;
			upVal += 1;
			downVal += 1;
			leftVal += 1;
			rightVal -= 5;
			attackVal += 1;
			healVal += 1;
		}
		else if (roll < (attackVal+rightVal+leftVal+downVal+upVal)){
			type = 5;
			upVal += 1;
			downVal += 1;
			leftVal += 1;
			rightVal += 1;
			attackVal -= 5;
			healVal +=1;
		}
		else if (roll < (healVal+attackVal+rightVal+leftVal+downVal+upVal)){
			type = 6;
			upVal += 1;
			downVal += 1;
			leftVal += 1;
			rightVal += 1;
			attackVal += 1;
			healVal -= 5;
		}
		else {
			type = 7;
		}
		return type;
	}

	private int getInverseTokenType(){
		int roll = Random.Range(1,1000);
		int type = 0;
		if(roll < upVal){
			type = 1;
			upVal += 5;
			downVal -= 1;
			leftVal -= 1;
			rightVal -= 1;
			attackVal -= 1;
			healVal -= 1;
		}
		else if(roll < (downVal+upVal)){
			type = 2;
			upVal -= 1;
			downVal += 5;
			leftVal -= 1;
			rightVal -= 1;
			attackVal -= 1;
			healVal -= 1;
		}
		else if (roll < (leftVal+downVal+upVal)){
			type = 3;
			upVal -= 1;
			downVal -= 1;
			leftVal += 5;
			rightVal -= 1;
			attackVal -= 1;
			healVal -= 1;
		}
		else if (roll < (rightVal+leftVal+downVal+upVal)){
			type = 4;
			upVal -= 1;
			downVal -= 1;
			leftVal -= 1;
			rightVal += 5;
			attackVal -= 1;
			healVal -= 1;
		}
		else if (roll < (attackVal+rightVal+leftVal+downVal+upVal)){
			type = 5;
			upVal -= 1;
			downVal -= 1;
			leftVal -= 1;
			rightVal -= 1;
			attackVal += 5;
			healVal -=1;
		}
		else if (roll < (healVal+attackVal+rightVal+leftVal+downVal+upVal)){
			type = 6;
			upVal -= 1;
			downVal -= 1;
			leftVal -= 1;
			rightVal -= 1;
			attackVal -= 1;
			healVal += 5;
		}
		else {
			type = 7;
		}
		return type;
	}
	private int codyTokenType(){
		upVal = 1;
		downVal = 1;
		leftVal = 1;
		rightVal = 1;
		attackVal = 1;
		healVal = 1;
		maxVal = 0;
		int roll = Random.Range(1,30);
		int type = 0;
		for (int i=0; i<6; i++) {
			for (int j=0; j<5; j++){
				if (puzzleGrid[i,j].tokenVal == TokenType.Up){
					upVal++;
				}
				if (puzzleGrid[i,j].tokenVal == TokenType.Down){
					downVal++;
				}
				if (puzzleGrid[i,j].tokenVal == TokenType.Left){
					leftVal++;
				}
				if (puzzleGrid[i,j].tokenVal == TokenType.Right){
					rightVal++;
				}
				if (puzzleGrid[i,j].tokenVal == TokenType.Attack){
					attackVal++;
				}
				if (puzzleGrid[i,j].tokenVal == TokenType.Heal){
					healVal++;
				}
			}
		}
		for (int i=0; i<6; i++) {
			if(upVal > maxVal){ maxVal = upVal; trackerVal=1;}
			if(downVal > maxVal){ maxVal = downVal; trackerVal=2;}
			if(leftVal > maxVal){ maxVal = leftVal; trackerVal=3;}
			if(rightVal > maxVal){ maxVal = rightVal; trackerVal=4;}
			if(attackVal > maxVal){ maxVal = attackVal; trackerVal=5;}
			if(healVal > maxVal){ maxVal = healVal; trackerVal=6;}
			
			maxValue[i] = maxVal;
			minValue[5-i] = trackerVal;
			
			if(trackerVal==1) upVal=0;
			if(trackerVal==2) downVal=0;
			if(trackerVal==3) leftVal=0;
			if(trackerVal==4) rightVal=0;
			if(trackerVal==5) attackVal=0;
			if(trackerVal==6) healVal=0;
			maxVal=0;
			trackerVal=0;
		}
		if (roll < maxValue [0]) {
			type = minValue [0];
		}
		else if (roll < maxValue [1]) {
			type = minValue[1];
		}
		else if (roll < maxValue [2]) {
			type = minValue[2];
		}
		else if (roll < maxValue [3]) {
			type = minValue[3];
		}
		else if (roll < maxValue [4]) {
			type = minValue[4];
		}
		else if (roll < maxValue [5]) {
			type = minValue[5];
		}
		return type;
	}


	private void tutorialBoard(){
		puzzleGrid[0,0].tokenVal = TokenType.Heal;
		puzzleGrid[0,0].ResetSprite();
		puzzleGrid[1,0].tokenVal = TokenType.Heal;
		puzzleGrid[1,0].ResetSprite();
		puzzleGrid[2,0].tokenVal = TokenType.Left;
		puzzleGrid[2,0].ResetSprite();
		puzzleGrid[3,0].tokenVal = TokenType.Down;
		puzzleGrid[3,0].ResetSprite();
		puzzleGrid[4,0].tokenVal = TokenType.Right;
		puzzleGrid[4,0].ResetSprite();
		puzzleGrid[5,0].tokenVal = TokenType.Right;
		puzzleGrid[5,0].ResetSprite();
		puzzleGrid[0,1].tokenVal = TokenType.Right;
		puzzleGrid[0,1].ResetSprite();
		puzzleGrid[1,1].tokenVal = TokenType.Right;
		puzzleGrid[1,1].ResetSprite();
		puzzleGrid[2,1].tokenVal = TokenType.Down;
		puzzleGrid[2,1].ResetSprite();
		puzzleGrid[3,1].tokenVal = TokenType.Attack;
		puzzleGrid[3,1].ResetSprite();
		puzzleGrid[4,1].tokenVal = TokenType.Down;
		puzzleGrid[4,1].ResetSprite();
		puzzleGrid[5,1].tokenVal = TokenType.Attack;
		puzzleGrid[5,1].ResetSprite();
		puzzleGrid[0,2].tokenVal = TokenType.Heal;
		puzzleGrid[0,2].ResetSprite();
		puzzleGrid[1,2].tokenVal = TokenType.Left;
		puzzleGrid[1,2].ResetSprite();
		puzzleGrid[2,2].tokenVal = TokenType.Right;
		puzzleGrid[2,2].ResetSprite();
		puzzleGrid[3,2].tokenVal = TokenType.Right;
		puzzleGrid[3,2].ResetSprite();
		puzzleGrid[4,2].tokenVal = TokenType.Attack;
		puzzleGrid[4,2].ResetSprite();
		puzzleGrid[5,2].tokenVal = TokenType.Up;
		puzzleGrid[5,2].ResetSprite();
		puzzleGrid[0,3].tokenVal = TokenType.Left;
		puzzleGrid[0,3].ResetSprite();
		puzzleGrid[1,3].tokenVal = TokenType.Attack;
		puzzleGrid[1,3].ResetSprite();
		puzzleGrid[2,3].tokenVal = TokenType.Left;
		puzzleGrid[2,3].ResetSprite();
		puzzleGrid[3,3].tokenVal = TokenType.Up;
		puzzleGrid[3,3].ResetSprite();
		puzzleGrid[4,3].tokenVal = TokenType.Left;
		puzzleGrid[4,3].ResetSprite();
		puzzleGrid[5,3].tokenVal = TokenType.Attack;
		puzzleGrid[5,3].ResetSprite();
		puzzleGrid[0,4].tokenVal = TokenType.Attack;
		puzzleGrid[0,4].ResetSprite();
		puzzleGrid[1,4].tokenVal = TokenType.Up;
		puzzleGrid[1,4].ResetSprite();
		puzzleGrid[2,4].tokenVal = TokenType.Attack;
		puzzleGrid[2,4].ResetSprite();
		puzzleGrid[3,4].tokenVal = TokenType.Heal; //this is hardcoded because if this is an attack, the tutorial breaks
		puzzleGrid[3,4].ResetSprite();
		puzzleGrid[4,4].tokenVal = TokenType.Down;
		puzzleGrid[4,4].ResetSprite();
		puzzleGrid[5,4].tokenVal = TokenType.Right;
		puzzleGrid[5,4].ResetSprite();
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

		if (tutorialState == 0) {
			handleNormalBoardLogic();
			drawNormalGUI();
		} else {
			handleTutorialBoardLogic();
			drawTutorialGUI ();
		}
	}

	private void drawNormalGUI(){
		//draw the board
		for (int i=0; i<6; i++) {
			for (int j=0; j<5; j++) {
				if (puzzleGrid [i, j].tokenVal == TokenType.Empty)
					continue;
				if (puzzleGrid [i, j] != activeToken) {
					//draw the tokens gray if disabled
					if (GameObject.Find ("Player").GetComponent<TurnManager> ().turn != 0 && refillStep == 4) {
						GUI.color = Color.gray;
					} else {
						GUI.color = new Color (1.0f, 1.0f, 1.0f, puzzleGrid [i, j].drawAlpha);
					}
					GUI.DrawTexture (puzzleGrid [i, j].location, puzzleGrid [i, j].sprite);
				}
			}
			//draw the 6th row if refilling
			if (refillStep == 3){
				if (puzzleGrid[i,6].tokenVal == TokenType.Empty) continue;
				if (puzzleGrid[i, 6] != activeToken){
					GUI.color = new Color(1.0f, 1.0f, 1.0f, puzzleGrid[i,6].drawAlpha);
					GUI.DrawTexture(puzzleGrid[i,6].location, puzzleGrid[i,6].sprite);
				}
			}
		}

		//draw the queue of moves
		//center-align the queue
		float centerX = Screen.width/2 - (setOfMoves.Count * Screen.width/16)/2;
		if (centerX < 0) centerX = 0f;
		float centerY = Screen.width * 5/6 - Screen.width/6;
		int index = 0;
		foreach (TokenType t in setOfMoves) {
			GUI.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
			GUI.DrawTexture(new Rect(centerX + Screen.width/16 * index, centerY, Screen.width/16, Screen.width/16), Token.SpriteOf(t));
			index++;
		}

		//draw the active token
		if (activeToken != null) {
			GUI.color = new Color (1.0f, 1.0f, 1.0f, activeToken.drawAlpha);
			GUI.DrawTexture (activeToken.location, activeToken.sprite);
		}

		//enable or disable the timer visibility based on time
		GameObject timer = GameObject.Find("timer");
		if (currTime <= 300) {
			timer.GetComponent<dfLabel> ().enabled = true;
		} else {
			timer.GetComponent<dfLabel> ().enabled = false;
		}
	}

	private void handleNormalBoardLogic(){
		//if in the proper phase and there is a click/touch and the player is alive
		if (refillStep == 5 && Input.GetMouseButton (0) && pc.GetComponent<PlayerCharacter>().health > 0) {
			//and if there is already an active token
			if (activeToken != null) {
				//drag around the currently selected token
				activeToken.location.x = Input.mousePosition.x + mouseTokenRelativeLocation.x;
				activeToken.location.y = Screen.height - (Input.mousePosition.y + mouseTokenRelativeLocation.y);
				
				//swap around the tiles
				int a = Mathf.FloorToInt (Input.mousePosition.x / (Screen.width * 1.0f / 6.0f));
				int b = Mathf.FloorToInt (Input.mousePosition.y / (Screen.width * 1.0f / 6.0f));
				
				//keep the active token on the board
				if (b > 4){
					b = 4;
				}
				
				//if the token has moved
				if (puzzleGrid[a, b] != activeToken){
					//perform a swap
					puzzleGrid[activeX, activeY] = puzzleGrid[a, b];
					puzzleGrid[activeX, activeY].Reposition(activeX, activeY);
					puzzleGrid [a, b] = activeToken;
					activeX = a;
					activeY = b;
					audioSource.Play();
					swapCount++;
				}
				
				//keep the token within the bounds
				if (activeToken.location.x < 0)
					activeToken.location.x = 0;
				if (activeToken.location.x > Screen.width - activeToken.location.width)
					activeToken.location.x = Screen.width - activeToken.location.width;
				if (activeToken.location.y < Screen.height - 5.0f/6.0f*Screen.width) {
					activeToken.location.y = Screen.height - 5.0f/6.0f*Screen.width;
				}
				if (activeToken.location.y > Screen.height - activeToken.location.height)
					activeToken.location.y = Screen.height - activeToken.location.height;
				
				//if time is up, drop the token
				if (currTime <= 0) {
					activeToken.Reposition(activeX, activeY);
					activeToken = null;
					if (swapCount > 0){
						refillStep = 0;
						swapCount = 0;
					}
				}
				//advance the timer
				currTime--;
			
			//else, if we don't already have an active token	
			} else if (activeToken == null && Input.mousePosition.y < 5.0 / 6.0 * Screen.width) {
				//get the token that the mouse is over, and pick it up
				int a = Mathf.FloorToInt (Input.mousePosition.x / (Screen.width * 1.0f / 6.0f));
				int b = Mathf.FloorToInt (Input.mousePosition.y / (Screen.width * 1.0f / 6.0f));
				activeToken = puzzleGrid [a, b];
				activeX = a;
				activeY = b;
				//start the movement timer
				currTime = timeLimit;
				//get the difference between the mouse position and the token's origin
				mouseTokenRelativeLocation = new Vector2 (activeToken.location.x, Screen.height - activeToken.location.y) - new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
			}
		//else, if there is no touches
		} else {
			//if there is an active token, drop it
			if (activeToken != null){
				activeToken.Reposition(activeX, activeY);
				activeToken = null;
				if (swapCount > 0){
					refillStep = 0;
					swapCount = 0;
				}
			}
			//reset the time limit
			if (currTime != timeLimit) currTime = timeLimit;
		}
	}

	private void drawTutorialGUI(){

		//draw the board
		for (int i=0; i<6; i++) {
			for (int j=0; j<5; j++){
				if (puzzleGrid[i,j].tokenVal == TokenType.Empty) continue;
				if (puzzleGrid[i, j] != activeToken){
					if((GameObject.Find ("Player").GetComponent<TurnManager>().turn!=0 && refillStep == 4)|| (puzzleGrid[i,j].highlight == false && refillStep == 5)) {
						//gray out the tokens if disabled
						GUI.color = Color.gray;
					} else {
						GUI.color = new Color(1.0f, 1.0f, 1.0f, puzzleGrid[i,j].drawAlpha);
					}
					GUI.DrawTexture(puzzleGrid[i,j].location, puzzleGrid[i,j].sprite);
					//set cursor position based on which token is highlighted
//					if (puzzleGrid[i,j].highlight) {
//						x = puzzleGrid[i,j].location.x;
//						y = puzzleGrid[i,j].location.y + 25;
//						locx = x;
//						locy = y;
//						m = i;
//						n = j;
//						//set = true;
//					}
					//if (tut1 || tut2 || tut3) GUI.DrawTexture(new Rect(x, y, puzzleGrid[i,j].location.width, puzzleGrid[i,j].location.height), cursor);	
				}
			}
			//draw the 6th row if refilling
			if (refillStep == 3){
				if (puzzleGrid[i,6].tokenVal == TokenType.Empty) continue;
				if (puzzleGrid[i, 6] != activeToken){
					GUI.color = new Color(1.0f, 1.0f, 1.0f, puzzleGrid[i,6].drawAlpha);
					GUI.DrawTexture(puzzleGrid[i,6].location, puzzleGrid[i,6].sprite);
					//GUI.Box(puzzleGrid[i,j].location, "i: " + i.ToString() + "j: " + j.ToString());
				}
			}
		}

		//draw the active token
		if (activeToken != null) {
			GUI.color = new Color (1.0f, 1.0f, 1.0f, activeToken.drawAlpha);
			GUI.DrawTexture (activeToken.location, activeToken.sprite);
			drawn = true;
		}

		//draw the queue of moves
		//center-align the queue
		float centerX = Screen.width/2 - (setOfMoves.Count * Screen.width/16)/2;
		if (centerX < 0) centerX = 0f;
		float centerY = Screen.width * 5/6 - Screen.width/6;
		int index = 0;
		foreach (TokenType t in setOfMoves) {
			GUI.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
			GUI.DrawTexture(new Rect(centerX + Screen.width/16 * index, centerY, Screen.width/16, Screen.width/16), Token.SpriteOf(t));
			index++;
		}

		//draw tutorial cursor after turn is over
		if (refillStep==5 && activeToken == null) {
			GUI.DrawTexture(new Rect(x, y, puzzleGrid[m,n].location.width, puzzleGrid[m,n].location.height), cursor);
			drawn = true;
		}
	}

	private void handleTutorialBoardLogic(){
		//get the token space that the mouse is over
		int a = Mathf.FloorToInt (Input.mousePosition.x / (Screen.width * 1.0f / 6.0f));
		int b = Mathf.FloorToInt (Input.mousePosition.y / (Screen.width * 1.0f / 6.0f));

		switch(tutorialState) {
		case 0:
			break;
		case 1: 
			//part 1
			//activate the text
			tLabel1.SetActive(true);
			//set cursor postions
			if (!drawn) {
				m = 1;
				n = 3;
				x = puzzleGrid[m,n].location.x;
				y = puzzleGrid[m,n].location.y + 25;
				locx = x;
				locy = y;
			} else if (drawn) { 
				//animate the cursor if positions were already set and it was alreay drawn at the intial position
				y -= Time.deltaTime * 75;
				if (locy - y >= 125) y = locy;
			}
			//we have yet to have a token picked up, so wait for a token to be picked up and check if it is the right token
			if (Input.GetMouseButton(0)){
				//if they clicked on token [1, 3], the correct token
				if (a == 1 && b == 3){
					activeToken = puzzleGrid[a,b];
					activeX = a;
					activeY = b;
					mouseTokenRelativeLocation = new Vector2 (activeToken.location.x, Screen.height - activeToken.location.y) - new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
					tutorialState = 2;
				}
			}
			//set the correct highlights
			puzzleGrid[1,3].highlight = true;
			break;
		case 2: 
			//drag around the currently selected token
			activeToken.location.x = Input.mousePosition.x + mouseTokenRelativeLocation.x;
			activeToken.location.y = Screen.height - (Input.mousePosition.y + mouseTokenRelativeLocation.y);
			//we have the correct token picked up, so only allow swaps to the right location or to drop the token
			if (!Input.GetMouseButton(0)){
				//drop the token
				activeToken.Reposition(activeX, activeY);
				activeToken = null;
				tutorialState = 1;
			} else {
				//if the token has moved, and to the right spot
				if (puzzleGrid[a, b] != activeToken && a == 1 && b == 4){
					//perform a swap
					puzzleGrid[activeX, activeY] = puzzleGrid[a, b];
					puzzleGrid[activeX, activeY].Reposition(activeX, activeY);
					puzzleGrid [a, b] = activeToken;
					activeX = a;
					activeY = b;
					audioSource.Play();
					//swapCount++;
					tutorialState = 3;
				}
			}
			//set the correct highlights
			puzzleGrid[1,3].highlight = false;
			puzzleGrid[1,4].highlight = true;
			break;
		case 3: 
			//drag around the currently selected token
			activeToken.location.x = Input.mousePosition.x + mouseTokenRelativeLocation.x;
			activeToken.location.y = Screen.height - (Input.mousePosition.y + mouseTokenRelativeLocation.y);
			//they have done the correct swap, so only let them drop the token
			if (!Input.GetMouseButton(0)){
				activeToken.Reposition(activeX, activeY);
				activeToken = null;
				tutorialState = 4;
				refillStep = 0;
			}
			drawn = false;
			break;
		case 4:
			//part 2
			//activate the proper tutorial text
			tLabel1.SetActive(false);
			tLabel2.SetActive(true);
			//set cursor postions
			if (!drawn) {
				m = 3;
				n = 2;
				x = puzzleGrid[m,n].location.x;
				y = puzzleGrid[m,n].location.y + 25;
				locx = x;
				locy = y;
			} else if (drawn) {
				//animate the cursor if positions were already set and it was alreay drawn at the intial position
				if (y - locy <= 190) y += Time.deltaTime * 100;
				else y = locy;
			}
			//we have yet to have a token picked up, so wait for a token to be picked up and check if it is the right token
			if (Input.GetMouseButton(0)){
				//if they clicked on token [1, 3], the correct token
				if (a == 3 && b == 2){
					activeToken = puzzleGrid[a,b];
					activeX = a;
					activeY = b;
					mouseTokenRelativeLocation = new Vector2 (activeToken.location.x, Screen.height - activeToken.location.y) - new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
					tutorialState = 5;
				}
			}
			//set the correct highlights
			puzzleGrid[3,2].highlight = true;
			break;
		case 5: 
			//drag around the currently selected token
			activeToken.location.x = Input.mousePosition.x + mouseTokenRelativeLocation.x;
			activeToken.location.y = Screen.height - (Input.mousePosition.y + mouseTokenRelativeLocation.y);
			//we have the correct token picked up, so only allow swaps to the right location or to drop the token
			if (!Input.GetMouseButton(0)){
				//drop the token
				activeToken.Reposition(activeX, activeY);
				activeToken = null;
				tutorialState = 4;
			} else {
				//if the token has moved, and to the right spot
				if (puzzleGrid[a, b] != activeToken && a == 3 && b == 1){
					//perform a swap
					puzzleGrid[activeX, activeY] = puzzleGrid[a, b];
					puzzleGrid[activeX, activeY].Reposition(activeX, activeY);
					puzzleGrid [a, b] = activeToken;
					activeX = a;
					activeY = b;
					audioSource.Play();
					//swapCount++;
					tutorialState = 6;
				}
			}
			//set the correct highlights
			puzzleGrid[3, 2].highlight = false;
			puzzleGrid[3, 1].highlight = true;
			puzzleGrid[3, 0].highlight = true;
			break;
		case 6: 
			//drag around the currently selected token
			activeToken.location.x = Input.mousePosition.x + mouseTokenRelativeLocation.x;
			activeToken.location.y = Screen.height - (Input.mousePosition.y + mouseTokenRelativeLocation.y);
			//we have the correct token picked up, so only allow swaps to the right location or to drop the token
			if (!Input.GetMouseButton(0)){
				//reposition the other token we swapped
				puzzleGrid[3, 1] = puzzleGrid[3, 2];
				puzzleGrid[3, 1].Reposition(3, 1);
				//drop the token
				puzzleGrid[3, 2] = activeToken;
				puzzleGrid[3, 2].Reposition(3, 2);
				activeToken = null;
				tutorialState = 4;
			} else {
				//if the token has moved, and to the right spot
				if (puzzleGrid[a, b] != activeToken && a == 3 && b == 0){
					//perform a swap
					puzzleGrid[activeX, activeY] = puzzleGrid[a, b];
					puzzleGrid[activeX, activeY].Reposition(activeX, activeY);
					puzzleGrid [a, b] = activeToken;
					activeX = a;
					activeY = b;
					audioSource.Play();
					//swapCount++;
					tutorialState = 7;
				}
			}
			puzzleGrid[3, 1].highlight = false;
			break;
		case 7:
			//drag around the currently selected token
			activeToken.location.x = Input.mousePosition.x + mouseTokenRelativeLocation.x;
			activeToken.location.y = Screen.height - (Input.mousePosition.y + mouseTokenRelativeLocation.y);
			//they have done the correct swap, so only let them drop the token
			if (!Input.GetMouseButton(0)){
				activeToken.Reposition(activeX, activeY);
				activeToken = null;
				tutorialState = 8;
				refillStep = 0;
			}
			puzzleGrid[3, 0].highlight = false;
			drawn = false;
			break;
		case 8: 
			//part 3
			//activate the proper tutorial text
			tLabel2.SetActive(false);
			tLabel3.SetActive(true);
			//set cursor postions
			if (!drawn) {
				m = 0;
				n = 1;
				x = puzzleGrid[m,n].location.x;
				y = puzzleGrid[m,n].location.y + 25;
				locx = x;
				locy = y;
			} else if (drawn) {
				if (x - locx <= 150) x += Time.deltaTime * 100;
				else y += Time.deltaTime * 100;
				if (y - locy >= 200) {
					x = locx;
					y = locy;
				}
			}
			//we have yet to have a token picked up, so wait for a token to be picked up and check if it is the right token
			if (Input.GetMouseButton(0)){
				//if they clicked on token [1, 3], the correct token
				if (a == 0 && b == 1){
					activeToken = puzzleGrid[a,b];
					activeX = a;
					activeY = b;
					mouseTokenRelativeLocation = new Vector2 (activeToken.location.x, Screen.height - activeToken.location.y) - new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
					tutorialState = 9;
				}
			}
			puzzleGrid[0,1].highlight = true;
			break;
		case 9: 
			//drag around the currently selected token
			activeToken.location.x = Input.mousePosition.x + mouseTokenRelativeLocation.x;
			activeToken.location.y = Screen.height - (Input.mousePosition.y + mouseTokenRelativeLocation.y);
			//we have the correct token picked up, so only allow swaps to the right location or to drop the token
			if (!Input.GetMouseButton(0)){
				//drop the token
				activeToken.Reposition(activeX, activeY);
				activeToken = null;
				tutorialState = 8;
			} else {
				//if the token has moved, and to the right spot
				if (puzzleGrid[a, b] != activeToken && a == 1 && b == 1){
					//perform a swap
					puzzleGrid[activeX, activeY] = puzzleGrid[a, b];
					puzzleGrid[activeX, activeY].Reposition(activeX, activeY);
					puzzleGrid [a, b] = activeToken;
					activeX = a;
					activeY = b;
					audioSource.Play();
					//swapCount++;
					tutorialState = 10;
				}
			}
			puzzleGrid[0, 1].highlight = false;
			puzzleGrid[1, 1].highlight = true;
			puzzleGrid[2, 1].highlight = true;
			puzzleGrid[2, 0].highlight = true;
			break;
		case 10: 
			//drag around the currently selected token
			activeToken.location.x = Input.mousePosition.x + mouseTokenRelativeLocation.x;
			activeToken.location.y = Screen.height - (Input.mousePosition.y + mouseTokenRelativeLocation.y);
			//we have the correct token picked up, so only allow swaps to the right location or to drop the token
			if (!Input.GetMouseButton(0)){
				//reposition the other token we swapped
				puzzleGrid[1, 1] = puzzleGrid[0, 1];
				puzzleGrid[1, 1].Reposition(1, 1);
				//drop the active token
				puzzleGrid[0, 1] = activeToken;
				puzzleGrid[0, 1].Reposition(0, 1);
				activeToken = null;
				tutorialState = 8;
			} else {
				//if the token has moved, and to the right spot
				if (puzzleGrid[a, b] != activeToken && a == 2 && b == 1){
					//perform a swap
					puzzleGrid[activeX, activeY] = puzzleGrid[a, b];
					puzzleGrid[activeX, activeY].Reposition(activeX, activeY);
					puzzleGrid [a, b] = activeToken;
					activeX = a;
					activeY = b;
					audioSource.Play();
					//swapCount++;
					tutorialState = 11;
				}
			}
			puzzleGrid[1, 1].highlight = false;
			break;
		case 11:
			//drag around the currently selected token
			activeToken.location.x = Input.mousePosition.x + mouseTokenRelativeLocation.x;
			activeToken.location.y = Screen.height - (Input.mousePosition.y + mouseTokenRelativeLocation.y);
			//we have the correct token picked up, so only allow swaps to the right location or to drop the token
			if (!Input.GetMouseButton(0)){
				//reposition the other tokens we swapped
				puzzleGrid[2, 1] = puzzleGrid[1, 1];
				puzzleGrid[2, 1].Reposition(2, 1);
				puzzleGrid[1, 1] = puzzleGrid[0, 1];
				puzzleGrid[1, 1].Reposition(1, 1);
				//drop the token
				puzzleGrid[0, 1] = activeToken;
				puzzleGrid[0, 1].Reposition(0, 1);
				activeToken = null;
				tutorialState = 8;
			} else {
				//if the token has moved, and to the right spot
				if (puzzleGrid[a, b] != activeToken && a == 2 && b == 0){
					//perform a swap
					puzzleGrid[activeX, activeY] = puzzleGrid[a, b];
					puzzleGrid[activeX, activeY].Reposition(activeX, activeY);
					puzzleGrid [a, b] = activeToken;
					activeX = a;
					activeY = b;
					audioSource.Play();
					//swapCount++;
					tutorialState = 12;
				}
			}
			puzzleGrid[2, 1].highlight = false;
			break;
		case 12:
			//drag around the currently selected token
			activeToken.location.x = Input.mousePosition.x + mouseTokenRelativeLocation.x;
			activeToken.location.y = Screen.height - (Input.mousePosition.y + mouseTokenRelativeLocation.y);
			//they have done the correct swap, so only let them drop the token
			if (!Input.GetMouseButton(0)){
				activeToken.Reposition(activeX, activeY);
				activeToken = null;
				tutorialState = 0;
				PlayerPrefs.SetInt("ShowTutorial", 0);
				refillStep = 0;
				tLabel3.SetActive(false);
			}
			puzzleGrid[2, 0].highlight = false;
			break;
		}	
	}

	//function to set the score and display combo popups
	//author: Krishna Velury
	private void SetScore(int moves) {
		if (moves == 1)
			Scores.total += 25;
		else if (moves == 2) {
			//Debug.Log("2x combo!");
			Scores.total += 50;
			cLabel2.SetActive(true);
			StartCoroutine(Wait(cLabel2));
		} else if (moves == 3) {
			//Debug.Log("3x combo!");
			Scores.total += 75;
			cLabel3.SetActive(true);
			StartCoroutine(Wait(cLabel3));
		} else if (moves == 4) {
			//Debug.Log("4x combo!");
			Scores.total += 100;
			cLabel4.SetActive(true);
			StartCoroutine(Wait(cLabel4));
		} else if (moves >= 5) {
			//Debug.Log("Crazy combo!");
			Scores.total += 125;
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

	//for tutorial
	public bool highlight = false;
	
	public Vector2 Origin {
		get { return new Vector2(location.x, location.y); }
	}
	
	public static Vector2 GetCoordsOfPosition(int i, int j){
		return new Vector2 (Screen.width * (i / 6.0f), Screen.height - Screen.width / 6.0f * (1 + j));
	}

	/// <summary>
	/// Gets the i, j board position of a location om the screen
	/// </summary>
	/// <returns>The position of coords.</returns>
	/// <param name="coords">Coords.</param>
	public static Vector2 GetPositionOfCoords(Vector2 coords){
		return new Vector2 (Mathf.FloorToInt (coords.x * 6.0f / Screen.width), Mathf.FloorToInt ((Screen.height - coords.y) * 6.0f / Screen.width - 1));
	}

	public static Texture SpriteOf(TokenType t){
		Texture sprite = null;
		switch (t) {
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
		case TokenType.Heal:
			sprite = GameObject.Find ("PuzzleManager").GetComponent<PuzzleManager>().tokenHeal;
			break;
		case TokenType.Empty:
			sprite = GameObject.Find("PuzzleManager").GetComponent<PuzzleManager>().tokenEmpty;
			break;
		default:
			break;
		}
		return sprite;
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
		case TokenType.Heal:
			sprite = GameObject.Find("PuzzleManager").GetComponent<PuzzleManager>().tokenHeal;
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
		case TokenType.Heal:
			sprite = GameObject.Find("PuzzleManager").GetComponent<PuzzleManager>().tokenHeal;
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

public enum TokenType : int { Empty, Up, Down, Left, Right, Attack, Heal };
