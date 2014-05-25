using UnityEngine;
using System.Collections;

public class OptionsMenu : MonoBehaviour {

	public Font chewy;

	public Texture options;
	public Texture yesTutorial;
	public Texture noTutorial;
	public Texture yesProfanity;
	public Texture noProfanity;
	public Texture moreVioLence;
	public Texture lessViolence;
	public Texture healthBarNumbers;
	public Texture noHealthBarNumbers;
	public Texture credits;
	public Texture mainMenu;
	public bool tutorial;
	public bool profanity;
	public bool violenceSoundtrack;
	public bool showNumericHealth;

	private GUIStyle optionsHeader;
	private GUIStyle otherText;
	private GUIStyle buttonStyle;
	string style = "<color=#ffffff><b><i>";
	string endStyle = "</i></b></color>";

	// Use this for initialization
	void Start () {
		tutorial = PlayerPrefs.GetInt("ShowTutorial") == 1;
		profanity = PlayerPrefs.GetInt ("Profanity") == 1;
		violenceSoundtrack = PlayerPrefs.GetInt ("ViolenceMusic") == 1;
		showNumericHealth = PlayerPrefs.GetInt("HealthBarNumbers") == 1;

		optionsHeader = new GUIStyle();
		optionsHeader.fontSize = Screen.width/10;
		optionsHeader.font = chewy;


		otherText = new GUIStyle();
		otherText.font = chewy;
		//otherText.fontStyle = FontStyle.BoldAndItalic;
		otherText.richText = true;

		buttonStyle = new GUIStyle();//this one remains empty to hide the button outline around the textures
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		Texture tutorialTexture  = (tutorial?yesTutorial:noTutorial);
		Texture profanityTexture = (profanity?yesProfanity:noProfanity);
		Texture violenceTexture  = (violenceSoundtrack?moreVioLence:lessViolence);
		Texture numericHealthTexture = (showNumericHealth?healthBarNumbers:noHealthBarNumbers);

		//draw options
		GUI.Box(new Rect(Screen.width/2 - options.width/2, 0, options.width, options.height), options, buttonStyle);

		//option to allow profanity in game
		GUI.Label (new Rect(Screen.width/2 - profanityTexture.width/2, Screen.height/7,
		                    profanityTexture.width, profanityTexture.height), style + "I'm an adult" + (profanity?" God damn it!":"!") + endStyle, otherText);
		if(GUI.Button (new Rect(Screen.width/2 - profanityTexture.width/2, Screen.height/7 + 25,
		                        profanityTexture.width, profanityTexture.height), profanityTexture, buttonStyle)){
			profanity = !profanity;
			//store this result
			PlayerPrefs.SetInt ("Profanity", (profanity?1:0));
		}


		//option to override our music with vio-lence
		GUI.Label (new Rect(Screen.width/2 - violenceTexture.width/2, 2 * Screen.height/7,
		                    violenceTexture.width, violenceTexture.height), style + "I want more Vio-Lence!" + endStyle, otherText);
		if(GUI.Button (new Rect(Screen.width/2 - violenceTexture.width/2, 2 * Screen.height/7 + 25,
		                        violenceTexture.width, violenceTexture.height), violenceTexture, buttonStyle)){
			violenceSoundtrack = !violenceSoundtrack;
			//store this result
			PlayerPrefs.SetInt ("ViolenceMusic", (violenceSoundtrack?1:0));
		}


		//show tutorial setting
		GUI.Label (new Rect(Screen.width/2-tutorialTexture.width/2, 3 * Screen.height/7, 
		                    tutorialTexture.width, tutorialTexture.height), style + "Help me play this complicated game!" + endStyle, otherText);
		if(GUI.Button (new Rect(Screen.width/2-tutorialTexture.width/2, 3 * Screen.height/7 + 25, 
		                        tutorialTexture.width, tutorialTexture.height), tutorialTexture, buttonStyle)){
			tutorial = !tutorial;
			//store this result
			PlayerPrefs.SetInt("ShowTutorial", (tutorial?1:0));
		}


		//show numeric health values setting
		GUI.Label(new Rect(Screen.width/2 - numericHealthTexture.width/2, 4 * Screen.height/7,
		                   numericHealthTexture.width, numericHealthTexture.height), style + "Quantify my drunkenness" + endStyle, otherText);
		if(GUI.Button (new Rect(Screen.width/2 - numericHealthTexture.width/2, 4 * Screen.height/7 + 25,
		                        numericHealthTexture.width, numericHealthTexture.height), numericHealthTexture, buttonStyle)){
			showNumericHealth = !showNumericHealth;
			PlayerPrefs.SetInt(	"HealthBarNumbers", (showNumericHealth?1:0));
		}


		//show credits
		GUI.Label (new Rect(Screen.width/2 - tutorialTexture.width/2, 5 * Screen.height/7,
		                    tutorialTexture.width, tutorialTexture.height), style + "Who made this awesome game?" + endStyle, otherText);
		if(GUI.Button (new Rect(Screen.width/2 - credits.width/2, 5 * Screen.height/7 + 25,
		                        credits.width, credits.height), credits, buttonStyle)){
			Application.LoadLevel("Credits");
		}


		//return to main
		if(GUI.Button (new Rect(Screen.width/2 - mainMenu.width/2, 6*Screen.height/7 + 25,
		                        mainMenu.width, mainMenu.height), mainMenu, buttonStyle)){
			Application.LoadLevel ("Main_Menu");
		}
	}	
}
