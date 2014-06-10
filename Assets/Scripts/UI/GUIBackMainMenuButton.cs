using UnityEngine;
using System.Collections;

public class GUIBackMainMenuButton : MonoBehaviour {

	//public AudioClip buttonPressClip;
	
	public Texture2D backButton;
	public Texture2D fbPlaceholder;
	public Texture findUsOnFacebook;
    public Texture PostToFB;
    public Texture LogIn;
	public Texture2D buttonbg;
	public Font chewy;

	private GUIStyle textStyle;
    bool isInit = false;
	private GUIStyle imageStyle;

	//float buttony2 = Screen.height/9f;

    void Awake() {
        // Initialize FB SDK 
      //  Debug.Log( "Awake -- enabled = false" );     
       // enabled = false;
        Debug.Log( "going into fb.init" );
        if( !isInit ) {
            FB.Init( SetInit, OnHideUnity );
        }
    }

    private void SetInit() {
       // Debug.Log( "SetInit -- enabled = true" );
       // enabled = true; // "enabled" is a property inherited from MonoBehaviour   
        isInit = true;       
        if( FB.IsLoggedIn ) {
            Debug.Log( "Already logged in" );
            OnLoggedIn();
        }
    }  

    private void OnHideUnity( bool isGameShown ) {
        Debug.Log( "OnHideUnity" );
        if( !isGameShown ) {
            // pause the game - we will need to hide                                             
            Time.timeScale = 0;
        } else {
            // start the game back up - we're getting focus again                                
            Time.timeScale = 1;
        }  
    }

	void Update(){
		//buttony2 = Screen.height/9f;
		textStyle = new GUIStyle();
		textStyle.font = chewy;
		textStyle.fontSize = Screen.width / 10;
		textStyle.alignment = TextAnchor.MiddleCenter;
		textStyle.richText = true;
		imageStyle = new GUIStyle();
	}


	void OnGUI () {
		//if (GUI.Button (new Rect (Screen.width/1.4f,Screen.height/1.2f,buttony2,buttony2), backButton)) {
		GUI.DrawTexture (new Rect(0, Screen.height-100, Screen.width, 100), buttonbg);
		if(GUI.Button (new Rect(0, Screen.height-100, Screen.width, 100), "<color=#ffffff>On to the next bar!</color>", textStyle)){
			Application.LoadLevel("Main_Menu");
		}
		textStyle.fontSize = Mathf.RoundToInt (textStyle.fontSize * 0.8f);//shrink the font a smidge so it fits on screen
		GUI.Label (new Rect(0, Screen.height - 100 - findUsOnFacebook.height - 60, Screen.width, 50), 
		           "<color=#ffffff><i>Share your score!</i></color>", textStyle);
		textStyle.fontSize = Screen.width / 10;
        if( FB.IsLoggedIn ) {
            if( GUI.Button( new Rect( Screen.width / 2 - findUsOnFacebook.width / 2, Screen.height - 110 - findUsOnFacebook.height, 
                findUsOnFacebook.width, findUsOnFacebook.height ), PostToFB, imageStyle ) ) 
            {
                FB.Feed(
                    linkCaption: "I just scored " + Scores.total.ToString() + " in an epic bar fight.  Think you can do better?",
                    picture: "http://i.imgur.com/S11tl0R.png",
                    linkName: "Check out Bar Crawl greatness on Facebook!!",
                    link: "https://www.facebook.com/BarCrawlANightToForget"
                    );
            }
        } else {
            if( GUI.Button( new Rect( Screen.width / 2 - findUsOnFacebook.width / 2, Screen.height - 110 - findUsOnFacebook.height,
                findUsOnFacebook.width, findUsOnFacebook.height ), LogIn, imageStyle ) ) 
            {
                 FB.Login( "email,publish_actions", LoginCallback );
            }
        }

	}
    void LoginCallback( FBResult result ) {
        Debug.Log( "LoginCallback" );
        if( FB.IsLoggedIn ) {
            OnLoggedIn();
        } else {
            Debug.Log( "login failed." );
        }
    }

    void OnLoggedIn() {
        if( FB.IsLoggedIn ) {
            Debug.Log( "Logged in. ID: " + FB.UserId );
            
        }
        
    }  

}
