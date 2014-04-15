using UnityEngine;
using System.Collections;

public class InGameMenu : MonoBehaviour {

	public bool menuActive;
	public Texture2D pauseButton;
	public GameObject puzzleManager;

	private bool paused;

	// Use this for initialization
	void Start () {
		this.menuActive = false;
		pauseButton.Resize (Screen.width/9, Screen.width/9);
		paused = false;
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnGUI(){
		if(!menuActive) return;
		if(GUI.Button(new Rect(Screen.width-pauseButton.width, 0, pauseButton.width, pauseButton.height), pauseButton)){
			paused = !paused;
			if(paused){
				puzzleManager.GetComponent<PuzzleManager>().puzzleActive = false;
			}else{
				puzzleManager.GetComponent<PuzzleManager>().puzzleActive = true;
			}
		}
		if(paused){
			if(GUI.Button (new Rect(Screen.width/2-Screen.width/4, Screen.height/2, Screen.width/2, Screen.height/4), "Main Menu")){
				Application.LoadLevel("Main_Menu");
			}
			if(GUI.Button (new Rect(Screen.width/2-Screen.width/4, 3*Screen.height/4, Screen.width/2, Screen.height/4), "Return to Game")){
				paused = false;
				puzzleManager.GetComponent<PuzzleManager>().puzzleActive = true;
			}
		}
	}	
}
