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
			audio.clip = bikerBar;
			audio.Play ();
		}else if (GlobalMusic.LEVELNUM == 2) {
			audio.clip = luauBar;
			audio.Play ();
		}else if (GlobalMusic.LEVELNUM == 3) {
			audio.clip = raveBar;
			audio.Play ();
		}else if (GlobalMusic.LEVELNUM == 0) {
			audio.clip = vioLence;
			audio.Play ();
		} else {
			audio.clip = null;
			audio.Play();
		}
	}
	
	// Update is called once per frame
	void Update () {
		audio.volume = (PlayerPrefs.GetFloat (InGameMenu.musicVolKey) - 25) / (Screen.width / 50);
	}
}
