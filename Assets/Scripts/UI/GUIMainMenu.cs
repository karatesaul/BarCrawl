using UnityEngine;
using System.Collections;

public class GUIMainMenu : MonoBehaviour {
	
	//for calculating button locations
	float screenWidth = (Screen.width/2);
	float screenHeight = (Screen.height/2);
	float buttonx2 = Screen.width/5;
	float buttony2 = Screen.height/8;
	float buttonx1 = 1.0f;
	
	//button press sounds
	//public AudioClip buttonPressClip;
	
	public Texture2D play;
	public Texture2D highScores;
	public Texture2D instructions;
	public Texture2D credits;
	public Texture2D quit;
	
	void Update(){
		buttonx2 = Screen.width/5;
		buttony2 = Screen.height/8;
		screenWidth = (Screen.width/2);
		screenHeight = (Screen.height/2);
		buttonx1 = screenWidth / 1.2f;
	}
	//replace the string value with the texture2D
	void OnGUI () {
		if (GUI.Button (new Rect (buttonx1,screenHeight-1.25f*buttony2,buttonx2,buttony2), play)) {
			//audio.clip = buttonPressClip;
			//audio.Play();
			Application.LoadLevel("Combined_Test_Scene");
		}
		if (GUI.Button (new Rect (buttonx1,screenHeight-0.25f*buttony2,buttonx2,buttony2), highScores)) {
			//audio.clip = buttonPressClip;
			//audio.Play();
			Application.LoadLevel("High_Scores");
		}
		if (GUI.Button (new Rect (buttonx1,screenHeight+0.75f*buttony2,buttonx2,buttony2), instructions)) {
			//audio.clip = buttonPressClip;
			//audio.Play();
			Application.LoadLevel("Instructions");
		}
		if (GUI.Button (new Rect (buttonx1,screenHeight+1.75f*buttony2,buttonx2,buttony2), credits)) {
			//audio.clip = buttonPressClip;
			//audio.Play();
			Application.LoadLevel("Credits");
		}
		if (GUI.Button (new Rect (buttonx1,screenHeight+2.75f*buttony2,buttonx2,buttony2), quit)) {
			//audio.clip = buttonPressClip;
			//audio.Play();
			Application.Quit();
		}
	}
	
}
