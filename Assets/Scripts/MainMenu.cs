using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	bool displayMenu;

	// Use this for initialization
	void Start () {
		displayMenu = true;
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnGUI(){
		//no need to display if not active
		if(!displayMenu){
			return;
		}
		//find desired button size
		int x = Screen.width;
		int y = Screen.height;
		int buttonWidth  = Mathf.RoundToInt(x * 2.0f/3.0f);
		int buttonHeight = Mathf.RoundToInt(y * 1.0f/4.0f);
		if(GUI.Button (new Rect(x/2 - buttonWidth/2, y/2, buttonWidth, buttonHeight), "Start game")){
			//turn the menu off
			displayMenu = false;
			//turn the puzzle on
			GameObject[] PM = GameObject.FindGameObjectsWithTag("PM");
			//PM should only have 1 element
			PM[0].GetComponent <PuzzleManager>().puzzleActive = true;
		}
	}
}
