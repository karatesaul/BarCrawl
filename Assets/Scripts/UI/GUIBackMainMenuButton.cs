using UnityEngine;
using System.Collections;

public class GUIBackMainMenuButton : MonoBehaviour {

	//public AudioClip buttonPressClip;
	
	public Texture2D backButton;
	public Texture2D fbPlaceholder;
	public Texture findUsOnFacebook;
	public Font chewy;

	private GUIStyle text;
	private GUIStyle image;

	//float buttony2 = Screen.height/9f;

	void Update(){
		//buttony2 = Screen.height/9f;
		text = new GUIStyle();
		text.font = chewy;
		text.fontSize = Screen.width / 10;
		text.alignment = TextAnchor.MiddleCenter;
		text.richText = true;
		image = new GUIStyle();
	}


	void OnGUI () {
		//if (GUI.Button (new Rect (Screen.width/1.4f,Screen.height/1.2f,buttony2,buttony2), backButton)) {

		if(GUI.Button (new Rect(0, Screen.height-100, Screen.width, 100), "<color=#ffffff>Continue my bar crawl</color>", text)){
			//audio.clip = buttonPressClip;
			//audio.Play();
			Application.LoadLevel("Main_Menu");
		}

		//if (GUI.Button (new Rect (Screen.width/8.9f,Screen.height/1.2f,buttony2,buttony2), fbPlaceholder)) {
		if(GUI.Button (new Rect(Screen.height - 100 - findUsOnFacebook.height, Screen.width/2 - findUsOnFacebook.width/2, 
		                        findUsOnFacebook.width, findUsOnFacebook.height),
		               findUsOnFacebook/*, image*/)){
		//Debug.Log ("w: " + findUsOnFacebook.width + " h: " + findUsOnFacebook.height);
		//if(GUI.Button (new Rect(0, 0, findUsOnFacebook.width, findUsOnFacebook.height), "FB")){
			//audio.clip = buttonPressClip;
			//audio.Play();
			Application.OpenURL("https://www.facebook.com/BarCrawlANightToForget");
		}
	}

}
