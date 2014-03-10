using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {


	public int x;
	public int y;

	public int health;

	public bool isPassable = false;

	//for display & fight logic purposes.
	protected Move lastMove;

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
	/// Attempts to execute the specified move.  If there is something in the way, will not change position.
	/// Does not handle Fight, should be overriden for Fight behaviour.
	/// </summary>
	/// <param name="move">The move to execute.</param>
	protected void AttemptMove(Move move)
	{
		Vector2 moveDir = move.getDirection();
		int destX = x + (int)moveDir.x;
		int destY = y + (int)moveDir.y;
		if(move != Move.Fight){
			lastMove = move;
			if(GridManager.instance.isPassable(destX, destY))
			{
				Debug.Log ("moved");
				x = destX;
				y = destY;
				Vector2 dest = GridManager.getTransformPosition(x, y);
				transform.position = new Vector3(dest.x, dest.y, -1);
			}
			else if(gameObject.tag == "enemy"){
				Vector2[] moveOrder = move.attackOrder ();
				for(int i = 1; i < 4; i++){
					destX = x + (int)moveOrder[i].x;
					destY = y + (int)moveOrder[i].y;
					if(GridManager.instance.isPassable(destX, destY))
					{
						Debug.Log ("moved");
						x = destX;
						y = destY;
						Vector2 dest = GridManager.getTransformPosition(x, y);
						transform.position = new Vector3(dest.x, dest.y, -1);
						i = 4;
					}
				}
			}
		}
		else{
			Vector2[] fightOrder = move.attackOrder ();
			//Fight code starts here
			//If target is straight ahead of player
			for(int i = 0; i < 4; i++){
				Vector2 fightDir = fightOrder[i];
				destX = x + (int)fightDir.x;
				destY = y + (int)fightDir.y;
				if(GridManager.instance.getTarget(destX, destY) != null){
					if((gameObject.tag == "Player" && GridManager.instance.getTarget(destX, destY).gameObject.tag == "enemy") ||
					   (gameObject.tag == "enemy" && GridManager.instance.getTarget (destX, destY).gameObject.tag == "Player")){
						//Attack animation code goes here.
						GridManager.instance.getTarget (destX, destY).health -= 10;
						Debug.Log (gameObject.name + " attacks! " + GridManager.instance.getTarget (destX, destY).gameObject.name + " takes 10 damage, and has " +
						           GridManager.instance.getTarget(destX, destY).health + " remaining!");
						return;
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
		Destroy (this);
	}
}
