using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuController : MonoBehaviour {

	//need to be made global so that other scenes can manipulate them.
	int score = 100;

	bool enterScore = true; //used to determine if a new score needs to be inputed or
							//if score is being observed from main meu



	public GUIStyle tehFont;
	public Font f;
	string name = System.DateTime.Now.ToString ();
	//string score="";
	List<Scores> highscore;

	public Texture2D checkScore;
    public Texture2D resetButtonTex;
	
	// Use this for initialization
	void Start () {
		//EventManager._instance._buttonClick += ButtonClicked;
		
		highscore = new List<Scores>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnGUI()
	{


		if (enterScore == true) {
			/*Code that would implement player name... IF THERE WAS A KEYBOARD
						GUI.Label (new Rect (Screen.width / 3, (Screen.height * 3 / 5) - 30, Screen.width / 3, Screen.height / 5), "Player Name: ");
						name = GUI.TextField (new Rect (Screen.width / 3, Screen.height * 3 / 5, Screen.width / 3, Screen.height / 5), name,5);
			*/
			GUI.color = Color.black;
			GUI.Label (new Rect (Screen.width/2-90, Screen.height/2-20, 200, 20), "Score: " + score.ToString (),tehFont);
			GUI.color = Color.white;
			if (GUI.Button (new Rect(Screen.width/2 - 90,Screen.height-100,180,90),checkScore)) {
								HighScoreManager._instance.SaveHighScore (name, score);
								highscore = HighScoreManager._instance.GetHighScore ();
								enterScore = false;
				}
						

			} else {
					highscore = HighScoreManager._instance.GetHighScore ();
					//if(GameManager.enterScore == false)
					GUI.color = Color.black;
					GUI.Label(new Rect(Screen.width/4, Screen.height/5, 100,50),"Date",tehFont);
					GUI.Label(new Rect(Screen.width/4*2.4f, Screen.height/5, 100,50), "Score",tehFont);
					GUI.color = Color.white;

            if(GUI.Button(new Rect (Screen.width - 100, Screen.height - 100, 90, 90), resetButtonTex))
				{
					HighScoreManager._instance.ClearLeaderBoard();
					highscore = HighScoreManager._instance.GetHighScore();
				}
			}

		

		

		GUILayout.Space(Screen.height/5*1.25f);
		foreach(Scores _score in highscore)
		{
			GUILayout.BeginHorizontal();
			GUI.color = Color.black;
			GUI.skin.font = f;
			GUILayout.Label("",GUILayout.Width(Screen.width/4));
			GUILayout.Label(""+_score.name,GUILayout.Width(Screen.width/4*1.39f));
			GUILayout.Label(""+_score.score,GUILayout.Width(Screen.width/4*2.4f));
			GUI.color = Color.white;
			GUILayout.EndHorizontal();
		}
	}
}