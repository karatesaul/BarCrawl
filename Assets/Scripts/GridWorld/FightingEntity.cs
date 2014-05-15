using UnityEngine;
using System.Collections;

public class FightingEntity : Entity {

	// at the moment there are no non-fighting entities, but whatever.
	// anyway this exists because I realized ranged fighting code also worked for melee, and thus we can have good inheritance for enemies without a diamond problem.

	/// <summary>
	/// The tag assigned to entities that this entity can fight.
	/// </summary>
	protected string foeTag;

	
	/// <summary>
	/// The damage dealt.  Defaults to 10.
	/// </summary>
	protected int damageDealt = 10;

	/// <summary>
	/// The range of attacks.  Defaults to 1, AKA melee.
	/// </summary>
	protected int range = 1;
	
	//used for animation stuff
	
	protected override void Start(){
		base.Start ();

		//sprite renderer
	}
	
	/// <summary>
	/// Attempts to execute the specified move. If there is something in the way, will not change position, but will change facing.
	/// In FightingEntity, this handles fighting as well as movement.
	/// </summary>
	/// <returns><c>true</c>, if the move was successfully executed, <c>false</c> otherwise.</returns>
	/// <param name="move">The move to execute.</param>
	protected override bool AttemptMove(Move move)
	{
		if (move != Move.Fight)
		{
			return base.AttemptMove(move);
		}
		
		//Debug.Log (gameObject.name + " attempts to fight!");

		//handles fighting
		//this function orders the tiles so that the entity will attack those in front of it before ones to the side
		Vector2[] fightOrder = facing.attackOrder ();
		animator.Play("AttackLeft");
		for(int i = 0; i < 4; i++)
		{
			
			for(int j = 1; j <= range; j++)
			{
				
				Vector2 fightDir = fightOrder[i];
				int destX = x + ((int)fightDir.x * j);
				int destY = y + ((int)fightDir.y * j);
				
				Entity target = GridManager.instance.getTarget(destX, destY);
				if(target != null){
					if(target.gameObject.tag == foeTag){
						//at this point, we have confirmed there is a fightable entity in this tile.
						//Attack animation code should go somewhere under this if.
						
						target.takeDamage(damageDealt);
						
						facing = moveExtensions.getMove(fightOrder[i]);
						if(facing == Move.Left || facing == Move.Right)
						{
							animator.Play("AttackLeft");
						}
						else if(facing == Move.Down)
						{
							animator.Play("AttackDown");
						}
						else if(facing == Move.Up)
						{
							animator.Play("AttackUp");
						}
						
						return true;
					}
				}
				
				//this code, if uncommented, would prevent ranged attacks through obstacles/other entities.  
				//(however, if it reaches this point, the attack would be attempted but would fail; no other move would be substituted.)
				//at the moment, though, we don't care.
				/*
				if(!GridManager.instance.isPassable(destX, destY))
				{
					continue;
				}
				//*/
			}
		}

		return false;  //no target was found.
	}

}
