using UnityEngine;
using System.Collections;

public class OptionsMenu : MonoBehaviour {

	private class MenuItem{
		public bool currentlyTrue;
		public GUIStyle gs;
		public string displayText;

		public virtual void render(){
			//not used.  Override in derived classes
		}
	}

	private class Option : MenuItem{
		public Texture enabled;
		public Texture disabled;
		public string playerPrefsKey;
		public string trueSnarkyComment;
		public string falseSnarkyComment;

		public Option(Texture e, Texture d, bool status, string playerPrefsKey, string yesComment, string noComment, GUIStyle g){
			enabled = e;
			disabled = d;
			currentlyTrue = status;
			this.playerPrefsKey = playerPrefsKey;
			trueSnarkyComment = yesComment;
			falseSnarkyComment = noComment;
			gs = g;
		}
	}

	private class Title : MenuItem{
		public Texture title;

		public Title(Texture t){
			title = t;
		}
	}

	private class ExitButton{
		public Texture text;
		public Texture background;

		public ExitButton(Texture t, Texture b){
			text = t;
			background = b;
		}
	}

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
<<<<<<< HEAD
=======

	//for scrolling
	private int scrollOffset;
>>>>>>> 5ea5432... Scrolling options

	private GUIStyle optionsHeader;
	private GUIStyle otherText;
	private GUIStyle buttonStyle;
	const string style = "<color=#ffffff><b><i>";
	const string endStyle = "</i></b></color>";
	private MenuItem[] optionList;

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
		Texture tutorialTexture  = (tutorial?yesTutorial:noTutorial);
		Texture profanityTexture = (profanity?yesProfanity:noProfanity);
		Texture violenceTexture  = (violenceSoundtrack?moreVioLence:lessViolence);
		Texture numericHealthTexture = (showNumericHealth?healthBarNumbers:noHealthBarNumbers);
		Texture easyModeTexture = (easyMode?easyModeEnabled:easyModeDisabled);

		//draw options
<<<<<<< HEAD
		GUI.Box(new Rect(Screen.width/2 - options.width/2, 0, options.width, options.height), options, buttonStyle);

		//option to allow profanity in game
		GUI.Label (new Rect(Screen.width/2 - profanityTexture.width/2, Screen.height/7,
		                    profanityTexture.width, profanityTexture.height), style + "I'm an adult" + (profanity?" God damn it!":"!") + endStyle, otherText);
		if(GUI.Button (new Rect(Screen.width/2 - profanityTexture.width/2, Screen.height/7 + 25,
		                        profanityTexture.width, profanityTexture.height), profanityTexture, buttonStyle)){
=======
		GUI.Box(new Rect(Screen.width/2 - buttonWidth/2, scrollOffset, options.width, options.height), options, buttonStyle);

		//option to allow profanity in game
		GUI.Label (new Rect(Screen.width/2 - buttonWidth/2, Screen.height/7 + scrollOffset,
		                   buttonWidth, buttonHeight), style + "I'm an adult" + (profanity?" God damn it!":"!") + endStyle, otherText);
		if(GUI.Button (new Rect(Screen.width/2 - buttonWidth/2, Screen.height/7 + 25 + scrollOffset,
		                        buttonWidth, buttonHeight), profanityTexture, buttonStyle)){
>>>>>>> 5ea5432... Scrolling options
			profanity = !profanity;
			//store this result
			PlayerPrefs.SetInt ("Profanity", (profanity?1:0));
		}


		//option to override our music with vio-lence
<<<<<<< HEAD
		GUI.Label (new Rect(Screen.width/2 - violenceTexture.width/2, 2 * Screen.height/7,
		                    violenceTexture.width, violenceTexture.height), style + (violenceSoundtrack?"I want more Vio-Lence!":"Bang that head that doesn't bang") + endStyle, otherText);
		if(GUI.Button (new Rect(Screen.width/2 - violenceTexture.width/2, 2 * Screen.height/7 + 25,
		                        violenceTexture.width, violenceTexture.height), violenceTexture, buttonStyle)){
=======
		GUI.Label (new Rect(Screen.width/2 - buttonWidth/2, 2 * Screen.height/7 + scrollOffset,
		                    buttonWidth, buttonHeight), style + (violenceSoundtrack?"I want more Vio-Lence!":"Bang that head that doesn't bang") + endStyle, otherText);
		if(GUI.Button (new Rect(Screen.width/2 - buttonWidth/2, 2 * Screen.height/7 + 25 + scrollOffset,
		                        buttonWidth, buttonHeight), violenceTexture, buttonStyle)){
>>>>>>> 5ea5432... Scrolling options
			violenceSoundtrack = !violenceSoundtrack;
			//store this result
			PlayerPrefs.SetInt ("ViolenceMusic", (violenceSoundtrack?1:0));
		}


		//show tutorial setting
<<<<<<< HEAD
		GUI.Label (new Rect(Screen.width/2-tutorialTexture.width/2, 3 * Screen.height/7, 
		                    tutorialTexture.width, tutorialTexture.height), style + (tutorial?"Help me play this complicated game!":"I got it") + endStyle, otherText);
		if(GUI.Button (new Rect(Screen.width/2-tutorialTexture.width/2, 3 * Screen.height/7 + 25, 
		                        tutorialTexture.width, tutorialTexture.height), tutorialTexture, buttonStyle)){
=======
		GUI.Label (new Rect(Screen.width/2-buttonWidth/2, 3 * Screen.height/7 + scrollOffset, 
		                    buttonWidth, buttonHeight), style + (tutorial?"Help me play this complicated game!":"I got it") + endStyle, otherText);
		if(GUI.Button (new Rect(Screen.width/2-buttonWidth/2, 3 * Screen.height/7 + 25 + scrollOffset, 
		                        buttonWidth, buttonHeight), tutorialTexture, buttonStyle)){
>>>>>>> 5ea5432... Scrolling options
			tutorial = !tutorial;
			//store this result
			PlayerPrefs.SetInt("ShowTutorial", (tutorial?1:0));
		}


		//show numeric health values setting
<<<<<<< HEAD
		GUI.Label(new Rect(Screen.width/2 - numericHealthTexture.width/2, 4 * Screen.height/7,
		                   numericHealthTexture.width, numericHealthTexture.height), style + (showNumericHealth?"Quantify my drunkenness":"I don't need no stinkin' numbers") + endStyle, otherText);
		if(GUI.Button (new Rect(Screen.width/2 - numericHealthTexture.width/2, 4 * Screen.height/7 + 25,
		                        numericHealthTexture.width, numericHealthTexture.height), numericHealthTexture, buttonStyle)){
=======
		GUI.Label(new Rect(Screen.width/2 - buttonWidth/2, 4 * Screen.height/7 + scrollOffset,
		                   numericHealthTexture.width, numericHealthTexture.height), style + (showNumericHealth?"Quantify my drunkenness":"I don't need no stinkin' numbers") + endStyle, otherText);
		if(GUI.Button (new Rect(Screen.width/2 - buttonWidth/2, 4 * Screen.height/7 + 25 + scrollOffset,
		                        buttonWidth, buttonHeight), numericHealthTexture, buttonStyle)){
>>>>>>> 5ea5432... Scrolling options
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
<<<<<<< HEAD
		GUI.Label (new Rect(Screen.width/2 - tutorialTexture.width/2, 5 * Screen.height/7,
		                    tutorialTexture.width, tutorialTexture.height), style + "Who made this awesome game?" + endStyle, otherText);
		if(GUI.Button (new Rect(Screen.width/2 - credits.width/2, 5 * Screen.height/7 + 25,
		                        credits.width, credits.height), credits, buttonStyle)){
=======
		GUI.Label (new Rect(Screen.width/2 - buttonWidth/2, 7 * Screen.height/7 + scrollOffset,
		                    tutorialTexture.width, tutorialTexture.height), style + "Who made this awesome game?" + endStyle, otherText);
		if(GUI.Button (new Rect(Screen.width/2 - buttonWidth/2, 7 * Screen.height/7 + 25 + scrollOffset,
		                       buttonWidth, buttonHeight), credits, buttonStyle)){
>>>>>>> 5ea5432... Scrolling options
			Application.LoadLevel("Credits");
		}


		//return to main
<<<<<<< HEAD
		GUI.DrawTexture (new Rect(0, 6*Screen.height/7+25, Screen.width, Screen.height/7-25), buttonbg);
		if(GUI.Button (new Rect(Screen.width/2 - mainMenu.width/2, 6*Screen.height/7 + 25,
		                        mainMenu.width, mainMenu.height), mainMenu, buttonStyle)){
=======
		GUI.DrawTexture (new Rect(0, 8*Screen.height/7 + scrollOffset, Screen.width, Screen.height/7-25), buttonbg);
		if(GUI.Button (new Rect(Screen.width/2 - buttonWidth/2, 8*Screen.height/7 + scrollOffset,
		                        buttonWidth, buttonHeight), mainMenu, buttonStyle)){
>>>>>>> 5ea5432... Scrolling options
			Application.LoadLevel ("Main_Menu");
		}
	}	

	void InitializeList(){
		/*
		optionList = new MenuItem[9];
		optionsList[0] = new Title(options);
		optionsList[1] = new Option();
		optionsList[2] = new Option();
		optionsList[3] = new Option();
		optionsList[4] = new Option();
		optionsList[5] = new Option();
		optionsList[6] = new Option();
		optionsList[7] = new Option();
		optionsList[8] = new ExitButton();
		*/
	}
}
