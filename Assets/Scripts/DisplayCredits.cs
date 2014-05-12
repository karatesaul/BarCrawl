using UnityEngine;
using System.Collections;

public class DisplayCredits : MonoBehaviour {

	string[] credits = {
		"Lead Designer\nCody Childers",
		"Lead Developer\nSaul Winer",
		"Lead Producer\nIan MacLeish",
		"Lead Tester\nKyle Sullivan",
		"Lead User Interface Designer\nKrishna Velury",
		"Lead Artist Coordinator\nMark McGowan",
		"Lead Level Designer\nSteven Hack",
		"Lead Artist\nRebecca Alto",
		"Other Visual Artist\nAlexis Williams\nPhoebe Rothfeld\nJes Udelle\nInsert Ian's guy here",
		"Lead Audio Designer\nChristopher Miller",
		"Additional Music\nLiquid Courage by Vio-Lence",
		"Voice acting\nLucahjin",
		"Special Thanks\nDavid Wessman"
	};
	float position = Screen.height;
	float increment = 1;
	int totalOffset = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		position -= increment;
		totalOffset = 0;
	}

	void OnGUI(){
		for(int i = 0; i < credits.Length; i++){
			GUIStyle centeredText = GUI.skin.GetStyle ("Label");
			Vector2 sizeOfLabel = centeredText.CalcSize (new GUIContent(credits[i]));
			//int w = Screen.width/2 - Mathf.RoundToInt (sizeOfLabel.x/2);
			GUI.Label (new Rect(Screen.width/2 - sizeOfLabel.x/2,
			                    position + totalOffset,
			                    sizeOfLabel.x,
			                    sizeOfLabel.y), credits[i], centeredText);
			totalOffset += Mathf.RoundToInt (sizeOfLabel.x) + 25;
		}
		if(totalOffset + position <= -5){
			Application.LoadLevel ("Main_Menu");
		}
	}
}

/*
 * function OnGUI () {
    var centeredStyle = GUI.skin.GetStyle("Label");
    centeredStyle.alignment = TextAnchor.UpperCenter;
    GUI.Label (Rect (Screen.width/2-50, Screen.height/2-25, 100, 50), "BLAH", centeredStyle);
}

Vector2 sizeOfLabel = myStyle.CalcSize(new GUIContent("My string"));
 * */
