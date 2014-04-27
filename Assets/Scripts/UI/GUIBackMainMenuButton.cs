using UnityEngine;
using System.Collections;

public class GUIBackMainMenuButton : MonoBehaviour {

	//public AudioClip buttonPressClip;
	
	public Texture2D backButton;
	public Texture2D fbPlaceholder;

	float buttony2 = Screen.height/9f;

	void Update(){
		buttony2 = Screen.height/9f;
	}


	void OnGUI () {
		if (GUI.Button (new Rect (Screen.width/1.4f,Screen.height/1.2f,buttony2,buttony2), backButton)) {
			//audio.clip = buttonPressClip;
			//audio.Play();
			Application.LoadLevel("Main_Menu");
		}

		if (GUI.Button (new Rect (Screen.width/8.9f,Screen.height/1.2f,buttony2,buttony2), fbPlaceholder)) {
			//audio.clip = buttonPressClip;
			//audio.Play();
			Application.OpenURL("http://www.facebook.com/");
		}
	}

}
