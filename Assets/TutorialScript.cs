using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialScript : MonoBehaviour {

	private GameObject menu;
	private GameObject tut1;
	private GameObject tut2;

	public bool tutorial;
	public bool t1;
	public bool t2;

	IEnumerator Wait(GameObject label) {
		Debug.Log("WAITING");
		yield return new WaitForSeconds(5.0f); // waits 5 seconds
		label.SetActive(false);
	}

	// Use this for initialization
	void Start () {
		menu = GameObject.Find("MainMenu");
		tut1 = GameObject.Find("tutorial");
		tut2 = GameObject.Find("tutorial2");

		t1 = false;
		t2 = false;

		//tutorial = menu.GetComponent<MainMenu>().tutorial;

	}
	
	// Update is called once per frame
	void Update () {
		//tutorial = menu.GetComponent<MainMenu>().tutorial;
		if (!t1 && tutorial) t1 = true;
		//t2 = false;

		if (menu.GetComponent<MainMenu>().displayMenu == false) {

			if (tutorial) {
				Destroy (tut1, 8.0F);
				t2 = true;
				//tut2.SetActive(true);
				//Destroy (tut2, 4.0F);
			}


			//if (menu.GetComponent<MainMenu>().tutorial == true) {
				Debug.Log("***in tutorial***");

				//t1 = true;
				//Destroy (tut1, 4.0F);
			//StartCoroutine(Wait(tut1));
			//tutorial = false;

			//tut2.SetActive(true);
			//StartCoroutine(Wait(tut2));	
			//Destroy (tut2, 4.0F);

			//}
			//menu.GetComponent<MainMenu>().tutorial = false;
		}

	}
}
