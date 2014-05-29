using UnityEngine;
using UnityEditor;
using System.Collections;

public class SFXManager : MonoBehaviour {

	public static AudioClip[] uncensored = {null/*others*/};
	public static AudioClip[] censored = {null/*others*/};
	public static GameObject player;

	// Use this for initialization
	void Start () {
		player = gameObject;
		uncensored = new AudioClip[17];
		//This should iterate to the number of voice files.
		for (int i = 0; i < 17; i++) {
			uncensored[i] = AssetDatabase.LoadAssetAtPath("Assets/Sound/Voice/Uncensored/"+(i+1).ToString ()+".wav", typeof(AudioClip)) as AudioClip;
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
		AudioSource.PlayClipAtPoint (censored [rand], player.transform.position);
	}

	private static void playUncensored(){
		int rand = Mathf.RoundToInt(Random.Range(0, uncensored.Length - 1));
		AudioSource.PlayClipAtPoint(uncensored[rand], player.transform.position);
	}
}
