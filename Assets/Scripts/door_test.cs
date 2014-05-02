using UnityEngine;
using System.Collections;

public class door_test : MonoBehaviour {

	private Animator animator;

	// Use this for initialization
	void Start () {
	

		animator = gameObject.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
	
		if(Input.GetKeyDown("["))
			animator.Play("door_open");
		if(Input.GetKeyDown("]"))
		   animator.Play("door_close");
	}
}
