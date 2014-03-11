using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {


	public int x;
	public int y;

	public int health;

	public bool isPassable = false;

	//for display & fight logic purposes.
	protected Move facing;

	// Use this for initialization
	// Start MUST be called by superclasses!
	protected virtual void Start () {
		GridManager.instance.entities.Add(this);
	}
	
	// Update is called once per frame
	// At the moment, Update need not be called by superclasses.  And indeed, cannot.
	void Update() {

	}

	/// <summary>
	/// Attempts to execute the specified move.  If there is something in the way, will not change position, but will change facing.
	/// </summary>
	/// <param name="move">The move to execute.</param>
	protected void AttemptMove(Move move)
	{
		//find the destination tile
		Vector2 moveDir = move.getDirection();
		int destX = x + (int)moveDir.x;
		int destY = y + (int)moveDir.y;


		if(move != Move.Fight){

			if(GridManager.instance.isPassable(destX, destY) && gameObject.tag != "enemy")
			{
				//moves the player if there is no obstacle
				Debug.Log ("moved");
				x = destX;
				y = destY;
				Vector2 dest = GridManager.getTransformPosition(x, y);
				transform.position = new Vector3(dest.x, dest.y, -1);
				facing = move;
			}
			else if(gameObject.tag != "enemy")
			{
				//the player will still turn to face the obstacle
				facing = move;
			}
			else if(gameObject.tag == "enemy")
			{
				//enemies will attempt to move around obstacles if they are in the way
				Vector2[] moveOrder = move.attackOrder ();
				for(int i = 0; i < 4; i++){
					destX = x + (int)moveOrder[i].x;
					destY = y + (int)moveOrder[i].y;

					//they will not make U-turns
					if(GridManager.instance.isPassable(destX, destY) && !moveOrder[i].Equals (-1*facing.getDirection ()))
					{
						Debug.Log ("moved");
						x = destX;
						y = destY;
						Vector2 dest = GridManager.getTransformPosition(x, y);
						transform.position = new Vector3(dest.x, dest.y, -1);
						facing.getMove (moveOrder[i]);
						i = 4;
					}
				}
			}



		}
		else{
			//handles fighting
			//this function orders the tiles so that the entity will attack those in front of it before ones to the side
			Vector2[] fightOrder = facing.attackOrder ();

			for(int i = 0; i < 4; i++){

				Vector2 fightDir = fightOrder[i];
				destX = x + (int)fightDir.x;
				destY = y + (int)fightDir.y;
				if(GridManager.instance.getTarget(destX, destY) != null){
					if((gameObject.tag == "Player" && GridManager.instance.getTarget(destX, destY).gameObject.tag == "enemy") ||
					   (gameObject.tag == "enemy" && GridManager.instance.getTarget (destX, destY).gameObject.tag == "Player")){
						//at this point, we have confirmed there is a fightable entity in this tile
						//Attack animation code should go somewhere under this if.

						GridManager.instance.getTarget (destX, destY).health -= 10;
						Debug.Log (gameObject.name + " attacks! " + GridManager.instance.getTarget (destX, destY).gameObject.name + " takes 10 damage, and has " +
						           GridManager.instance.getTarget(destX, destY).health + " remaining!");


						facing = moveExtensions.getMove(fightOrder[i]);
					}
				}
			}
		}
	}

	/// <summary>
	/// Kills the entity.  Call this to make sure its deconstruction logic is done.
	/// </summary>
	public void Die()
	{
		//right now, that consists of removing it from the GridManager's entity list.

		GridManager.instance.entities.Remove(this);
		Debug.Log (gameObject.name + " has been defeated!");
		Destroy (this.gameObject);
	}
}
