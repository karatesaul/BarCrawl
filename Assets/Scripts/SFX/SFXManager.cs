using UnityEngine;
//using UnityEditor;
using System.Collections;

public class SFXManager : MonoBehaviour {

	public static AudioClip[] uncensored = {null/*others*/};
	public static AudioClip[] censored = {null/*others*/};
	public static GameObject currentAudio;
	public static GameObject player;

	// Use this for initialization
	void Start () {
		player = gameObject;
		uncensored = new AudioClip[18];
		censored = new AudioClip[18];
		//This should iterate to the number of voice files.
		for (int i = 0; i < 8; i++) {
			uncensored[i] = Resources.Load("Uncensored/"+(i+1).ToString (), typeof(AudioClip)) as AudioClip;
		}
		for (int i = 0; i < 8; i++) {
			censored[i] = Resources.Load("Censored/"+(i+1).ToString (), typeof(AudioClip)) as AudioClip;
		}
		for (int i = 8; i < 18; i++) {
			uncensored[i] = Resources.Load("Nonvulgar/"+(i+1).ToString (), typeof(AudioClip)) as AudioClip;
		}
		for (int i = 8; i < 18; i++) {
			censored[i] = Resources.Load("Nonvulgar/"+(i+1).ToString (), typeof(AudioClip)) as AudioClip;
		}

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
		if (currentAudio == null) {
			AudioSource.PlayClipAtPoint (censored [rand], player.transform.position);
			currentAudio = GameObject.Find ("One shot audio");
		} 
		else
			Debug.Log ("Voice overlap prevented!");
	}

	private static void playUncensored(){
		int rand = Mathf.RoundToInt(Random.Range(0, uncensored.Length - 1));
		if (currentAudio == null) {
		AudioSource.PlayClipAtPoint (uncensored [rand], player.transform.position);
			currentAudio = GameObject.Find ("One shot audio");
		} 
		else
			Debug.Log ("Voice overlap prevented!");
	}
}
