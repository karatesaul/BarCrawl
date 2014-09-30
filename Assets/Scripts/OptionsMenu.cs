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
	public Texture sliderButton;
	public Texture2D buttonbg;
	public bool tutorial;
	public bool profanity;
	public bool violenceSoundtrack;
	public bool showNumericHealth;
	public bool easyMode;

	//main pause menu, or adjust volume?
	private bool adjustingVolume;

	//for scrolling
	private int scrollOffset;

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

		adjustingVolume = false;
	}
	
	// Update is called once per frame
	void Update () {
		/*
		if(Input.GetKeyDown (KeyCode.E)){
			int x = PlayerPrefs.GetInt ("EasyMode") + 1;
			x %= 2;
			PlayerPrefs.SetInt ("EasyMode", x);
			print ("Difficulty set to: " + (PlayerPrefs.GetInt ("EasyMode") == 0?"hard":"easy"));
		}
		*/
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
		//intercept call if in adjusting volume state
		//I know it is ugly, but I want to get this out the door and never touch it again :(
		if(adjustingVolume){
			volumeGUI();
			return;
		}
		int x = Screen.width;
		int y = Screen.height;
		int buttonWidth  = Mathf.RoundToInt(x * 2.0f/3.0f);
		int buttonHeight = options.height;
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
			return;
		}


		//option to override our music with vio-lence
		GUI.Label (new Rect(Screen.width/2 - buttonWidth/2, 2 * Screen.height/7 + scrollOffset,
		                    buttonWidth, buttonHeight), style + (violenceSoundtrack?"I want more Vio-Lence!":"Bang that head that doesn't bang") + endStyle, otherText);
		if(GUI.Button (new Rect(Screen.width/2 - buttonWidth/2, 2 * Screen.height/7 + 25 + scrollOffset,
		                        buttonWidth, buttonHeight), violenceTexture, buttonStyle)){
			violenceSoundtrack = !violenceSoundtrack;
			//store this result
			PlayerPrefs.SetInt ("ViolenceMusic", (violenceSoundtrack?1:0));
			return;
		}


		//show tutorial setting
		GUI.Label (new Rect(Screen.width/2-buttonWidth/2, 3 * Screen.height/7 + scrollOffset, 
		                    buttonWidth, buttonHeight), style + (tutorial?"Help me play this complicated game!":"How hard could this be?") + endStyle, otherText);
		if(GUI.Button (new Rect(Screen.width/2-buttonWidth/2, 3 * Screen.height/7 + 25 + scrollOffset, 
		                        buttonWidth, buttonHeight), tutorialTexture, buttonStyle)){
			tutorial = !tutorial;
			//store this result
			PlayerPrefs.SetInt("ShowTutorial", (tutorial?1:0));
			return;
		}


		//show numeric health values setting
		GUI.Label(new Rect(Screen.width/2 - buttonWidth/2, 4 * Screen.height/7 + scrollOffset,
		                   numericHealthTexture.width, numericHealthTexture.height), style + (showNumericHealth?"Quantify my drunkenness":"I don't need no stinkin' numbers") + endStyle, otherText);
		if(GUI.Button (new Rect(Screen.width/2 - buttonWidth/2, 4 * Screen.height/7 + 25 + scrollOffset,
		                        buttonWidth, buttonHeight), numericHealthTexture, buttonStyle)){
			showNumericHealth = !showNumericHealth;
			PlayerPrefs.SetInt(	"HealthBarNumbers", (showNumericHealth?1:0));
			return;
		}

		//toggle easy mode option
		GUI.Label (new Rect(Screen.width/2 - buttonWidth/2, 5 * Screen.height/7 + scrollOffset,
		                    easyModeTexture.width, easyModeTexture.height), style + (easyMode?"Go easy, this is my first bar fight!":"Bring it" + (profanity?", bitch!":"!")) + endStyle, otherText);
		if(GUI.Button (new Rect(Screen.width/2 - buttonWidth/2, 5 * Screen.height/7 + 25 + scrollOffset,
		                        buttonWidth, buttonHeight), easyModeTexture, buttonStyle)){
			easyMode = !easyMode;
			PlayerPrefs.SetInt ("EasyMode", (easyMode?1:0));
			return;
		}

		//edit volume stuff
		GUI.Label (new Rect(Screen.width/2 - buttonWidth/2, 6 * Screen.height/7 + scrollOffset,
		                    easyModeTexture.width, easyModeTexture.height), style + "Edit volume settings" + endStyle, otherText);
		if(GUI.Button (new Rect(Screen.width/2 - buttonWidth/2, 6 * Screen.height/7 + 25 + scrollOffset,
		                        buttonWidth, buttonHeight), options, buttonStyle)){
			adjustingVolume = true;
			return;
		}

		//show credits
		GUI.Label (new Rect(Screen.width/2 - buttonWidth/2, 7 * Screen.height/7 + scrollOffset,
		                    tutorialTexture.width, tutorialTexture.height), style + "Who made this awesome game?" + endStyle, otherText);
		if(GUI.Button (new Rect(Screen.width/2 - buttonWidth/2, 7 * Screen.height/7 + 25 + scrollOffset,
		                       buttonWidth, buttonHeight), credits, buttonStyle)){
			Application.LoadLevel("Credits");
			return;
		}


		//return to main
		GUI.DrawTexture (new Rect(0, 8*Screen.height/7 + scrollOffset, Screen.width, Screen.height/7-25), buttonbg);
		if(GUI.Button (new Rect(Screen.width/2 - buttonWidth/2, 8*Screen.height/7 + scrollOffset,
		                        buttonWidth, buttonHeight), mainMenu, buttonStyle)){
			Application.LoadLevel ("Main_Menu");
			return;
		}

		//if there is a touch event, and activated a button, it won't get here.
		//if it did get here, check if we need to do any scrolling
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) {
			Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
			scrollOffset += touchDeltaPosition.y;
		}

		//constrain scrolling
		if(scrollOffset > 0)
			scrollOffset = 0;
		if(scrollOffset < -175)
			scrollOffset = -175;
	}

	//this method is probably the ugliest code I've written in this project
	//I blame Obama... or my laziness.  Whatever.  Leave me alone.
	private void volumeGUI(){
		GUIStyle volumeSliders = new GUIStyle();
		volumeSliders.stretchWidth = true;
		volumeSliders.stretchHeight = true;
		volumeSliders.font = chewy;
		volumeSliders.richText = true;
		volumeSliders.fontSize = Screen.width/18;
		GUIStyle returnButtonStyle = new GUIStyle();
		returnButtonStyle.font = chewy;
		returnButtonStyle.fontSize = 30;
		returnButtonStyle.alignment = TextAnchor.MiddleCenter;
		float baseVolume = 1;
		const string voiceVolKey = "voiceKey";
		const string effectVolKey = "effectKey";
		const string musicVolKey = "musicKey";
		float musicVolume = PlayerPrefs.GetFloat(musicVolKey,baseVolume) * (Screen.width - 50) + 25; 
		float effectVolume = PlayerPrefs.GetFloat(effectVolKey,baseVolume) * 2 * (Screen.width - 50) + 25; 
		float voiceVolume = PlayerPrefs.GetFloat(voiceVolKey,baseVolume) * 2 * (Screen.width - 50) + 25;

		GUI.Box(new Rect(Screen.width/2 - options.width/2, 10, options.width, options.height), options, buttonStyle);
	//music volume
		//draw bar
		GUI.Label(new Rect(25, Screen.height / 4 - 30, Screen.width - 50, 50), "<color=#ffffff>Music Volume</color>", volumeSliders);
		if(GUI.RepeatButton( new Rect(25, Screen.height / 4, Screen.width - 50, 50), "")){
			musicVolume = Input.mousePosition.x;
			musicVolume = constrain(musicVolume, 25, Screen.width - 25);
			if ((musicVolume - 25)/(Screen.width - 50) < .05) musicVolume = 25;
			PlayerPrefs.SetFloat(musicVolKey, (musicVolume - 25) / (Screen.width - 50));
			Debug.Log ("Music volume is: " + (musicVolume - 25) / (Screen.width - 50));
		}
		//draw slider
		GUI.Box ( new Rect(musicVolume - 25, Screen.height / 4, 50, 50), sliderButton, new GUIStyle());
	//effect volume
		//draw bar
		GUI.Label (new Rect(25, Screen.height / 2 - 30, Screen.width - 50, 50), "<color=#ffffff>Sound Effect Volume</color>", volumeSliders);
		if(GUI.RepeatButton( new Rect(25, Screen.height / 2, Screen.width - 50, 50), "")){
			effectVolume = Input.mousePosition.x;
			effectVolume = constrain(effectVolume, 25, Screen.width - 25);
			if ((effectVolume - 25)/2/(Screen.width - 50) < .05) effectVolume = 25;
			PlayerPrefs.SetFloat(effectVolKey,(effectVolume - 25) / 2 / (Screen.width - 50));
			Debug.Log ("Effect volume is: " + (effectVolume - 25) / 2 / (Screen.width - 50));
			//Debug.Log ("Ian: scale and store new effect volume here");
		}
		//draw slider
		GUI.Box ( new Rect(effectVolume - 25, Screen.height / 2, 50, 50), sliderButton, new GUIStyle());
	//voice volume
		//draw bar
		GUI.Label (new Rect(25, 3 * Screen.height / 4 - 30, Screen.width - 50, 50), "<color=#ffffff>Voice Volume</color>", volumeSliders);
		if(GUI.RepeatButton( new Rect(25, 3 * Screen.height / 4, Screen.width - 50, 50), "")){
			voiceVolume = Input.mousePosition.x;
			voiceVolume = constrain(voiceVolume, 25, Screen.width - 25);
			if ((voiceVolume - 25)/2/(Screen.width - 50) < .05) voiceVolume = 25;
			PlayerPrefs.SetFloat(voiceVolKey,(voiceVolume - 25) / 2 / (Screen.width - 50));
			Debug.Log ("Voice volume is: " + (voiceVolume - 25) / 2 / (Screen.width - 50));
		}
		//draw slider
		GUI.Box ( new Rect(voiceVolume - 25, 3 * Screen.height / 4, 50, 50), sliderButton, new GUIStyle());
	//return to root pause menu
		if(GUI.Button (new Rect(Screen.width/6, 5 * Screen.height/6, 2 * Screen.width / 3, Screen.height/6), "<color=#ffffff>Return</color>", returnButtonStyle)){
			adjustingVolume = false;
		}
	}

	private float constrain(float val, float min, float max){
		if(val < min) return min;
		if(val > max) return max;
		return val;
	}
}
