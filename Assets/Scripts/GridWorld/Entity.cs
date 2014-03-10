using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {


	public int x;
	public int y;

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

		if(GridManager.instance.isPassable(destX, destY))
		{
			Debug.Log ("moved");
			x = destX;
			y = destY;
			Vector2 dest = GridManager.getTransformPosition(x, y);
			transform.position = new Vector3(dest.x, dest.y, -1);
		}

		lastMove = move;
	}

	/// <summary>
	/// Kills the entity.  Call this to make sure its deconstruction logic is done.
	/// </summary>
	public void Die()
	{
		//right now, that consists of removing it from the GridManager's entity list.

		GridManager.instance.entities.Remove(this);
	}
}
