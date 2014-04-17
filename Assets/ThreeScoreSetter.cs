using UnityEngine;
using System.Collections;

public class ThreeScoreSetter : MonoBehaviour {
	int baseScore = 0; //used to set original highscores to 0/

	//for displaying scores
	public GUIText scoreText1;
	public GUIText scoreText2;
	public GUIText scoreText3;


	//player's scores this game/
	int score1 = 10;
	int score2 = 20;
	int score3 = 50;

	//key for accessing where highscores are saved/
	string scoreKey1 = "Player Score 1";
	string scoreKey2 = "Player Score 2";
	string scoreKey3 = "Player Score 3";

	//actual saved highscores.
	int highscore1 = 0;
	int highscore2 = 0;
	int highscore3 = 0;

	void Start(){
		//gets the saved score. If the saved score is undefined, set it to 0 (basescore).
		highscore1 = PlayerPrefs.GetInt(scoreKey1,baseScore); 
		Debug.Log("score1: " + PlayerPrefs.GetInt(scoreKey1));
		highscore2 = PlayerPrefs.GetInt(scoreKey2,baseScore); 
		Debug.Log("score2: " + PlayerPrefs.GetInt(scoreKey2));
		highscore3 = PlayerPrefs.GetInt(scoreKey3,baseScore); 
		Debug.Log("score3: " + PlayerPrefs.GetInt(scoreKey3));
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

		//pulls highscores from player prefs
		highscore1 = PlayerPrefs.GetInt(scoreKey1,baseScore); 
		highscore2 = PlayerPrefs.GetInt(scoreKey2,baseScore);
		highscore3 = PlayerPrefs.GetInt(scoreKey3,baseScore); 

		//display scores to viewport
		scoreText1.text = "Score 1:\n" + highscore1.ToString();
		scoreText2.text = "Score 2:\n" + highscore2.ToString();
		scoreText3.text = "Score 3:\n" + highscore3.ToString();
	}


	void OnGUI(){

		//Reset scores to original values
		if (GUI.Button (new Rect (10, 70, 120, 30), "Reset Scores")) {
			PlayerPrefs.DeleteKey(scoreKey1);
			PlayerPrefs.DeleteKey(scoreKey2);
			PlayerPrefs.DeleteKey(scoreKey3);
			score1 = 10;
			score2 = 20;
			score3 = 50;
		}


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


		//testing score replacement
		if (GUI.Button (new Rect (10, Screen.height-200, 120, 30), "Player Scored 5")) {
			score1 = 5;
			score2 = 5;
			score3 = 5;
		}
		if (GUI.Button (new Rect (130, Screen.height-200, 120, 30), "Player Scored 25")) {
			score1 = 25;
			score2 = 25;
			score3 = 25;
		}
		if (GUI.Button (new Rect (250, Screen.height-200, 120, 30), "Player Scored 55")) {
			score1 = 55;
			score2 = 55;
			score3 = 55;
		}

	}
}
