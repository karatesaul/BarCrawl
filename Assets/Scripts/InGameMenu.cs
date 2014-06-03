using UnityEngine;
using System.Collections;

public class InGameMenu : MonoBehaviour {

	public bool menuActive;
	public TurnManager tm;
	public PlayerCharacter pc;
	public PuzzleManager pm;
	public Texture2D pauseButton;
	public Texture2D healthbar;
	public Texture2D emptybar;
	public Texture2D clock1;
	public Texture2D clock2;
	public Texture2D clock3;
	public Texture2D clock4;
	public GameObject puzzleManager;
	public Font chewy;

	public bool combo2;
	public bool combo3;
	public bool combo4;
	public bool combo5;
	public bool tut1;
	public bool tut2;
	public bool tut3;
	public bool showNumericHealth;
	private float timer;

	private bool paused;
	private bool showInstructions;

	private string ins = "How to Play:\n" +
						 "- Move a bottle cap as far as you want\n" +
						 "- Make matches of 3 or more\n" +
						 "- Matches of 4 or more can start combos\n" +
						 "- Moving into an enemy will damage them\n" +
						 "- You have a limited time to move each bottlecap\n" +
						 "- Drunkenly brawl for as long as possible!\n";
	private GUIStyle insFormatting;
	private GUIStyle gs;
	private GUIStyle healthtext;
	private GUIStyle scoreText;
	private GUIStyle comboText;

	// Use this for initialization
	void Start () {
		this.menuActive = false;
		pauseButton.Resize (Screen.width/9, Screen.width/9);
		paused = false;
		showInstructions = false;
		tm = GameObject.Find ("Player").GetComponent<TurnManager> ();
		pc = GameObject.Find ("Player").GetComponent<PlayerCharacter> ();
		pm = puzzleManager.GetComponent<PuzzleManager> ();
		showNumericHealth = PlayerPrefs.GetInt("HealthBarNumbers") == 1;

		insFormatting = new GUIStyle();
		insFormatting.alignment = TextAnchor.UpperLeft;
		insFormatting.clipping = TextClipping.Clip;
		insFormatting.wordWrap = true;
		insFormatting.font = chewy;
		insFormatting.fontSize = Screen.width/20;

		gs = new GUIStyle();
		gs.font = chewy;
		gs.fontSize = Screen.width/10;
		gs.alignment = TextAnchor.MiddleCenter;
		gs.stretchWidth = true;
		gs.stretchHeight = true;

		healthtext = new GUIStyle ();
		healthtext.font = chewy;
		healthtext.fontSize = 14;
		healthtext.normal.textColor = Color.white;
		healthtext.alignment = TextAnchor.UpperCenter;

		scoreText = new GUIStyle ();
		scoreText.font = chewy;
		scoreText.fontSize = Screen.width/15;
		scoreText.normal.textColor = Color.white;
		scoreText.alignment = TextAnchor.UpperCenter;

		comboText = new GUIStyle ();
		comboText.font = chewy;
		comboText.fontSize = Screen.width/25;
		comboText.normal.textColor = Color.white;
		comboText.alignment = TextAnchor.UpperCenter;

		timer = 3.0f;
		combo2 = false;
		combo3 = false;
		combo4 = false;
		combo5 = false;
		tut1 = false;
		tut2 = false;
		tut3 = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (combo2) {
			timer -= Time.deltaTime;
			if (timer < 0.0f) {
				combo2 = false;
				timer = 3.0f;
			}
		}
		if (combo3) {
			timer -= Time.deltaTime;
			if (timer < 0.0f) {
				combo3 = false;
				timer = 3.0f;
			}
		}
		if (combo4) {
			timer -= Time.deltaTime;
			if (timer < 0.0f) {
				combo4 = false;
				timer = 3.0f;
			}
		}
		if (combo5) {
			timer -= Time.deltaTime;
			if (timer < 0.0f) {
				combo5 = false;
				timer = 3.0f;
			}
		}

	}

	void OnGUI(){
		if(!menuActive) return;

		//healthbar
		GUI.DrawTexture (new Rect (Screen.width/2 - pc.startingHealth, Screen.height/7,
		                         pc.startingHealth*2, emptybar.height), emptybar);
		GUI.DrawTexture (new Rect (Screen.width/2 - pc.startingHealth, Screen.height/7,
		                           pc.health*2, healthbar.height), healthbar);
		if (showNumericHealth) DrawOutline (new Rect (Screen.width / 2, Screen.height / 7, 0, 0), pc.health + "/100", healthtext, Color.black);

		//score
		DrawOutline (new Rect (Screen.width / 2, Screen.height / 6, 0, 0), "Score: " + Scores.total.ToString(), scoreText, Color.black);

		//combotext
		if (combo2) DrawOutline(new Rect (Screen.width / 2, Screen.height / 5, 0, 0), "2X combo!", comboText, Color.black);
		if (combo3) DrawOutline(new Rect (Screen.width / 2, Screen.height / 5, 0, 0), "3X combo!", comboText, Color.black);
		if (combo4) DrawOutline(new Rect (Screen.width / 2, Screen.height / 5, 0, 0), "4X combo!", comboText, Color.black);
		if (combo5) DrawOutline(new Rect (Screen.width / 2, Screen.height / 5, 0, 0), "Crazy Combo!", comboText, Color.red);

		//visual timer
		if (pm.currTime <= 400 && pm.currTime > 300) GUI.DrawTexture(new Rect (Screen.width/25, Screen.height/8, 4*Screen.width/25, 4*Screen.height/45), clock4);
		else if (pm.currTime <= 300 && pm.currTime > 200) GUI.DrawTexture(new Rect (Screen.width/25, Screen.height/8, 4*Screen.width/25, 4*Screen.height/45), clock3);
		else if (pm.currTime <= 200 && pm.currTime > 100) GUI.DrawTexture(new Rect (Screen.width/25, Screen.height/8, 4*Screen.width/25, 4*Screen.height/45), clock2);
		else if (pm.currTime <= 100) GUI.DrawTexture(new Rect (Screen.width/25, Screen.height/8, 4*Screen.width/25, 4*Screen.height/45), clock1);
		else {
		//draw nothing
		}

		//tutorial text
		if (tut1) DrawOutline (new Rect (Screen.width / 2, Screen.height / 3, 0, 0), "Combine 3 punches to fight enemies!", comboText, Color.black);
		if (tut2) {
			DrawOutline (new Rect (Screen.width / 2, Screen.height / 3, 0, 0), "Make more matches to do more moves!", comboText, Color.black);
			DrawOutline (new Rect (Screen.width / 2, Screen.height/3 + Screen.width/25, 0, 0), "Moving into enemies will damage them too!", comboText, Color.black);
		}
		if (tut3) {
			DrawOutline (new Rect (Screen.width / 2, Screen.height / 3, 0, 0), "Connect hearts to restore health!", comboText, Color.black);
			DrawOutline (new Rect (Screen.width / 2, Screen.height/3 + Screen.width/25, 0, 0), "4+ punches or hearts results in a greater effect!", comboText, Color.black);
		}

		if (tm.coolDownTimer > 4) {
			if(GUI.Button (new Rect(Screen.width/25, Screen.height/45, 4*Screen.width/25, 4*Screen.height/45), "Reset the\nPuzzle!") && tm.turn == 1){
				puzzleManager.GetComponent<PuzzleManager>().ResetPuzzle();
				tm.coolDownTimer = 0;
			}
		}
		else{
			GUI.Box (new Rect(Screen.width/25, Screen.height/45, 4*Screen.width/25, 4*Screen.height/45), "Reset\nRecharging!");
		}
		if(GUI.Button(new Rect(Screen.width-pauseButton.width, 0, pauseButton.width, pauseButton.height), pauseButton)){
			paused = !paused;
			if(paused){
				puzzleManager.GetComponent<PuzzleManager>().puzzleActive = false;
			}else{
				puzzleManager.GetComponent<PuzzleManager>().puzzleActive = true;
			}
		}
		if(paused && !showInstructions){
			//return to game
			if(GUI.Button (new Rect(Screen.width/2-Screen.width/4, Screen.height/2, Screen.width/2, Screen.height/6), "Return to Game", gs)){
				paused = false;
				puzzleManager.GetComponent<PuzzleManager>().puzzleActive = true;
			}
			//how to play
			if(GUI.Button (new Rect(Screen.width/2 - Screen.width/4, 4*Screen.height/6, Screen.width/2, Screen.height/6), "How to play", gs)){
				showInstructions = true;
			}
			//return to main menu
			if(GUI.Button (new Rect(Screen.width/2-Screen.width/4, 5*Screen.height/6, Screen.width/2, Screen.height/6), "Main Menu", gs)){
				Application.LoadLevel("Main_Menu");
			}
		}
		if(paused && showInstructions){
			GUI.Box (new Rect(5, Screen.height/2, Screen.width - 5, Screen.height/2), ins, insFormatting);
			if(GUI.Button (new Rect(Screen.width/6, 5 * Screen.height/6, 2 * Screen.width / 3, Screen.height/6), "Return", gs)){
				showInstructions = false;
			}
		}
	}

	void DrawOutline(Rect position, string text, GUIStyle style, Color outColor){
		GUIStyle backupStyle = style;
		Color oldColor = style.normal.textColor;
		style.normal.textColor = outColor;
		position.x--;
		GUI.Label(position, text, style);
		position.x +=2;
		GUI.Label(position, text, style);
		position.x--;
		position.y--;
		GUI.Label(position, text, style);
		position.y +=2;
		GUI.Label(position, text, style);
		position.y--;
		style.normal.textColor = oldColor;
		GUI.Label(position, text, style);
		style = backupStyle;   
	}
}
