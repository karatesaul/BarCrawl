using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridManager{

	// just to be clear:
	// the positive x-axis is RIGHT
	// the positive y-axis is UP
	//so (0,0) should be the bottom-left.  Unless we allow negative tiles, which we probably don't.

	public List<Entity> entities;

	public Map map;
	//this really wants to be serialized,
	//but apparently unity can't serialize 2D arrays.
	//we could make a "map row" object that just holds an array of tiles.
	//it seems stupid, but it'll WORK.
	//,,,not doing that yet though.  largely cause I don't like it.

	//consts, just in case, but these shouldn't need changing
	public const float tileSizeX = .5f;
	public const float tileSizeY = .5f;

	// singleton behaviour
	private static GridManager m_instance;
	public static GridManager instance
	{
		get
		{
			if(m_instance == null)
			{
				m_instance = new GridManager();
			}

			return m_instance;
		}
	}

	GridManager() {
		entities = new List<Entity>();
	}

	/// <summary>
	/// Checks if the specified tile is currently passable.
	/// </summary>
	/// <returns><c>true</c>, if it is currently passable, <c>false</c> otherwise.</returns>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	public bool isPassable(int x, int y)
	{
		if (!isPassableMapOnly (x, y))
			return false;

		foreach (Entity entity in entities) {
			if(entity.x == x && entity.y == y)
			{
				return false;
			}
		}

		return true;
	}

	//For combat
	public Entity getTarget(int x, int y)
	{
		if (!isPassableMapOnly (x, y))
			return null;
		foreach (Entity entity in entities) {
			if(entity.x == x && entity.y == y)
			{
				return entity;
			}
		}
		
		return null;
	}

	/// <summary>
	/// Checks if the specified tile is passable, ignoring entities.
	/// </summary>
	/// <returns><c>true</c>, if it is passable, <c>false</c> otherwise.</returns>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	public bool isPassableMapOnly(int x, int y)
	{
		if (x < 0)
			return false;
		if (y < 0)
			return false;
		if (x >= map.width)
			return false;
		if (y >= map.height)
			return false;

		if(map[x,y].isPassable)
			return true;

		return false;
	}

	/// <summary>
	/// Gets the transform position for the tile at (x, y).
	/// </summary>
	/// <returns>The transform position.</returns>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	public static Vector2 getTransformPosition(int x, int y)
	{
		Vector2 result = new Vector2(x * tileSizeX, y * tileSizeY);
		return result;
	}

	/// <summary>
	/// Gets the x coordinate of the tile that the indicated Vector2 is in.
	/// </summary>
	/// <returns>The x coordinate.</returns>
	/// <param name="transformPosition">Transform position.</param>
	public static int getX(Vector2 transformPosition)
	{
		float x = transformPosition.x / tileSizeX;
		return (int)System.Math.Round(x);
	}

	/// <summary>
	/// Gets the y coordinate of the tile that the indicated Vector2 is in.
	/// </summary>
	/// <returns>The y coordinate.</returns>
	/// <param name="transformPosition">Transform position.</param>
	public static int getY(Vector2 transformPosition)
	{
		float y = transformPosition.y / tileSizeY;
		return (int)System.Math.Round(y);
	}

	/// <summary>
	/// Gets the x and y coordinates of the tile that transformPosition is in.
	/// </summary>
	/// <returns>The coordinates as a Vector2.  It should be safe to cast them to ints.</returns>
	/// <param name="transformPosition">Transform position.</param>
	public static Vector2 getTilePosition(Vector2 transformPosition)
	{
		float x = getX(transformPosition);
		float y = getY(transformPosition);
		return new Vector2(x,y);
	}

	public void clearEntities()
	{
		entities.Clear();
	}

	/// <summary>
	/// Returns all entities in the defined rectangle, edges included.
	/// </summary>
	/// <returns>The entities in the rectangle.</returns>
	/// <param name="leftX">Left x.</param>
	/// <param name="rightX">Right x.</param>
	/// <param name="topY">Top y.</param>
	/// <param name="bottomY">Bottom y.</param>
	public List<Entity> getEntitiesInRect(int leftX, int rightX, int topY, int bottomY)
	{
		List<Entity> result = new List<Entity> ();

		foreach(Entity entity in entities)
		{
			if(entity.x <= rightX && entity.x >= leftX &&
			   entity.y <= topY   && entity.y >= bottomY)
				result.Add(entity);
		}

		return result;
	}
}
