﻿using UnityEngine;
using System.Collections;

public class UI : MonoBehaviour {
	
	public bool showUI;
	public TurnManager tm;
	public PlayerCharacter pc;
	public PuzzleManager pm;
	public Texture2D healthbar;
	public Texture2D healthbg;
	public Texture2D emptybar;
	public Texture2D clock1;
	public Texture2D clock2;
	public Texture2D clock3;
	public Texture2D clock4;
	public Texture2D clock5;
	public Texture2D clock6;
	public Texture2D beer0;
	public Texture2D beer1;
	public Texture2D beer2;
	public Texture2D beer3;
	public Texture2D beerBackground;
	public Font chewy;
	
	public bool combo2;
	public bool combo3;
	public bool combo4;
	public bool combo5;
	public bool tut1;
	public bool tut2;
	public bool tut3;
	public bool tut4;
	public bool showNumericHealth;
	private float timer;

	private Color c;

	private GUIStyle healthtext;
	private GUIStyle scoreText;
	private GUIStyle comboText;
	private GUIStyle tutText;
	private GUIStyle buttonStyle;
	
	
	// Use this for initialization
	void Start () {
		this.showUI = false;
		tm = GameObject.Find ("Player").GetComponent<TurnManager> ();
		pc = GameObject.Find ("Player").GetComponent<PlayerCharacter> ();
		pm = GameObject.Find("PuzzleManager").GetComponent<PuzzleManager> ();
		showNumericHealth = PlayerPrefs.GetInt("HealthBarNumbers") == 1;
		
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

		tutText = new GUIStyle ();
		tutText.font = chewy;
		tutText.fontSize = Screen.width/20;
		tutText.normal.textColor = Color.white;
		tutText.alignment = TextAnchor.UpperCenter;
		
		buttonStyle = new GUIStyle();
		
		timer = 3.0f;
		combo2 = false;
		combo3 = false;
		combo4 = false;
		combo5 = false;
		tut1 = false;
		tut2 = false;
		tut3 = false;
		tut4 = false;
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


		if (showUI) {
			//drawing the healthbar

			//border (need to remove?)
			c.a = 0.5f;
			GUI.color = c;
			GUI.DrawTexture (new Rect (Screen.width/2 - Screen.width/4, Screen.height/35, Screen.width/2, Screen.height/30), emptybar);

			//background
			GUI.color = Color.white;
			GUI.DrawTexture (new Rect (Screen.width/2 - Screen.width/4, Screen.height/35, Screen.width/2, Screen.height/30), healthbg);

			//bg
			c = Color.white;
			c.a = 0.4f;
			GUI.color = c;
			GUI.DrawTexture (new Rect (Screen.width/2 - Screen.width/4, Screen.height/35, Screen.width/2, Screen.height/30), healthbar);

			//actual bar
			//set alpha to dark
			c.a = 5.0f;
			GUI.color = Color.red;
			GUI.DrawTexture (new Rect (Screen.width/2 - (Screen.width/2)*((float)pc.health/pc.startingHealth)/2, Screen.height/35,
			                           (Screen.width/2)* ((float)pc.health/pc.startingHealth), Screen.height/30), healthbar);

			//set color/alpha back to normal
			GUI.color = Color.white;

			if (showNumericHealth) DrawOutline (new Rect (Screen.width / 2, Screen.height/35, 0, 0), pc.health + "/100", healthtext, Color.black);
			
			//score
			DrawOutline (new Rect (Screen.width / 2, Screen.height/22, 0, 0), "Score: " + Scores.total.ToString(), scoreText, Color.black);
			
			//combotext
			if (combo2) DrawOutline(new Rect (Screen.width/2, Screen.height/12, 0, 0), "2X combo!", comboText, Color.black);
			if (combo3) DrawOutline(new Rect (Screen.width/2, Screen.height/12, 0, 0), "3X combo!", comboText, Color.black);
			if (combo4) DrawOutline(new Rect (Screen.width/2, Screen.height/12, 0, 0), "4X combo!", comboText, Color.black);
			if (combo5) DrawOutline(new Rect (Screen.width/2, Screen.height/12, 0, 0), "Crazy Combo!", comboText, Color.red);
			
			//visual timer
			if (pm.currTime <= 600 && pm.currTime > 500) GUI.DrawTexture(new Rect (Screen.width/2 - 2*Screen.width/25, Screen.width * 5/6 - Screen.width/6, 4*Screen.width/25, 4*Screen.height/45), clock1);
			else if (pm.currTime <= 500 && pm.currTime > 400) GUI.DrawTexture(new Rect (Screen.width/2 - 2*Screen.width/25, Screen.width * 5/6 - Screen.width/6, 4*Screen.width/25, 4*Screen.height/45), clock2);
			else if (pm.currTime <= 400 && pm.currTime > 300) GUI.DrawTexture(new Rect (Screen.width/2 - 2*Screen.width/25, Screen.width * 5/6 - Screen.width/6, 4*Screen.width/25, 4*Screen.height/45), clock3);
			else if (pm.currTime <= 300 && pm.currTime > 200) GUI.DrawTexture(new Rect (Screen.width/2 - 2*Screen.width/25, Screen.width * 5/6 - Screen.width/6, 4*Screen.width/25, 4*Screen.height/45), clock4);
			else if (pm.currTime <= 200 && pm.currTime > 100) GUI.DrawTexture(new Rect (Screen.width/2 - 2*Screen.width/25, Screen.width * 5/6 - Screen.width/6, 4*Screen.width/25, 4*Screen.height/45), clock5);
			else if (pm.currTime <= 100) GUI.DrawTexture(new Rect (Screen.width/2 - 2*Screen.width/25, Screen.width * 5/6 - Screen.width/6, 4*Screen.width/25, 4*Screen.height/45), clock6);
			else {
				//draw nothing
			}
			
			//reset button
			GUI.Box (new Rect(Screen.width/25, Screen.height/45, 4*Screen.width/25, 4*Screen.height/45), beerBackground, buttonStyle);
			if (tm.coolDownTimer > 4) {
				if (GUI.Button (new Rect(Screen.width/25, Screen.height/45, 4*Screen.width/25, 4*Screen.height/45), beer3, buttonStyle) && tm.turn ==1) {
					if (pm.tutorialState == 0){
						PuzzleManager.upVal = 150;
						PuzzleManager.downVal = 150;
						PuzzleManager.leftVal = 150;
						PuzzleManager.rightVal = 150;
						PuzzleManager.attackVal = 350;
						PuzzleManager.healVal = 50;
						pm.ResetPuzzle();
						tm.coolDownTimer = 0;
					} else if (pm.tutorialState == 13){
						pm.ResetPuzzle();
						pm.tutorialState = 0;
						tm.coolDownTimer = 0;
						tut4 = false;
					}
				}
			}
			else if (tm.coolDownTimer == 4) GUI.Box (new Rect(Screen.width/25, Screen.height/45, 4*Screen.width/25, 4*Screen.height/45), beer2, buttonStyle);
			else if (tm.coolDownTimer == 2 || tm.coolDownTimer == 3) 
				GUI.Box (new Rect(Screen.width/25, Screen.height/45, 4*Screen.width/25, 4*Screen.height/45), beer1, buttonStyle);
			else if (tm.coolDownTimer <= 1) GUI.Box (new Rect(Screen.width/25, Screen.height/45, 4*Screen.width/25, 4*Screen.height/45), beer0, buttonStyle);
			
			/*
			if (tm.coolDownTimer > 4) {
				if(GUI.Button (new Rect(Screen.width/25, Screen.height/45, 4*Screen.width/25, 4*Screen.height/45), "Reset the\nPuzzle!") && tm.turn == 1){
					pm.ResetPuzzle();
					tm.coolDownTimer = 0;
				}
			}
			else{
				GUI.Box (new Rect(Screen.width/25, Screen.height/45, 4*Screen.width/25, 4*Screen.height/45), "Reset\nRecharging!");
			}
			*/
			
			//tutorial text
			if (tut1) { 
				DrawOutline (new Rect (Screen.width / 2, Screen.height / 3, 0, 0), "Combine 3 punches to fight enemies!", tutText, Color.black);
			}
			if (tut2) {
				DrawOutline (new Rect (Screen.width / 2, Screen.height / 3, 0, 0), "Make more matches to do more moves!", tutText, Color.black);
				DrawOutline (new Rect (Screen.width / 2, Screen.height/3 + Screen.width/25, 0, 0), "Moving into enemies will damage them too!", tutText, Color.black);
			}
			if (tut3) {
				DrawOutline (new Rect (Screen.width / 2, Screen.height / 3, 0, 0), "Connect hearts to restore health!", tutText, Color.black);
				DrawOutline (new Rect (Screen.width / 2, Screen.height/3 + Screen.width/25, 0, 0), "4+ punches or hearts results in a greater effect!", tutText, Color.black);
			}
			if (tut4) {
				DrawOutline (new Rect (Screen.width/2 - 2*Screen.width/25, Screen.height/22 + 3*Screen.width/25, 0, 0), "Touch the beer", tutText, Color.black);
				DrawOutline (new Rect (Screen.width/2 - 2*Screen.width/25, Screen.height/22 + 4*Screen.width/25, 0, 0), "to reset the puzzle!", tutText, Color.black);
				DrawOutline (new Rect (Screen.width / 2, Screen.height/3, 0, 0), "From now on you have a limited time.", tutText, Color.black);
				DrawOutline (new Rect (Screen.width / 2, Screen.height/3 + Screen.width/25, 0, 0), "to complete each move. Good luck!", tutText, Color.black);
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
