using UnityEngine;
using System.Collections;

public class InGameMenu : MonoBehaviour {

	public bool menuActive;
	public TurnManager tm;
	public Texture2D pauseButton;
	public GameObject puzzleManager;
	public Font chewy;

	private bool paused;
	private bool showInstructions;

	private string ins = "How to Play:\n" +
						 "- Move a bottle cap as far as you want\n" +
						 "- Make matches of 3 or more\n" +
						 "- Matches of 4 or more can start combos\n" +
						 "- Moving into an enemy will damage them\n" +
						 "- You have a limited time to move each bottlecap\n" +
						 "- Drunkenly brawl for as long as possible!\n";
	private GUIStyle insFormatting;
	private GUIStyle gs;

	// Use this for initialization
	void Start () {
		this.menuActive = false;
//		pauseButton.Resize (Screen.width/9, Screen.width/9);
		paused = false;
		showInstructions = false;
		tm = GameObject.Find ("Player").GetComponent<TurnManager> ();

		insFormatting = new GUIStyle();
		insFormatting.alignment = TextAnchor.UpperLeft;
		insFormatting.clipping = TextClipping.Clip;
		insFormatting.wordWrap = true;
		insFormatting.font = chewy;
		insFormatting.fontSize = Screen.width/20;

		gs = new GUIStyle();
		gs.font = chewy;
		gs.fontSize = Screen.width/10;
		gs.alignment = TextAnchor.MiddleCenter;
		gs.stretchWidth = true;
		gs.stretchHeight = true;
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnGUI(){
		if(!menuActive) return;

		if(GUI.Button(new Rect(Screen.width - 5*Screen.width/25, Screen.height/45, 4*Screen.width/25, 4*Screen.height/45/*Screen.width-pauseButton.width, 0, pauseButton.width, pauseButton.height*/), pauseButton)){
			paused = !paused;
			if(paused){
				puzzleManager.GetComponent<PuzzleManager>().puzzleActive = false;
			}else{
				puzzleManager.GetComponent<PuzzleManager>().puzzleActive = true;
			}
		}
		if(paused && !showInstructions){
			//return to game
			if(GUI.Button (new Rect(Screen.width/2-Screen.width/4, Screen.height/2, Screen.width/2, Screen.height/6), "Return to Game", gs)){
				paused = false;
				puzzleManager.GetComponent<PuzzleManager>().puzzleActive = true;
			}
			//how to play
			if(GUI.Button (new Rect(Screen.width/2 - Screen.width/4, 4*Screen.height/6, Screen.width/2, Screen.height/6), "How to play", gs)){
				showInstructions = true;
			}
			//return to main menu
			if(GUI.Button (new Rect(Screen.width/2-Screen.width/4, 5*Screen.height/6, Screen.width/2, Screen.height/6), "Main Menu", gs)){
				Application.LoadLevel("Main_Menu");
			}
		}
		if(paused && showInstructions){
			GUI.Box (new Rect(5, Screen.height/2, Screen.width - 5, Screen.height/2), ins, insFormatting);
			if(GUI.Button (new Rect(Screen.width/6, 5 * Screen.height/6, 2 * Screen.width / 3, Screen.height/6), "Return", gs)){
				showInstructions = false;
			}
		}
	}	
}
