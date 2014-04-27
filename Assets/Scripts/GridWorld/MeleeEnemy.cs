using UnityEngine;
using System.Collections;

public class MeleeEnemy : Enemy {

	// Use this for initialization
	protected override void Start () {
		health = 15;

		damageDealt = 10;
		range = 1;
		//these are the defaults anyway, but w/e.

		base.Start();
	}
	
	// Update is called once per frame
	protected override void Update()
	{
		base.Update ();
	}


}
