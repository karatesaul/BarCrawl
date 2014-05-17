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
	public bool showNumericHealth;

	// Use this for initialization
	void Start () {
		tutorial = PlayerPrefs.GetInt("ShowTutorial") == 1;
		profanity = PlayerPrefs.GetInt ("Profanity") == 1;
		violenceSoundtrack = PlayerPrefs.GetInt ("ViolenceMusic") == 1;
		showNumericHealth = PlayerPrefs.GetInt("HealthBarNumbers") == 1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		Texture tutorialTexture  = (tutorial?yesTutorial:noTutorial);
		Texture profanityTexture = (profanity?yesProfanity:noProfanity);
		Texture violenceTexture  = (violenceSoundtrack?moreVioLence:lessViolence);
		//Texture numericHealthTexture = (showNumericHealth? ... : ...);

		//draw options
		GUI.DrawTexture(new Rect(Screen.width/2 - options.width/2, 0, options.width, options.height), options);


		//option to allow profanity in game
		GUI.Label (new Rect(Screen.width/2 - profanityTexture.width/2, Screen.height/7,
		                    profanityTexture.width, profanityTexture.height), "To swear or not to swear, that is the question");
		if(GUI.Button (new Rect(Screen.width/2 - profanityTexture.width/2, Screen.height/7 + 25,
		                        profanityTexture.width, profanityTexture.height), profanityTexture)){
			profanity = !profanity;
			//store this result
			PlayerPrefs.SetInt ("Profanity", (profanity?1:0));
		}


		//option to override our music with vio-lence
		GUI.Label (new Rect(Screen.width/2 - violenceTexture.width/2, 2 * Screen.height/7,
		                    violenceTexture.width, violenceTexture.height), "I want more Vio-Lence!");
		if(GUI.Button (new Rect(Screen.width/2 - violenceTexture.width/2, 2 * Screen.height/7 + 25,
		                        violenceTexture.width, violenceTexture.height), violenceTexture)){
			violenceSoundtrack = !violenceSoundtrack;
			//store this result
			PlayerPrefs.SetInt ("ViolenceMusic", (violenceSoundtrack?1:0));
		}


		//show tutorial setting
		GUI.Label (new Rect(Screen.width/2-tutorialTexture.width/2, 3 * Screen.height/7, 
		                    tutorialTexture.width, tutorialTexture.height), "Help me play this complicated game!");
		if(GUI.Button (new Rect(Screen.width/2-tutorialTexture.width/2, 3 * Screen.height/7 + 25, 
		                        tutorialTexture.width, tutorialTexture.height), tutorialTexture)){
			tutorial = !tutorial;
			//store this result
			PlayerPrefs.SetInt("ShowTutorial", (tutorial?1:0));
		}


		//show numeric health values setting
		GUI.Label(new Rect(Screen.width/2 - tutorialTexture.width/2, 4 * Screen.height/7,
		                   tutorialTexture.width, tutorialTexture.height), "Quantify my drunkenness");
		if(GUI.Button (new Rect(Screen.width/2 - tutorialTexture.width/2, 4 * Screen.height/7 + 25,
		                        tutorialTexture.width, tutorialTexture.height), "Show health bar numbers: " + showNumericHealth)){
			showNumericHealth = !showNumericHealth;
			PlayerPrefs.SetInt(	"HealthBarNumbers", (showNumericHealth?1:0));
		}


		//show credits
		GUI.Label (new Rect(Screen.width/2 - tutorialTexture.width/2, 5 * Screen.height/7,
		                    tutorialTexture.width, tutorialTexture.height), "Who made this awesome game?");
		if(GUI.Button (new Rect(Screen.width/2 - tutorialTexture.width/2, 5 * Screen.height/7 + 25,
		                        tutorialTexture.width, tutorialTexture.height), "View credits")){
			Application.LoadLevel("Credits");
		}


		//return to main
		if(GUI.Button (new Rect(Screen.width/2 - violenceTexture.width/2, 6*Screen.height/7 + 25,
		                        violenceTexture.width, violenceTexture.height), "Return to Main Menu")){
			Application.LoadLevel ("Main_Menu");
		}
	}	
}
