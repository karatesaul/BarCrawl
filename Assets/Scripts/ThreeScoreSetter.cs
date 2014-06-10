using UnityEngine;
using System.Collections;

public class ThreeScoreSetter : MonoBehaviour {
	int baseScore = 0; //used to set original highscores to 0/

	//for displaying scores from this round
	public GUIText currText1;
	public GUIText currText2;
	public GUIText currText3;
	public GUIText currText4;

	//lifetimebest scores
	public GUIText scoreText1;
	public GUIText scoreText2;
	public GUIText scoreText3;
	public GUIText scoreText4;

	public GUIText results;
	public GUIText lifeTimeBest;


	//player's scores this game/
	int score1 = 10;
	int score2 = 20;
	int score3 = 50;
	int score4;

	//key for accessing where highscores are saved/
	string scoreKey1 = "Player Score 1";
	string scoreKey2 = "Player Score 2";
	string scoreKey3 = "Player Score 3";
	string scoreKey4 = "Player Score 4";

	//actual saved highscores.
	int highscore1 = 0;
	int highscore2 = 0;
	int highscore3 = 0;
	int highscore4 = 0;

	void Start(){
		score1 = Scores.enemiesKilled;
		score2 = Scores.maxCombo;
		score3 = Scores.turnsSurvived;
		score4 = Scores.total;
		//gets the saved score. If the saved score is undefined, set it to 0 (basescore).
		highscore1 = PlayerPrefs.GetInt(scoreKey1,baseScore); 
		highscore2 = PlayerPrefs.GetInt(scoreKey2,baseScore); 
		highscore3 = PlayerPrefs.GetInt(scoreKey3,baseScore); 
		highscore4 = PlayerPrefs.GetInt(scoreKey4,baseScore); 
		/*
		currText1.text = "Knockouts:\n" + PlayerPrefs.GetInt(scoreKey1);
		currText2.text = "Highest Combo:\n" + PlayerPrefs.GetInt(scoreKey2);
		currText3.text = "Turns Lasted:\n" + PlayerPrefs.GetInt(scoreKey3);
		currText4.text = "Composite:\n" + PlayerPrefs.GetInt(scoreKey4);
		*/

		currText1.text = "Knockouts:\n" + score1;
		currText2.text = "Highest Combo:\n" + score2;
		currText3.text = "Turns Lasted:\n" + score3;
		currText4.text = "Composite:\n" + score4;

		currText1.fontSize = Screen.width/20;
		currText2.fontSize = Screen.width/20;
		currText3.fontSize = Screen.width/20;
		currText4.fontSize = Screen.width/20;

		scoreText1.fontSize = Screen.width/20;
		scoreText2.fontSize = Screen.width/20;
		scoreText3.fontSize = Screen.width/20;
		scoreText4.fontSize = Screen.width/20;

		results.text = "Your Score!";
		results.fontSize = Screen.width/12;
		lifeTimeBest.text = "Life Time Best!";
		lifeTimeBest.fontSize = Screen.width/12;
	}

	void Update(){

		//updates highscores before display
		if(score1>highscore1){
			PlayerPrefs.SetInt(scoreKey1, score1);
			PlayerPrefs.Save();
		}
		if(score2>highscore2){
			PlayerPrefs.SetInt(scoreKey2, score2);
			PlayerPrefs.Save();
		}
		if(score3>highscore3){
			PlayerPrefs.SetInt(scoreKey3, score3);
			PlayerPrefs.Save();
		}
		if(score4>highscore4){
			PlayerPrefs.SetInt(scoreKey4, score4);
			PlayerPrefs.Save();
		}

		//pulls highscores from player prefs
		highscore1 = PlayerPrefs.GetInt(scoreKey1,baseScore); 
		highscore2 = PlayerPrefs.GetInt(scoreKey2,baseScore);
		highscore3 = PlayerPrefs.GetInt(scoreKey3,baseScore); 
		highscore4 = PlayerPrefs.GetInt(scoreKey4,baseScore); 

		//display scores to viewport
		scoreText1.text = "Knockouts:\n" + highscore1.ToString();
		scoreText2.text = "Highest Combo:\n" + highscore2.ToString();
		scoreText3.text = "Turns Lasted:\n" + highscore3.ToString();
		scoreText4.text = "Composite:\n" + highscore4.ToString();
	}


	void OnGUI(){
		/*
		//buttons for deleting individual highscores
		if (GUI.Button (new Rect (10, Screen.height-40, 120, 30), "Clear Scores1")) {
			PlayerPrefs.DeleteKey(scoreKey1);
			score1 = 0;
		}
		if (GUI.Button (new Rect (130, Screen.height-40, 120, 30), "Clear Scores2")) {
			PlayerPrefs.DeleteKey(scoreKey2);
			score2 = 0;
		}
		if (GUI.Button (new Rect (250, Screen.height-40, 120, 30), "Clear Scores3")) {
			PlayerPrefs.DeleteKey(scoreKey3);
			score3 = 0;
		}
		if (GUI.Button (new Rect (370, Screen.height-40, 120, 30), "Clear Scores4")) {
			PlayerPrefs.DeleteKey(scoreKey4);
			score4 = 0;
		}
		*/
	}
}
