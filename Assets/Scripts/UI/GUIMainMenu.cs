using UnityEngine;
using System.Collections;

public class GUIMainMenu : MonoBehaviour {

	//add scene names to this
	string[] possibleLevels = {"Combined_Test_Scene"};

	static bool firstInit;//this will control the fancy MonoAxe Games presents stuff only on the first load
	bool debuggingForceLoad;
	string forceLevel;

	void Start(){
		firstInit = true;
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
	}
	
	void Update(){
		if(firstInit){
			//show the team logo and shit
			//turn firstInit to false when done
			firstInit = false;
		}else{
			if(debuggingForceLoad){
				Application.LoadLevel(forceLevel);
			}else{
				//get a random number
				int rand = (int)Mathf.Floor(Random.Range(0, possibleLevels.Length));
				Application.LoadLevel(possibleLevels[rand]);
			}
		}
	}
	
}
