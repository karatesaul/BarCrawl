using UnityEngine;
using System.Collections;

public class OptionsMenu : MonoBehaviour {

	public Texture options;
	public Texture yesTutorial;
	public Texture noTutorial;
	public Texture yesProfanity;
	public Texture noProfanity;
	public Texture moreVioLence;
	public Texture lessViolence;
	public bool tutorial;
	public bool profanity;
	public bool violenceSoundtrack;

	// Use this for initialization
	void Start () {
		/*
		options.Resize(Screen.width, Screen.height/5);
		options.Apply ();
		*/
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		Texture tutorialTexture  = (tutorial?yesTutorial:noTutorial);
		Texture profanityTexture = (profanity?yesProfanity:noProfanity);
		Texture violenceTexture  = (violenceSoundtrack?moreVioLence:lessViolence);
		//draw options
		GUI.DrawTexture(new Rect(Screen.width/2 - options.width/2, 0, options.width, options.height), options);
		//GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), 
		//                 options, ScaleMode.StretchToFill, false);
		//option to allow profanity in game
		GUI.Label (new Rect(Screen.width/2 - profanityTexture.width/2, Screen.height/5,
		                    profanityTexture.width, profanityTexture.height), "To swear or not to swear, that is the question");
		if(GUI.Button (new Rect(Screen.width/2 - profanityTexture.width/2, Screen.height/5 + 50,
		                        profanityTexture.width, profanityTexture.height), profanityTexture)){
			profanity = !profanity;
			//store this result
		}
		//option to override our music with vio-lence
		GUI.Label (new Rect(Screen.width/2 - violenceTexture.width/2, 2 * Screen.height/5,
		                    violenceTexture.width, violenceTexture.height), "I want more Vio-Lence!");
		if(GUI.Button (new Rect(Screen.width/2 - violenceTexture.width/2, 2 * Screen.height/5 + 50,
		                        violenceTexture.width, violenceTexture.height), violenceTexture)){
			violenceSoundtrack = !violenceSoundtrack;
			//store this result
		}
		//show tutorial setting
		GUI.Label (new Rect(Screen.width/2-tutorialTexture.width/2, 3 * Screen.height/5, 
		                    tutorialTexture.width, tutorialTexture.height), "Help me play this complicated game!");
		if(GUI.Button (new Rect(Screen.width/2-tutorialTexture.width/2, 3 * Screen.height/5 + 50, 
		                        tutorialTexture.width, tutorialTexture.height), tutorialTexture)){
			tutorial = !tutorial;
			//store this result somewhere for later retrival
		}
		//return to main
		if(GUI.Button (new Rect(Screen.width/2 - violenceTexture.width/2, 4*Screen.height/5 + 50,
		                        violenceTexture.width, violenceTexture.height), "Return to Main Menu")){
			Application.LoadLevel ("Main_Menu");
		}
	}	
}
