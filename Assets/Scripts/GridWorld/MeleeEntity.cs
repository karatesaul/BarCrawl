using UnityEngine;
using System.Collections;

public class MeleeEntity : Entity {

	/// <summary>
	/// The tag assigned to entities that this entity can fight.
	/// </summary>
	protected string foeTag;


	/// <summary>
	/// The damage dealt.  Defaults to 10.
	/// </summary>
	protected int damageDealt = 10;

	//used for animation stuff

	protected override void Start(){
		base.Start ();
		//sprite renderer
	}

	/// <summary>
	/// Attempts to execute the specified move. If there is something in the way, will not change position, but will change facing.
	/// In MeleeEntity, this handles melee fighting as well as movement.
	/// </summary>
	/// <param name="move">The move to execute.</param>
	protected override void AttemptMove(Move move)
	{
		if (move != Move.Fight)
		{
			base.AttemptMove(move);
			return;
		}

		Debug.Log (gameObject.name + "attempts to fight!");
		//handles fighting
		//this function orders the tiles so that the entity will attack those in front of it before ones to the side
		Vector2[] fightOrder = facing.attackOrder ();
		
		for(int i = 0; i < 4; i++){
			
			Vector2 fightDir = fightOrder[i];
			int destX = x + (int)fightDir.x;
			int destY = y + (int)fightDir.y;

			Entity target = GridManager.instance.getTarget(destX, destY);
			if(target != null){
				if(target.gameObject.tag == foeTag){
					//at this point, we have confirmed there is a fightable entity in this tile.
					//Attack animation code should go somewhere under this if.

					GridManager.instance.getTarget (destX, destY).health -= damageDealt;
					//change color at the same time
					
					GridManager.instance.getTarget (destX, destY).currentRed = 100;


					facing = moveExtensions.getMove(fightOrder[i]);

					Debug.Log (gameObject.name + " attacks! " + GridManager.instance.getTarget (destX, destY).gameObject.name + " takes " + damageDealt + " damage, and has " +
					           GridManager.instance.getTarget(destX, destY).health + " health remaining!");

					break;
				}
			}
		}


	}
}
