using UnityEngine;
using System.Collections;

public class ToggleVioLence : MonoBehaviour {

	public AudioClip vioLence;
	public AudioClip nonVioLence;

	// Use this for initialization
	void Start () {
		if(GlobalMusic.LEVELNUM == 0){
			audio.clip = vioLence;
			audio.Play();
		}else{
			audio.clip = nonVioLence;
			audio.Play();
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
