using UnityEngine;
using System.Collections;

public class GUIBackMainMenuButton : MonoBehaviour {

	//public AudioClip buttonPressClip;
	
	public Texture2D backButton;
	
	float buttony2 = Screen.height/10f;

	void Update(){
		buttony2 = Screen.height/10f;
	}


	void OnGUI () {
		if (GUI.Button (new Rect (10,8.6f*buttony2,buttony2,buttony2), backButton)) {
			//audio.clip = buttonPressClip;
			//audio.Play();
			Application.LoadLevel("Main_Menu");
		}
	}

}
