using UnityEngine;
using System.Collections;

public class BarMusic : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (PlayerPrefs.GetInt ("ViolenceMusic") == 1) {
			GlobalMusic.ViolenceMusic();
		} else {
			if(Application.loadedLevelName == "Biker_Bar_Scene1" || Application.loadedLevelName == "Biker_Bar_Scene2" || Application.loadedLevelName == "Biker_Bar_Scene3"){
				GlobalMusic.BikerBarMusic();
			}else if(Application.loadedLevelName == "Luau_Bar_Scene1" || Application.loadedLevelName == "Luau_Bar_Scene2" || Application.loadedLevelName == "Luau_Bar_Scene3"){
				GlobalMusic.LuauMusic();
			}else if(Application.loadedLevelName == "Rave_Bar_Scene1" || Application.loadedLevelName == "Rave_Bar_Scene2" || Application.loadedLevelName == "Rave_Bar_Scene3"){
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
