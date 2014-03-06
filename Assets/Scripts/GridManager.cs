using UnityEngine;
using System.Collections;

public class GridManager : MonoBehaviour {

	// singleton behaviour

	// although... this doesn't do anything yet
	// really the class does almost nothing yet

	static GridManager m_instance;
	static GridManager instance
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

	/// <summary>
	/// Gets the transform position for the tile at (x, y).
	/// </summary>
	/// <returns>The transform position.</returns>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	static Vector2 getTransformPosition(int x, int y)
	{
		Vector2 result = new Vector2(x * 0.5f, y * 0.5f);
		return result;
	}

	/// <summary>
	/// Gets the x coordinate of the tile that the indicated Vector2 is in.
	/// </summary>
	/// <returns>The x coordinate.</returns>
	/// <param name="transformPosition">Transform position.</param>
	static int getX(Vector2 transformPosition)
	{
		float x = transformPosition.x * 2;
		return (int)System.Math.Round(x);
	}

	/// <summary>
	/// Gets the y coordinate of the tile that the indicated Vector2 is in.
	/// </summary>
	/// <returns>The y coordinate.</returns>
	/// <param name="transformPosition">Transform position.</param>
	static int getY(Vector2 transformPosition)
	{
		float y = transformPosition.y * 2;
		return (int)System.Math.Round(y);
	}

	/// <summary>
	/// Gets the x and y coordinates of the tile that transformPosition is in.
	/// </summary>
	/// <returns>The coordinates as a Vector2.  It should be safe to cast them to ints.</returns>
	/// <param name="transformPosition">Transform position.</param>
	static Vector2 getTilePosition(Vector2 transformPosition)
	{
		float x = getX(transformPosition);
		float y = getY(transformPosition);
		return new Vector2(x,y);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
