using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {


	public int x;
	public int y;

	public bool isPassable = false;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
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
			x = destX;
			y = destY;
			Vector2 dest = GridManager.getTransformPosition(x, y);
			transform.position = new Vector3(dest.x, dest.y, -1);
		}
	}
}
