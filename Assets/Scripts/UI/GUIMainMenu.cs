using UnityEngine;
using System.Collections;

public class GUIMainMenu : MonoBehaviour {
	public Texture teamLogo;

	//add scene names to this
	string[] possibleLevels = {"Combined_Test_Scene"};

	static bool firstInit;//this will control the fancy MonoAxe Games presents stuff only on the first load
	private const int maxDisplayLogoTime = 500;
	private int currentDisplayLogoTime = 0;
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

	void OnGUI(){
		if(firstInit){
			GUI.Label(new Rect(Screen.width/2 - teamLogo.width/2, Screen.height/2 - teamLogo.height/2, 
			                   teamLogo.width, teamLogo.height), teamLogo);
			currentDisplayLogoTime++;
			if(currentDisplayLogoTime >= maxDisplayLogoTime){
				firstInit = false;
			}
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
