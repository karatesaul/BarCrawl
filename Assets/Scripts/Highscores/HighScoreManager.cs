using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// High score manager.
/// Local highScore manager for LeaderboardLength number of entries
/// 
/// this is a singleton class.  to access these functions, use HighScoreManager._instance object.
/// eg: HighScoreManager._instance.SaveHighScore("meh",1232);
/// No need to attach this to any game object, thought it would create errors attaching.
/// </summary>

public class HighScoreManager : MonoBehaviour
{
	
	private static HighScoreManager m_instance;
	private const int LeaderboardLength = 10;
	
	public static HighScoreManager _instance {
		get {
			if (m_instance == null) {
				m_instance = new GameObject ("HighScoreManager").AddComponent<HighScoreManager> ();          
			}
			return m_instance;
		}
	}
	
	void Awake ()
	{
		if (m_instance == null) {
			m_instance = this;      
		} else if (m_instance != this)       
			Destroy (gameObject);    
		//DontDestroyOnLoad (gameObject);
	}
	
	public void SaveHighScore (string name, int score)
	{
		List<Score> HighScores = new List<Score> ();
		
		int i = 1;
		while (i<=LeaderboardLength && PlayerPrefs.HasKey("HighScore"+i+"score")) {
			Score temp = new Score ();
			temp.score = PlayerPrefs.GetInt ("HighScore" + i + "score");
			temp.name = PlayerPrefs.GetString ("HighScore" + i + "name");
			HighScores.Add (temp);
			i++;
		}
		if (HighScores.Count == 0) {       
			Score _temp = new Score ();
			_temp.name = name;
			_temp.score = score;
			HighScores.Add (_temp);
		} else {
			for (i=1; i<=HighScores.Count && i<=LeaderboardLength; i++) {
				if (score > HighScores [i - 1].score) {
					Score _temp = new Score ();
					_temp.name = name;
					_temp.score = score;
					HighScores.Insert (i - 1, _temp);
					break;
				}      
				if (i == HighScores.Count && i < LeaderboardLength) {
					Score _temp = new Score ();
					_temp.name = name;
					_temp.score = score;
					HighScores.Add (_temp);
					break;
				}
			}
		}
		
		i = 1;
		while (i<=LeaderboardLength && i<=HighScores.Count) {
			PlayerPrefs.SetString ("HighScore" + i + "name", HighScores [i - 1].name);
			PlayerPrefs.SetInt ("HighScore" + i + "score", HighScores [i - 1].score);
			i++;
		}
		
	}
	
	public List<Score>  GetHighScore ()
	{
		List<Score> HighScores = new List<Score> ();
		
		int i = 1;
		while (i<=LeaderboardLength && PlayerPrefs.HasKey("HighScore"+i+"score")) {
			Score temp = new Score ();
			temp.score = PlayerPrefs.GetInt ("HighScore" + i + "score");
			temp.name = PlayerPrefs.GetString ("HighScore" + i + "name");
			HighScores.Add (temp);
			i++;
		}
		
		return HighScores;
	}
	
	public void ClearLeaderBoard ()
	{
		//for(int i=0;i<HighScores.
		List<Score> HighScores = GetHighScore();
		
		for(int i=1;i<=HighScores.Count;i++)
		{
			PlayerPrefs.DeleteKey("HighScore" + i + "name");
			PlayerPrefs.DeleteKey("HighScore" + i + "score");
		}
	}
	
	void OnApplicationQuit()
	{
		PlayerPrefs.Save();
	}
}

public class Score
{
	public int score;
	
	public string name;
	
}