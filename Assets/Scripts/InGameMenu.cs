using UnityEngine;
using System.Collections;

public class InGameMenu : MonoBehaviour {

	public bool menuActive;
	public TurnManager tm;
	public Texture2D pauseButton;
	public GameObject puzzleManager;
	public Font chewy;

	public Texture backdrop;

	private bool paused;
	private bool showInstructions;
	private bool adjustVolume;

	private string ins = "<color=#ffffff>" +
						 "How to Play:\n" +
						 "- Move a bottle cap as far as you want\n" +
						 "- Make matches of 3 or more\n" +
						 "- Matches of 4 or more can start combos\n" +
						 "- Moving into an enemy will damage them\n" +
						 "- You have a limited time to move each bottlecap\n" +
						 "- Drunkenly brawl for as long as possible!" +
						 "</color>";
	private GUIStyle insFormatting;
	private GUIStyle gs;
	private GUIStyle gs2;

	// Use this for initialization
	void Start () {
		this.menuActive = false;
//		pauseButton.Resize (Screen.width/9, Screen.width/9);
		paused = false;
		showInstructions = false;
		adjustVolume = false;
		tm = GameObject.Find ("Player").GetComponent<TurnManager> ();

		insFormatting = new GUIStyle();
		insFormatting.alignment = TextAnchor.UpperLeft;
		insFormatting.clipping = TextClipping.Clip;
		insFormatting.wordWrap = true;
		insFormatting.font = chewy;
		insFormatting.fontSize = Screen.width/20;
		insFormatting.richText = true;
		
		gs2 = new GUIStyle();
		gs2.stretchWidth = true;
		gs2.stretchHeight = true;
		gs2.fixedHeight = 5 * Screen.width / 6;
		gs2.fixedWidth = Screen.width;

		gs = new GUIStyle();
		gs.font = chewy;
		gs.fontSize = Screen.width/10;
		gs.alignment = TextAnchor.MiddleCenter;
		gs.stretchWidth = true;
		gs.stretchHeight = true;
		gs.richText = true;
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnGUI(){
		//if no menu, no need to run this method
		if(!menuActive) return;
		
		GUI.Box (new Rect (0, Screen.height - (5 * Screen.width / 6), Screen.width, 5 * Screen.height / 6), backdrop, gs2);

		//pause button in the upper right
		if(GUI.Button(new Rect(Screen.width - 5*Screen.width/25, Screen.height/45, 4*Screen.width/25, 4*Screen.height/45/*Screen.width-pauseButton.width, 0, pauseButton.width, pauseButton.height*/), pauseButton, new GUIStyle() )){
			paused = !paused;
			if(paused){
				puzzleManager.GetComponent<PuzzleManager>().puzzleActive = false;
			}else{
				puzzleManager.GetComponent<PuzzleManager>().puzzleActive = true;
			}
		}
		//primary pause menu
		if(paused && !showInstructions && !adjustVolume){
			//return to game
			if(GUI.Button (new Rect(Screen.width/2-Screen.width/4, 7*Screen.height/14, Screen.width/2, Screen.height/7), "<color=#ffffff>Return to Game</color>", gs)){
				paused = false;
				puzzleManager.GetComponent<PuzzleManager>().puzzleActive = true;
			}
			//how to play
			if(GUI.Button (new Rect(Screen.width/2 - Screen.width/4, 8.75f*Screen.height/14, Screen.width/2, Screen.height/7), "<color=#ffffff>How to play</color>", gs)){
				showInstructions = true;
			}
			//adjust sound volume
			if(GUI.Button (new Rect(Screen.width/2 - Screen.width/4, 10.5f*Screen.height/14, Screen.width/2, Screen.height/7), "<color=#ffffff>Adjust Volume</color>", gs)){
				adjustVolume = true;
			}
			//return to main menu
			if(GUI.Button (new Rect(Screen.width/2-Screen.width/4, 12.25f*Screen.height/14, Screen.width/2, Screen.height/7), "<color=#ffffff>Main Menu</color>", gs)){
				Application.LoadLevel("Main_Menu");
			}
		}
		//show instructions
		if(paused && showInstructions && !adjustVolume){
			//instructions text
			GUI.Box (new Rect(5, Screen.height/2, Screen.width - 5, Screen.height/2), ins, insFormatting);
			//return to root pause menu
			if(GUI.Button (new Rect(Screen.width/6, 5 * Screen.height/6, 2 * Screen.width / 3, Screen.height/6), "<color=#ffffff>Return</color>", gs)){
				showInstructions = false;
			}
		}
		//adjust volume settings
		if(paused && !showInstructions && adjustVolume){
			//
			//
			//draw the sliders here
			//
			//
			/*
			if( GUI button that lets them indicate that they are done ){
				adjustVolume = false;
			}
			*/
		}
	}	
}
