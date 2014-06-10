using UnityEngine;
using System.Collections;

public class Initializer : MonoBehaviour {

	//add scene names to this
	string[] possibleLevels = {"Biker_Bar_Scene1", "Biker_Bar_Scene2", "Biker_Bar_Scene3", "Beach_Bar_Scene1", "Beach_Bar_Scene2", "Beach_Bar_Scene3", "Club_Bar_Scene1", "Club_Bar_Scene2", "Club_Bar_Scene3"};

	bool debuggingForceLoad;
	string forceLevel;

	void Awake(){
		Application.targetFrameRate = 60;
	}

	void Start(){
		/**************************************************************************************************
		 *  Set this value to true and enter a level name to force test a level.  Change it back to false
		 *  when done.  While false, it will choose randomly from the set.  It should be false for the 
		 * final release
		 * ***********************************************************************************************/
		debuggingForceLoad = false;
		forceLevel = "";

		//set up player preferences for later if they haven't already
		if(!PlayerPrefs.HasKey("ViolenceMusic")){
			PlayerPrefs.SetInt("ViolenceMusic", 0);
		}
		if(!PlayerPrefs.HasKey ("Profanity")){
			PlayerPrefs.SetInt ("Profanity", 0);
		}
		if(!PlayerPrefs.HasKey ("ShowTutorial")){
			PlayerPrefs.SetInt ("ShowTutorial", 1);
		}
		if(!PlayerPrefs.HasKey ("HealthBarNumbers")){
			PlayerPrefs.SetInt("HealthBarNumbers", 0);
		}
		if (!PlayerPrefs.HasKey (InGameMenu.musicVolKey)) {
			PlayerPrefs.SetFloat(InGameMenu.musicVolKey, 1f);
		}
		if (!PlayerPrefs.HasKey (InGameMenu.effectVolKey)) {
			PlayerPrefs.SetFloat(InGameMenu.effectVolKey, .08f);
		}
		if (!PlayerPrefs.HasKey (InGameMenu.voiceVolKey)) {
			PlayerPrefs.SetFloat(InGameMenu.voiceVolKey, .1f);
		}

		//make sure there's no old instances hanging around causing bugs
		GridManager.clearInstance ();

		//transition into the level
		if(debuggingForceLoad){
			Application.LoadLevel(forceLevel);
		}else{
			//get a random number
			int rand = (int)Mathf.Floor(Random.Range(0, possibleLevels.Length));
			Application.LoadLevel(possibleLevels[rand]);
		}
	}
}
