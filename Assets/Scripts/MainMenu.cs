using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	//whether or not to display this instead of puzzle
	public bool displayMenu;
	//images and tutorial settings
	public Texture title;
	public Texture yesTutorial;
	public Texture noTutorial;
	private bool tutorial;
	
	public GameObject pauseMenu;
	
	// Use this for initialization
	void Start () {
		displayMenu = true;
		//this will be changed to retreive the value from storage later.
		//it assumes the first instance of the game to show the tutorial,
		//and will be turned off after that unless the player selects the
		//checkbox on this screen to turn it back on
		tutorial = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnGUI(){
		//no need to display if not active
		if(!displayMenu){
			return;
		}
		//draw title
		GUI.DrawTexture(new Rect(0, -Screen.height/3, Screen.width, Screen.height), title, ScaleMode.ScaleToFit);
		//find desired button size
		int x = Screen.width;
		int y = Screen.height;
		int buttonWidth  = Mathf.RoundToInt(x * 2.0f/3.0f);
		int buttonHeight = Mathf.RoundToInt(y * 1.0f/6.0f);
		//start game button
		if(GUI.Button (new Rect(x/2 - buttonWidth/2, y/2, buttonWidth, buttonHeight), "Start game")){
			//turn the menu off
			displayMenu = false;
			//turn the puzzle on
			GameObject[] PM = GameObject.FindGameObjectsWithTag("PM");
			//PM should only have 1 element
			//PM[0].GetComponent<PuzzleManager>().loadTutorial();
			PM[0].GetComponent <PuzzleManager>().puzzleActive = true;
			//turn on the pause menu as well
			pauseMenu.GetComponent<InGameMenu>().menuActive = true;
		}
		//high score screen button
		if(GUI.Button (new Rect(x/2 - buttonWidth/2, 2*y/3, buttonWidth, buttonHeight), "High Scores")){
			//transition to high score screen
			Application.LoadLevel("High_Scores");
		}
		//show tutorial setting
		Texture t = (tutorial?yesTutorial:noTutorial);
		if(GUI.Button (new Rect(Screen.width/2-t.width/2, Screen.height - t.height - 10, t.width, t.height), t)){
			tutorial = !tutorial;
		}
	}
}
