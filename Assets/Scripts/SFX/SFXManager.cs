using UnityEngine;
using System.Collections;

public class SFXManager : MonoBehaviour {

	public static AudioSource[] uncensored = {null/*others*/};
	public static AudioSource[] censored = {null/*others*/};

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		//audioSource = GetComponentInChildren<AudioSource> ();
	}

	public static void PlayerVoice(){
		switch(PlayerPrefs.GetInt ("Profanity")){
		case 0:
			playCensored();
			break;
		case 1:
			playUncensored();
			break;
		}
	}

	private static void playCensored(){
		int rand = Mathf.RoundToInt(Random.Range(0, censored.Length - 1));
		censored[rand].Play();
	}

	private static void playUncensored(){
		int rand = Mathf.RoundToInt(Random.Range(0, uncensored.Length - 1));
		uncensored[rand].Play();
	}
}
