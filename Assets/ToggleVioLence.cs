using UnityEngine;
using System.Collections;

public class ToggleVioLence : MonoBehaviour {

	public AudioClip vioLence;
	public AudioClip bikerBar;
	public AudioClip luauBar;
	public AudioClip raveBar;

	// Use this for initialization
	void Start () {
		if (GlobalMusic.LEVELNUM == 1) {
			GetComponent<AudioSource>().clip = bikerBar;
			GetComponent<AudioSource>().Play ();
		}else if (GlobalMusic.LEVELNUM == 2) {
			GetComponent<AudioSource>().clip = luauBar;
			GetComponent<AudioSource>().Play ();
		}else if (GlobalMusic.LEVELNUM == 3) {
			GetComponent<AudioSource>().clip = raveBar;
			GetComponent<AudioSource>().Play ();
		}else if (GlobalMusic.LEVELNUM == 0) {
			GetComponent<AudioSource>().clip = vioLence;
			GetComponent<AudioSource>().Play ();
		} else {
			GetComponent<AudioSource>().clip = null;
			GetComponent<AudioSource>().Play();
		}
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat (InGameMenu.musicVolKey);
	}
}
