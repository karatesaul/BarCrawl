using UnityEngine;
using System.Collections;

public class BarMusic : MonoBehaviour {

	// Use this for initialization
	void Awake() {
		Application.targetFrameRate = 60;
		if (PlayerPrefs.GetInt ("ViolenceMusic") == 1) {
			GlobalMusic.ViolenceMusic();
			//Debug.Log("Violence Music: " + PlayerPrefs.GetInt ("ViolenceMusic"));
		} else {
			if(Application.loadedLevelName == "Biker_Bar_Scene1" || Application.loadedLevelName == "Biker_Bar_Scene2" || Application.loadedLevelName == "Biker_Bar_Scene3"){
				GlobalMusic.BikerBarMusic();
			}else if(Application.loadedLevelName == "Beach_Bar_Scene1" || Application.loadedLevelName == "Beach_Bar_Scene2" || Application.loadedLevelName == "Beach_Bar_Scene3"){
				GlobalMusic.LuauMusic();
			}else if(Application.loadedLevelName == "Club_Bar_Scene1" || Application.loadedLevelName == "Club_Bar_Scene2" || Application.loadedLevelName == "Club_Bar_Scene3"){
				GlobalMusic.RaveMusic();
			}else{
				GlobalMusic.ViolenceMusic();
			}
		}

		//Debug.Log ("Levelnum: " + GlobalMusic.LEVELNUM);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
