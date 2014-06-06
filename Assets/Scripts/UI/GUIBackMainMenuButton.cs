using UnityEngine;
using System.Collections;

public class GUIBackMainMenuButton : MonoBehaviour {

	//public AudioClip buttonPressClip;
	
	public Texture2D backButton;
	public Texture2D fbPlaceholder;
	public Texture findUsOnFacebook;
	public Font chewy;

	private GUIStyle textStyle;
	private GUIStyle imageStyle;

	//float buttony2 = Screen.height/9f;

	void Update(){
		//buttony2 = Screen.height/9f;
		textStyle = new GUIStyle();
		textStyle.font = chewy;
		textStyle.fontSize = Screen.width / 10;
		textStyle.alignment = TextAnchor.MiddleCenter;
		textStyle.richText = true;
		imageStyle = new GUIStyle();
	}


	void OnGUI () {
		//if (GUI.Button (new Rect (Screen.width/1.4f,Screen.height/1.2f,buttony2,buttony2), backButton)) {

		if(GUI.Button (new Rect(0, Screen.height-100, Screen.width, 100), "<color=#ffffff>On to the next bar!</color>", textStyle)){
			Application.LoadLevel("Main_Menu");
		}
		textStyle.fontSize = Mathf.RoundToInt (textStyle.fontSize * 0.8f);//shrink the font a smidge so it fits on screen
		GUI.Label (new Rect(0, Screen.height - 70 - findUsOnFacebook.height - 50, Screen.width, 50), 
		           "<color=#ffffff><i>Tell us how you did!</i></color>", textStyle);
		textStyle.fontSize = Screen.width / 10;
		if(GUI.Button (new Rect(Screen.width/2 - findUsOnFacebook.width/2, Screen.height - 70 - findUsOnFacebook.height,
		                        findUsOnFacebook.width, findUsOnFacebook.height),
		               findUsOnFacebook, imageStyle)){
			Application.OpenURL("https://www.facebook.com/BarCrawlANightToForget");
		}
	}

}
