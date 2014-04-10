using UnityEngine;
using System.Collections;

public class RangedEnemy : Enemy {

	// Use this for initialization
	protected override void Start () {

		health = 10;
		damageDealt = 5;
		range = 3;

		base.Start();
	}
	
	// this is fairly pointless...
	protected override void Update()
	{
		base.Update ();

	}
	
}
