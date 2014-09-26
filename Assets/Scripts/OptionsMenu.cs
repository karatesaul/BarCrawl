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
	public Texture easyModeEnabled;
	public Texture easyModeDisabled;
	public Texture credits;
	public Texture mainMenu;
	public Texture2D buttonbg;
	public bool tutorial;
	public bool profanity;
	public bool violenceSoundtrack;
	public bool showNumericHealth;
	public bool easyMode;

	//for scrolling
	private int scrollOffset;

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
		easyMode = PlayerPrefs.GetInt ("EasyMode") == 1;

		optionsHeader = new GUIStyle();
		optionsHeader.fontSize = Screen.width/10;
		optionsHeader.font = chewy;


		otherText = new GUIStyle();
		otherText.font = chewy;
		//otherText.fontStyle = FontStyle.BoldAndItalic;
		otherText.richText = true;

		buttonStyle = new GUIStyle();
		buttonStyle.stretchWidth = true;
		buttonStyle.stretchHeight = true;
		buttonStyle.fixedWidth = yesProfanity.width * Screen.width/800;
		buttonStyle.fixedHeight = yesProfanity.height * Screen.height/1200;

		scrollOffset = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown (KeyCode.E)){
			int x = PlayerPrefs.GetInt ("EasyMode") + 1;
			x %= 2;
			PlayerPrefs.SetInt ("EasyMode", x);
			print ("Difficulty set to: " + (PlayerPrefs.GetInt ("EasyMode") == 0?"hard":"easy"));
		}
		if(Input.GetKey (KeyCode.O)){
			scrollOffset++;
		}
		if(Input.GetKey (KeyCode.L)){
			scrollOffset--;
		}
		if(Input.GetKeyDown (KeyCode.P)){
			print ("Scroll offset: " + scrollOffset);
		}
	}

	void OnGUI(){
		int x = Screen.width;
		int y = Screen.height;
		int buttonWidth  = Mathf.RoundToInt(x * 2.0f/3.0f);
		int buttonHeight = Mathf.RoundToInt(y * 1.0f/6.0f);
		Texture tutorialTexture  = (tutorial?yesTutorial:noTutorial);
		Texture profanityTexture = (profanity?yesProfanity:noProfanity);
		Texture violenceTexture  = (violenceSoundtrack?moreVioLence:lessViolence);
		Texture numericHealthTexture = (showNumericHealth?healthBarNumbers:noHealthBarNumbers);
		Texture easyModeTexture = (easyMode?easyModeEnabled:easyModeDisabled);

		//draw options
		GUI.Box(new Rect(Screen.width/2 - buttonWidth/2, scrollOffset, options.width, options.height), options, buttonStyle);

		//option to allow profanity in game
		GUI.Label (new Rect(Screen.width/2 - buttonWidth/2, Screen.height/7 + scrollOffset,
		                   buttonWidth, buttonHeight), style + "I'm an adult" + (profanity?" God damn it!":"!") + endStyle, otherText);
		if(GUI.Button (new Rect(Screen.width/2 - buttonWidth/2, Screen.height/7 + 25 + scrollOffset,
		                        buttonWidth, buttonHeight), profanityTexture, buttonStyle)){
			profanity = !profanity;
			//store this result
			PlayerPrefs.SetInt ("Profanity", (profanity?1:0));
		}


		//option to override our music with vio-lence
		GUI.Label (new Rect(Screen.width/2 - buttonWidth/2, 2 * Screen.height/7 + scrollOffset,
		                    buttonWidth, buttonHeight), style + (violenceSoundtrack?"I want more Vio-Lence!":"Bang that head that doesn't bang") + endStyle, otherText);
		if(GUI.Button (new Rect(Screen.width/2 - buttonWidth/2, 2 * Screen.height/7 + 25 + scrollOffset,
		                        buttonWidth, buttonHeight), violenceTexture, buttonStyle)){
			violenceSoundtrack = !violenceSoundtrack;
			//store this result
			PlayerPrefs.SetInt ("ViolenceMusic", (violenceSoundtrack?1:0));
		}


		//show tutorial setting
		GUI.Label (new Rect(Screen.width/2-buttonWidth/2, 3 * Screen.height/7 + scrollOffset, 
		                    buttonWidth, buttonHeight), style + (tutorial?"Help me play this complicated game!":"I got it") + endStyle, otherText);
		if(GUI.Button (new Rect(Screen.width/2-buttonWidth/2, 3 * Screen.height/7 + 25 + scrollOffset, 
		                        buttonWidth, buttonHeight), tutorialTexture, buttonStyle)){
			tutorial = !tutorial;
			//store this result
			PlayerPrefs.SetInt("ShowTutorial", (tutorial?1:0));
		}


		//show numeric health values setting
		GUI.Label(new Rect(Screen.width/2 - buttonWidth/2, 4 * Screen.height/7 + scrollOffset,
		                   numericHealthTexture.width, numericHealthTexture.height), style + (showNumericHealth?"Quantify my drunkenness":"I don't need no stinkin' numbers") + endStyle, otherText);
		if(GUI.Button (new Rect(Screen.width/2 - buttonWidth/2, 4 * Screen.height/7 + 25 + scrollOffset,
		                        buttonWidth, buttonHeight), numericHealthTexture, buttonStyle)){
			showNumericHealth = !showNumericHealth;
			PlayerPrefs.SetInt(	"HealthBarNumbers", (showNumericHealth?1:0));
		}

		//toggle easy mode option
		GUI.Label (new Rect(Screen.width/2 - buttonWidth/2, 5 * Screen.height/7 + scrollOffset,
		                    tutorialTexture.width, tutorialTexture.height), style + (easyMode?"Go easy, this is my first bar fight!":"Bring it" + (profanity?", bitch!":"!")) + endStyle, otherText);
		if(GUI.Button (new Rect(Screen.width/2 - buttonWidth/2, 5 * Screen.height/7 + 25 + scrollOffset,
		                        buttonWidth, buttonHeight), easyModeTexture, buttonStyle)){
			easyMode = !easyMode;
			PlayerPrefs.SetInt ("EasyMode", (easyMode?1:0));
		}

		//edit volume stuff


		//show credits
		GUI.Label (new Rect(Screen.width/2 - buttonWidth/2, 7 * Screen.height/7 + scrollOffset,
		                    tutorialTexture.width, tutorialTexture.height), style + "Who made this awesome game?" + endStyle, otherText);
		if(GUI.Button (new Rect(Screen.width/2 - buttonWidth/2, 7 * Screen.height/7 + 25 + scrollOffset,
		                       buttonWidth, buttonHeight), credits, buttonStyle)){
			Application.LoadLevel("Credits");
		}


		//return to main
		GUI.DrawTexture (new Rect(0, 8*Screen.height/7 + scrollOffset, Screen.width, Screen.height/7-25), buttonbg);
		if(GUI.Button (new Rect(Screen.width/2 - buttonWidth/2, 8*Screen.height/7 + scrollOffset,
		                        buttonWidth, buttonHeight), mainMenu, buttonStyle)){
			Application.LoadLevel ("Main_Menu");
		}
	}	
}
